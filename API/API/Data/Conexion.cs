using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using Oracle.ManagedDataAccess.Client;
using API.Models;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace API.Data
{
   
    public class Conexion
    {
        private string cadenaSQL;
        private int valExe = 1;
        private OracleConnection conexion = new OracleConnection();
        DataTable tbl = new DataTable();

        public Conexion()
        {
            open();
        }
        public void open()
        {
            var configuration = GetConfiguration();
            cadenaSQL = configuration.GetConnectionString("DefaultConnection");


            conexion.ConnectionString = cadenaSQL;
            conexion.Open();
        }
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        public void close()
        {
            conexion.Close();
        }
        public DataTable QueryData(string sql)
        {
            OracleCommand command = new OracleCommand(sql, conexion);
            OracleDataAdapter data = new OracleDataAdapter(command);
            data.Fill(tbl);

            return tbl;
        }
        public int execute(string sql)
        {
            
            OracleCommand command = new OracleCommand(sql, conexion);
            valExe = command.ExecuteNonQuery();

            return valExe;
        }
    }
}
