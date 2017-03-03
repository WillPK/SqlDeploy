using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SqlDeploy.ConsoleApp
{
    public class SqlCommandExecutor
    {
        static readonly string DbConnection = MyConfigManager.GetDbConnection();

        public static object ExecuteScalar(string script)
        {
            var connection = new SqlConnection(DbConnection);

            try
            {
                SqlCommand command = new SqlCommand(script, connection);

                connection.Open();

                return command.ExecuteScalar();
            }
            finally
            {
                connection.Close();
            }
        }
        
        public static void ExecuteNonQuery(string script, params SqlParameter[] parameters)
        {
            var connection = new SqlConnection(DbConnection);

            try
            {
                SqlCommand command = new SqlCommand
                {
                    Connection = connection
                };
                
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                connection.Open();

                foreach (var scriptBatch in script.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (string.IsNullOrWhiteSpace(scriptBatch))
                    {
                        continue;
                    }

                    command.CommandText = scriptBatch;
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
