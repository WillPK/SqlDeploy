using System;
using System.Configuration;

namespace SqlDeploy.ConsoleApp
{
    public class MyConfigManager
    {
        public static string GetDbConnection()
        {
            return GetConnection(Constants.AppSettingKeyFor.DbConnection);
        }

        public static string GetVersionTableName()
        {
            return GetAppSetting(Constants.AppSettingKeyFor.VersionTableName);
        }

        public static string GetRootFolder()
        {
            return GetAppSetting(Constants.AppSettingKeyFor.RootFolder);
        }

        private static string GetAppSetting(string configKey)
        {
            var result = Convert.ToString(ConfigurationManager.AppSettings[configKey]);

            if (string.IsNullOrEmpty(result))
            {
                Console.WriteLine("Config key missing: {0}.", configKey);
            }

            return result;
        }

        private static string GetConnection(string name)
        {
            var result = Convert.ToString(ConfigurationManager.ConnectionStrings[name]);

            if (string.IsNullOrEmpty(result))
            {
                Console.WriteLine("Connection key missing: {0}.", name);
            }

            return result;
        }
    }
}
