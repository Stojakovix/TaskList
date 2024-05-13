using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskList.Data
{
    public static class Constants
    {
        public const string DatabaseFilename = "ToDoDB.db3";
        public static string dbPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(dbPath, DatabaseFilename);

    }
}
