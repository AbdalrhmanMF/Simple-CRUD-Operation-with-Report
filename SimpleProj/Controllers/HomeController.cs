using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using SimpleProj.Models;
using SimpleProj.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SimpleProj.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment webHost;
        private readonly ApplicationDBContext _context;
        GenereServices services;

        public HomeController(ILogger<HomeController> logger, ApplicationDBContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _logger = logger;
            this.webHost = webHost;
            services = new GenereServices(context);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }





        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        public IActionResult GenersInfo()
        {
            var dt = services.GetGenersInfo();

            string mimType = string.Empty;
            int extention = 1;


            //string solutiondir = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            string solutiondir = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;// ==> GET DIRECTION IN OTHER PROJECT 
            // may need to go one directory higher for solution directory

            var path = $"{solutiondir}\\RDLCSolution\\Reports\\Code\\rptGenersInfo.rdlc";

            //var path = $"{webHost.WebRootPath}\\Reports\\Code\\rptGenersInfo.rdlc"; // ==> GET DIRECTION IN SAME PROJECT 

            Dictionary<string, string> paras = new()
            {
                { "param1", "RDLC Report" },
                { "param2", DateTime.Now.ToString("yyyy-MM-dd") },
                { "param3", "Geners Info" }
            };

            LocalReport localReport = new(path);
            localReport.AddDataSource("SimpleData", dt);

            var res = localReport.Execute(RenderType.Pdf, extention, paras, mimType);

            return File(res.MainStream, "application/pdf");
        }

    }
}
