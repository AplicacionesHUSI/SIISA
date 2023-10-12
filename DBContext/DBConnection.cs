using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

namespace HUSI_SIISA.DBContext
{
#pragma warning disable CS1591
    public class DBConnection:DbContext
    {
        String cs;
        public DBConnection()
        {
            SqlConnectionStringBuilder connection = new();
            connection.DataSource = "WinTFDB05";
            connection.InitialCatalog = "SAHI";
            connection.UserID = "husi_usr";
            connection.Password = "pwdHUSI";
            connection.TrustServerCertificate = true;
            connection.MultipleActiveResultSets = true;
            cs = connection.ConnectionString;
        }

        public String getCs()
        {
            return cs;
        }
    }
#pragma warning restore CS1591
}
