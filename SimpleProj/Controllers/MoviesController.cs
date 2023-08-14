using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using SimpleProj.Models;
using SimpleProj.Services;
using SimpleProj.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleProj.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IToastNotification _notify;
        private List<string> _allowedExitention = new List<string> { ".jpg", ".png" };
        private long _maxPosterSize = 1048576;
        public MoviesController(ApplicationDBContext context, IToastNotification notify)
        {
            _context = context;
            _notify = notify;
        }


        public async Task<IActionResult> Index()
        {
            var model = await _context.Movies.OrderByDescending(x => x.Rate).ToListAsync();
            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            var model = new MoviesVM
            {
                Genres = await _context.Genres.ToListAsync()
            };
            return View("Form", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MoviesVM model)
        {
            model.Code = model.Id.ToString();
            model.ArabicName = model.Title.ToString();
            model.EnglishName = model.Title.ToString();

            var files = Request.Form.Files;
            if (!files.Any())
            {
                ModelState.AddModelError("Poster", "Please Select Movie poster!");
            }
            var poster = files.FirstOrDefault();
            if (!_allowedExitention.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {
                ModelState.AddModelError("Poster", "Only .png, .jpg images are allowed!");
            }
            if (poster.Length > _maxPosterSize)
            {
                ModelState.AddModelError("Poster", "Poster cannot be more than 1 MB!");
            }

            //if (!ModelState.IsValid)
            //{
            //    return View("Form",model);
            //}

            using var datastream = new MemoryStream();
            await poster.CopyToAsync(datastream);
            var movie = new Movie
            {
                Code = model.Code,
                ArabicName = model.ArabicName,
                EnglishName = model.EnglishName,
                Title = model.Title,
                GenreId = model.GenreId,
                Year = model.Year,
                Rate = model.Rate,
                StoreLine = model.StoreLine,
                Poster = datastream.ToArray()
            };

            _context.Movies.Add(movie);
            _context.SaveChanges();

            _notify.AddSuccessToastMessage("Movie Created Successfully");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();
            var data = await _context.Movies.FindAsync(id);

            if (data == null)
                return NotFound();

            var model = new MoviesVM
            {
                Id = data.Id,
                Code = data.Code,
                ArabicName = data.ArabicName,
                EnglishName = data.EnglishName,
                Title = data.Title,
                GenreId = data.GenreId,
                Rate = data.Rate,
                Year = data.Year,
                StoreLine = data.StoreLine,
                Poster = data.Poster,
                Genres = await _context.Genres.ToListAsync()
            };

            return View("Form", model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MoviesVM model)
        {
            model.Genres = await _context.Genres.ToListAsync();

            //if (!ModelState.IsValid)
            //{
            //    return View("Form", model);
            //}


            var movie = await _context.Movies.FindAsync(model.Id);

            if (movie == null)
            {
                return NotFound();
            }

            var files = Request.Form.Files;

            if (files.Any())
            {
                var poster = files.FirstOrDefault();

                using var datastream = new MemoryStream();

                await poster.CopyToAsync(datastream);

                model.Poster = datastream.ToArray();

                if (!_allowedExitention.Contains(Path.GetExtension(poster.FileName).ToLower()))
                {
                    ModelState.AddModelError("Poster", "Only .png, .jpg images are allowed!");
                    return View("Form", model);
                }
                if (poster.Length > _maxPosterSize)
                {
                    ModelState.AddModelError("Poster", "Poster cannot be more than 1 MB!");
                    return View("Form", model);
                }
                movie.Poster = model.Poster;

            }

            //movie.Id = model.Id;
            movie.Code = model.Id.ToString();
            movie.ArabicName = model.Title;
            movie.EnglishName = model.Title;
            movie.Title = model.Title;
            movie.GenreId = model.GenreId;
            movie.Rate = model.Rate;
            movie.Year = model.Year;
            movie.StoreLine = model.StoreLine;


            _context.SaveChanges();

            _notify.AddSuccessToastMessage("Movie Updated Successfully");

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)
                return BadRequest();

            var model = await _context.Movies.Include(x => x.Genre).SingleOrDefaultAsync(x => x.Id == Id);
            if (model == null)
                return NotFound();

            return View(model);
        }


        public async Task<IActionResult> delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var model = await _context.Movies.FindAsync(id);

            if (model == null)
                return NotFound();

            _context.Movies.Remove(model);
            _context.SaveChanges();

            return Ok();

        }

    }
}
