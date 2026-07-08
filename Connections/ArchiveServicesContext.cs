using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymApplicationV2._0.Connections
{
    public class ArchiveServicesContext
    {
        public static string ConnectionStringArchive()
        {
            return "Data Source=Databases\\Archive.db;Version=3";
        }

        public static void CreatingDatabase()
        {
            SQLiteConnection.CreateFile("Databases\\Archive.db");

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionStringArchive()))
            {
                string commandString = @"
                    CREATE TABLE Archive(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Клиент TEXT(100),
                        №Карты TEXT(20),
                        Дата_окончания TEXT(20),
                        Абонемент TEXT(100),
                        Оплата INTEGER,
                        Посещений_осталось INTEGER CHECK(Посещений_осталось BETWEEN 0 AND 100)
                )";

                using (SQLiteCommand cmd = new SQLiteCommand(commandString, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
