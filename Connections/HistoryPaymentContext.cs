using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymApplicationV2._0.Connections
{
    public class HistoryPaymentContext
    {
        public static string ConnectionStringPayment()
        {
            return "Data Source=Databases\\Payments.db;Version=3";
        }

        public static void CreatingDatabase()
        {
            SQLiteConnection.CreateFile("Databases\\Payments.db");

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionStringPayment()))
            {
                string commandString = @"
                    CREATE TABLE History(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Клиент TEXT(100),
                        Абонемент TEXT(100),
                        Дата_начала TEXT(20),
                        Дата_окончания TEXT(20),
                        Цена INTEGER,
                        Дата_платежа TEXT(20)
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
