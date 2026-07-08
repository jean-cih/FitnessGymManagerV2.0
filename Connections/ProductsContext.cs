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
    public class ProductsContext
    {
        public static string ConnectionStringProducts()
        {
            return "Data Source=Databases\\Products.db;Version=3";
        }

        public static void CreatingDatabase()
        {
            SQLiteConnection.CreateFile("Databases\\Products.db");

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionStringProducts()))
            {
                string commandString = @"
                    CREATE TABLE Items(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Товары TEXT(100),
                        Цена TEXT(20),
                        Количество INTEGER,
                        Время_продажи TEXT(20)
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
