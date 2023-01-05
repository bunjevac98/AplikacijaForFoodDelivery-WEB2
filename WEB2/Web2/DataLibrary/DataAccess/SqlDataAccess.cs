using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataLibrary.DataAccess
{
    public static class SqlDataAccess
    {
        //vraca mi string za citanje iz baze
        public static string GetConnectionString(string connectionName = "MVCDemoDB") {

            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }
        //ovde menjao
        public static List<T> UcitajPodatak<T>(string sql) {

            using (IDbConnection cnn = new SqlConnection(GetConnectionString())) {

                return cnn.Query<T>(sql).ToList();
            }

        }
        // POLJA MI SE NALAZE U OVOM DATA PA AKO ZELIM NESTO DA MENJAM TAMO CU PRONACI SVE STO MI TREBA SQL MI VRACA SAMO BAZU
        public static int SacuvajPodatak<T>(string sql, T data) {

            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Execute(sql, data);
                

            }
        }
        

        public static int IzmeniPodatak<T>(string sql, T data)
        {

            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Execute(sql,data);
                
            }
        }


    }
}
