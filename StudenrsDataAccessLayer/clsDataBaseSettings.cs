using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsDataAccessLayer
{
    public static class clsDataBaseSettings
    {
        public static string SqlServerConnectionString
        {
            get

            { return "Server=.;Database=StudentDB;User Id=sa;Password=sa123456;TrustServerCertificate=True;";   }

        }
    }
}
