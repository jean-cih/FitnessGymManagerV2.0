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
    public class GeneralContext
    {
        public static DataTable GetDataFromDatabase(string commandString, string connectionString, params SQLiteParameter[] parameters)
        {
            DataTable dbContacts = new DataTable();

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(commandString, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            dbContacts.Load(reader);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show($"SQLite error in {connectionString}: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"General error in {connectionString}: {ex.Message}");
                return null;
            }

            return dbContacts;
        }

        public static object GetElementFromDatabase(string requireLine, string connectionString, params SQLiteParameter[] parameters)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(requireLine, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.Error.WriteLine($"SQLite error in {connectionString}: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"General error in {connectionString}: {ex.Message}");
                return null;
            }
        }

        public static void CommandDataFromDatabase(string command, string connectionString, params SQLiteParameter[] parameters)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(command, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.Error.WriteLine($"SQLite error in {connectionString}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"General error in {connectionString}: {ex.Message}");
            }
        }
    }
}
