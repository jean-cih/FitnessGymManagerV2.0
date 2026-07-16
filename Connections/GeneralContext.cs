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

        public static void FormatData(DataGridView dataGridView)
        {
            string[] dateColumns = { "Сохранено", "Дата окончания", "Дата рождения", "Дата начала", "Дата платежа", "Окончание заморозки" };
            foreach (string columnName in dateColumns)
            {
                if (dataGridView.Columns.Contains(columnName))
                {
                    dataGridView.Columns[columnName].DefaultCellStyle.Format = "dd.MM.yyyy";
                    dataGridView.Columns[columnName].DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("ru-RU");
                }
            }

            string[] fullDateColumns = { "Дата оформления", "Посетил", "Время продажи" };
            foreach (string columnName in fullDateColumns)
            {
                if (dataGridView.Columns.Contains(columnName))
                {
                    dataGridView.Columns[columnName].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss";
                    dataGridView.Columns[columnName].DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("ru-RU");
                }
            }
        }

        public static void FormatDateColumns(DataTable dataTable)
        {
            // Список колонок, которые нужно отформатировать
            string[] dateColumns = { "Сохранено", "Дата окончания", "Дата рождения", "Дата начала", "Дата платежа", "Окончание заморозки", "Дата оформления", "Посетил", "Время продажи" };

            // Используем культуру с форматом dd.MM.yyyy
            var culture = new System.Globalization.CultureInfo("ru-RU");

            foreach (string columnName in dateColumns)
            {
                if (dataTable.Columns.Contains(columnName))
                {
                    // Создаем новую колонку с типом DateTime
                    string newColumnName = columnName + "_temp";
                    dataTable.Columns.Add(newColumnName, typeof(DateTime));

                    // Копируем данные из старой колонки в новую с преобразованием
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row[columnName] != DBNull.Value && row[columnName] != null)
                        {
                            try
                            {
                                DateTime dateValue;
                                object value = row[columnName];

                                // Преобразуем в DateTime
                                if (value is DateTime dt)
                                {
                                    dateValue = dt;
                                }
                                else if (value is string str)
                                {
                                    // Пытаемся распарсить с учетом формата dd.MM.yyyy
                                    if (!DateTime.TryParseExact(str, new[] { "dd.MM.yyyy", "dd.MM.yyyy HH:mm", "dd.MM.yyyy HH:mm:ss" },
                                        culture, System.Globalization.DateTimeStyles.None, out dateValue))
                                    {
                                        // Если не удалось, пытаемся стандартным парсингом
                                        if (!DateTime.TryParse(str, culture, System.Globalization.DateTimeStyles.None, out dateValue))
                                        {
                                            row[newColumnName] = DBNull.Value;
                                            continue;
                                        }
                                    }
                                }
                                else
                                {
                                    dateValue = Convert.ToDateTime(value, culture);
                                }

                                row[newColumnName] = new DateTime(dateValue.Year, dateValue.Month, dateValue.Day,
                                                           dateValue.Hour, dateValue.Minute, dateValue.Second);
                            }
                            catch
                            {
                                row[newColumnName] = DBNull.Value;
                            }
                        }
                        else
                        {
                            row[newColumnName] = DBNull.Value;
                        }
                    }

                    // Удаляем старую колонку
                    dataTable.Columns.Remove(columnName);

                    // Переименовываем новую колонку в имя старой
                    dataTable.Columns[newColumnName].ColumnName = columnName;

                    // Устанавливаем формат отображения для колонки (для DataGridView)
                    dataTable.Columns[columnName].DateTimeMode = DataSetDateTime.Unspecified;
                }
            }
        }
    }
}
