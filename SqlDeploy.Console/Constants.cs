using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDeploy.ConsoleApp
{
    public class Constants
    {
        public static class AppSettingKeyFor
        {
            public static readonly string DbConnection = "DbConnection";
            public static readonly string VersionTableName = "VersionTableName";
            public static readonly string RootFolder = "RootFolder";
        }

        public static class SqlScripts
        {
            public static class VersionTable
            {
                public static readonly string VersionTableName = MyConfigManager.GetVersionTableName();

                public static readonly string CheckExists =
                    $"SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{VersionTableName}'";

                public static readonly string GetLatest =
                    $"Select TOP 1 Version From {VersionTableName} order by 1 desc";
                
                public static readonly string Insert =
                   $"Insert Into {VersionTableName} VALUES(@Version, @Date)";

                public static readonly string Create = $@"
                    CREATE TABLE[dbo].[{VersionTableName}](
	                [Version] [bigint] NOT NULL,
                    [DateTime] [datetime] NOT NULL,
                    CONSTRAINT[PK_{VersionTableName}] PRIMARY KEY CLUSTERED
                    ([Version] ASC
                    )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
                    ) ON[PRIMARY]";
            }
        }
    }
}