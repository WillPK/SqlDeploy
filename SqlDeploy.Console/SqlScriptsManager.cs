using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SqlDeploy.ConsoleApp
{
    public class SqlScriptsManager
    {
        public static List<KeyValuePair<int, FileInfo>> FindAllNewScripts(int? latestVersion)
        {
            var allScripts = FindAll();
            var newScripts = allScripts
                .Where(s => !latestVersion.HasValue || s.Key > latestVersion.Value)
                .ToList();

            Console.WriteLine($"Found '{allScripts.Count}' sql scripts in total '{newScripts.Count}' of which are new to run.");
            
            return newScripts;
        }

        public static List<KeyValuePair<int, FileInfo>> FindAll()
        {
            Console.WriteLine("Loading sql scripts...");

            var files = FindAllFiles();
            
            var result = new Dictionary<int, FileInfo>();
            
            var pattern = MyConfigManager.GetSqlScriptFileFormat();
            foreach (var file in files)
            {
                var match = Regex.Match(file.Name, pattern);

                if (match.Success)
                {
                    var version = Convert.ToInt16(match.Groups[1].Value);

                    if (result.ContainsKey(version))
                    {
                        throw new ApplicationException($"Duplicated key: '{version}'");
                    }

                    result.Add(version, file);
                }
                else
                {
                    throw new ApplicationException($"Incorrect file format: '{file.Name}'");
                }
            }
            
            return result.OrderBy(r => r.Key).ToList();
        }

        private static List<FileInfo> FindAllFiles()
        {
            var rootFolder = MyConfigManager.GetRootFolder();

            if (!Directory.Exists(rootFolder))
            {
                throw new ApplicationException($"Folder does not exist: {rootFolder}");
            }

            var files = Directory.GetFiles(rootFolder).Select(f => new FileInfo(f)).ToList();

            if (files.Count == 0)
            {
                throw new ApplicationException("No scripts were found.");
            }
            return files;
        }
    }
}
