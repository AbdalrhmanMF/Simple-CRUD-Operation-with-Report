using Microsoft.EntityFrameworkCore;
using SimpleProj.Models;
using System.Data;
using System.Data.SqlClient;

namespace SimpleProj.Services
{
    public class GenereServices
    {
        private readonly ApplicationDBContext context;

        private readonly string ConString;

        public GenereServices(ApplicationDBContext context)
        {
            this.context = context;
            ConString = context.Database.GetConnectionString()??string.Empty;
        }


        public DataTable GetGenersInfo()
        {
            DataTable result = new();

            if (!string.IsNullOrEmpty(ConString))
            {
                using (SqlConnection con = new(ConString))
                {
                    var cmd = new SqlCommand("Select * from Genres", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter adapter = new(cmd);
                    adapter.Fill(result);
                    con.Close();
                }
            }

            return result;
        }
    }
}
