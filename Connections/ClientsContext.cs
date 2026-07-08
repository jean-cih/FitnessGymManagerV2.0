using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace GymApplicationV2._0.Connections
{
    public class ClientsContext
    {
        public static string ConnectionStringClients()
        {
            return "Data Source=Databases\\Clients.db;Version=3";
        }

        public static void CreatingDatabase()
        {
            SQLiteConnection.CreateFile("Databases\\Clients.db");

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionStringClients()))
            {
                const string commandString = @"
                    CREATE TABLE Contacts(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Фамилия TEXT(20) NOT NULL,
                        Имя TEXT(20) NOT NULL,
                        Пол TEXT(20) DEFAULT NULL,
                        Телефон TEXT(20) DEFAULT NULL,
                        №Карты TEXT(20),
                        Покупки INTEGER DEFAULT NULL,
                        Отчество TEXT(20) DEFAULT NULL,
                        Email TEXT(100) DEFAULT NULL,
                        Дата_рождения TEXT(20) DEFAULT NULL,
                        Скидка INTEGER DEFAULT 0,
                        Сохранено TEXT(20) NOT NULL
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
