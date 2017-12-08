using LuckyIsland.API.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace LuckyIsland.API.Data
{
    public class DataContexts
    {
        private static string connectionString;
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(DataContexts.connectionString))
                {
                    DataContexts.connectionString = ConfigurationManager.ConnectionStrings["WorkConnection"].ConnectionString;
                }
                return DataContexts.connectionString;
            }
        }

        public static DataDataContext GetDataContext()
        {
            return new DataDataContext(DataContexts.ConnectionString);
        }

        public static void Exec(StringBuilder query)
        {
            using (SqlConnection connection = new SqlConnection(DataContexts.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query.ToString(), connection))
                {
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 30;

                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}