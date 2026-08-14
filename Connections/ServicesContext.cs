using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymApplicationV2._0.Connections
{
    public class ServicesContext
    {
        public static string ConnectionStringServices()
        {
            return "Data Source=Databases\\Services.db;Version=3";
        }

        public static void CreatingDatabase()
        {
            SQLiteConnection.CreateFile("Databases\\Services.db");

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionStringServices()))
            {
                string commandString = @"
                    CREATE TABLE Descriptions(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Абонемент TEXT(100) NOT NULL,
                        Цена INTEGER NOT NULL,
                        Срок_действия INTEGER NOT NULL,
                        Посещений INTEGER NULL,
                        Тип TEXT DEFAULT 'Обычный',
                        Проданных_за_месяц INTEGER DEFAULT 0
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
