using System;
using System.Collections.Generic;
using System.IO;

namespace SqlDeploy.ConsoleApp
{
    class Program
    {
        static readonly VersionTableManager VersionTableManager = new VersionTableManager();

        static void Main(string[] args)
        {
            try
            {
                // get latest version
                VersionTableManager.EnsureExists();
                int? lastVersion = VersionTableManager.GetLastVersion();

                // find and process the new scripts
                SqlScriptsManager.FindAllNewScripts(lastVersion).ForEach(ProcessScript);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                while (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                    ex = ex.InnerException;
                }
            }

            Console.WriteLine("Finished.");
            Console.ReadLine();
        }

        private static void ProcessScript(KeyValuePair<int, FileInfo> sqlScript)
        {
            var fileName = sqlScript.Value.Name;
            try
            {
                // load
                var script = File.ReadAllText(sqlScript.Value.FullName);

                if (string.IsNullOrEmpty(script))
                {
                    throw new ApplicationException("Script is empty");
                }
                
                // run
                SqlCommandExecutor.ExecuteNonQuery(script);

                // add version
                VersionTableManager.InsertVersion(sqlScript.Key);

                Console.WriteLine($"Ran script successfully: '{fileName}'");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to run: '{fileName}'", ex);
            }
        }
    }
}
