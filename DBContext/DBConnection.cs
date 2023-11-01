using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

namespace HUSI_SIISA.DBContext
{
#pragma warning disable CS1591
    public class DBConnection:DbContext
    {
        String cs;
        String cs2;
        public DBConnection()
        {

            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string con = configuration.GetConnectionString("conexion");
            SqlConnectionStringBuilder connection = new(con);
            cs = connection.ConnectionString;

            string con2 = configuration.GetConnectionString("conexionCaps");
            SqlConnectionStringBuilder connection2 = new(con2);
            cs2 = connection2.ConnectionString;
        }

        public String getCs()
        {
            return cs;
        }
        public String getCs2()
        {
            return cs2;
        }
    }
#pragma warning restore CS1591
}
