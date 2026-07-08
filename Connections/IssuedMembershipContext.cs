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
    public class IssuedMembershipContext
    {
        public static string ConnectionStringIssued()
        {
            return "Data Source=Databases\\IssuedMembership.db;Version=3";
        }

        public static void CreatingDatabase()
        {
            SQLiteConnection.CreateFile("Databases\\IssuedMembership.db");

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionStringIssued()))
            {
                string commandString = @"
                    CREATE TABLE Issued(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Клиент TEXT(100) NOT NULL,
                        №Карты TEXT(20) NOT NULL,
                        Дата_окончания TEXT(20) NOT NULL,
                        Дата_оформления TEXT(20) NOT NULL,
                        Абонемент TEXT(100) NOT NULL,
                        Посетил TEXT(20),
                        Оплата INTEGER NOT NULL,
                        Статус TEXT(20) NOT NULL,
                        Посещений_осталось TEXT(5),
                        Окончание_заморозки TEXT(20) 
                )";

                using (SQLiteCommand cmd = new SQLiteCommand(commandString, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static IssuedInfo GetIssuedData(string query, params SQLiteParameter[] parameters)
        {
            IssuedInfo issuedInfo = null;

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(ConnectionStringIssued()))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                issuedInfo = new IssuedInfo
                                {
                                    FullName = reader["Клиент"].ToString(),
                                    Membership = reader["Абонемент"].ToString(),
                                    EndDate = reader["Дата_окончания"].ToString(),
                                    VisitsLeft = reader["Посещений_осталось"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show($"SQLite error in GetClientData: {ex.Message}");
                //return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"General error in GetClientData: {ex.Message}");
                //return null;
            }

            return issuedInfo;
        }

        public class IssuedInfo
        {
            public string FullName { get; set; }
            public string Membership { get; set; }
            public string EndDate { get; set; }
            public string VisitsLeft { get; set; }
            public string Price { get; set; }
        }
    }
}
