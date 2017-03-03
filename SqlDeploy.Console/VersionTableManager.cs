using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sqlScripts = SqlDeploy.ConsoleApp.Constants.SqlScripts.VersionTable;

namespace SqlDeploy.ConsoleApp
{
    public class VersionTableManager
    {
        public void EnsureExists()
        {
            if (Exists())
            {
                Console.WriteLine("Version table exists.");
                return;
            }

            Console.WriteLine("Version table does not exists, creating...");

            SqlCommandExecutor.ExecuteNonQuery(sqlScripts.Create);

            Console.WriteLine("Created the version table successfully.");
        }

        private bool Exists()
        {
            Console.WriteLine($"Version table name is {sqlScripts.VersionTableName}");

            var result = SqlCommandExecutor.ExecuteScalar(sqlScripts.CheckExists);

            if (result == null || result == DBNull.Value)
            {
                return false;
            }

            return true;
        }

        public int? GetLastVersion()
        {
            var result = SqlCommandExecutor.ExecuteScalar(sqlScripts.GetLatest);

            if (result == null || result == DBNull.Value)
            {
                Console.WriteLine("No existing version number was found.");
                return null;
            }

            int lastVersion = Convert.ToInt16(result); ;

            Console.WriteLine("Latest version: " + lastVersion);

            return lastVersion;
        }

        public void InsertVersion(int version)
        {
            var versionParam = new SqlParameter("@Version", SqlDbType.BigInt)
            {
                Value = version
            };

            var dateParam = new SqlParameter("@Date", SqlDbType.DateTime)
            {
                Value = DateTime.UtcNow
            };

            SqlCommandExecutor.ExecuteNonQuery(sqlScripts.Insert, versionParam, dateParam);
        }
    }
}
