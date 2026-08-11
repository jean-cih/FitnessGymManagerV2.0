using GymApplicationV2._0.Connections;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace GymApplicationV2._0.Helpers
{
    public static class DatabaseMigration
    {
        /// <summary>
        /// Проверяет и конвертирует даты в ISO формат во всех таблицах
        /// </summary>
        public static void MigrateDatesToIsoFormat()
        {
            try
            {
                // Список всех БД для миграции
                var databases = new[]
                {
                    new { Context = ClientsContext.ConnectionStringClients(), Name = "Clients" },
                    new { Context = IssuedMembershipContext.ConnectionStringIssued(), Name = "Issued" },
                    new { Context = HistoryPaymentContext.ConnectionStringPayment(), Name = "Payments" },
                    new { Context = ArchiveServicesContext.ConnectionStringArchive(), Name = "Archive" }
                };

                foreach (var db in databases)
                {
                    MigrateDatabaseDates(db.Context, db.Name);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.MessageWindowOk($"Ошибка при миграции данных: {ex.Message}", "Ошибка");
            }
        }

        private static void MigrateDatabaseDates(string connectionString, string dbName)
        {
            if (!File.Exists(GetDatabasePath(connectionString)))
                return;

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // Получаем список всех таблиц
                var tables = GetTableNames(conn);

                foreach (var table in tables)
                {
                    // Получаем колонки с датами
                    var dateColumns = GetDateColumns(conn, table);

                    if (dateColumns.Count > 0)
                    {
                        MigrateTableDates(conn, table, dateColumns);
                    }
                }
            }
        }

        private static System.Collections.Generic.List<string> GetTableNames(SQLiteConnection conn)
        {
            var tables = new System.Collections.Generic.List<string>();
            using (var cmd = new SQLiteCommand(
                "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%'", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tables.Add(reader["name"].ToString());
                }
            }
            return tables;
        }

        private static System.Collections.Generic.List<string> GetDateColumns(SQLiteConnection conn, string tableName)
        {
            var dateColumns = new System.Collections.Generic.List<string>();

            using (var cmd = new SQLiteCommand($"PRAGMA table_info({tableName})", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string columnName = reader["name"].ToString();
                    string columnType = reader["type"]?.ToString()?.ToLower() ?? "";

                    // Проверяем, является ли колонка датой по имени или типу
                    if (columnType.Contains("date") ||
                        columnType.Contains("time") ||
                        columnName.Contains("Дата") ||
                        columnName.Contains("дата") ||
                        columnName.Contains("Date") ||
                        columnName.Contains("Окончание") ||
                        columnName.Contains("Рождения") ||
                        columnName.Contains("Заморозки"))
                    {
                        dateColumns.Add(columnName);
                    }
                }
            }

            return dateColumns;
        }

        private static void MigrateTableDates(SQLiteConnection conn, string tableName,
            System.Collections.Generic.List<string> dateColumns)
        {
            // Выбираем все данные
            string selectQuery = $"SELECT * FROM {tableName}";
            var dataTable = new DataTable();

            using (var adapter = new SQLiteDataAdapter(selectQuery, conn))
            {
                adapter.Fill(dataTable);
            }

            bool hasChanges = false;

            // Проходим по каждой строке
            foreach (DataRow row in dataTable.Rows)
            {
                foreach (string colName in dateColumns)
                {
                    if (row[colName] != DBNull.Value && row[colName] != null)
                    {
                        string value = row[colName].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            string converted = TryConvertToIsoDate(value);
                            if (converted != value)
                            {
                                row[colName] = converted;
                                hasChanges = true;
                            }
                        }
                    }
                }
            }

            // Если есть изменения, обновляем БД
            if (hasChanges)
            {
                using (var adapter = new SQLiteDataAdapter(selectQuery, conn))
                {
                    var builder = new SQLiteCommandBuilder(adapter);
                    adapter.Update(dataTable);
                }
            }
        }

        private static string TryConvertToIsoDate(string dateString)
        {
            // Если уже в ISO формате
            if (IsIsoFormat(dateString))
                return dateString;

            // Пробуем распарсить в разных форматах
            string[] formats = new[]
            {
                "dd.MM.yyyy",
                "dd.MM.yy",
                "dd/MM/yyyy",
                "dd/MM/yy",
                "yyyy-MM-dd",
                "yyyy.MM.dd",
                "dd-MM-yyyy",
                "dd-MM-yy",
                "MM/dd/yyyy",
                "M/d/yyyy",
                "dd MMM yyyy",
                "dd MMMM yyyy"
            };

            foreach (var format in formats)
            {
                if (DateTime.TryParseExact(dateString, format,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out DateTime parsedDate))
                {
                    return parsedDate.ToString("yyyy-MM-dd");
                }
            }

            // Если не удалось распарсить, пробуем общий TryParse
            if (DateTime.TryParse(dateString, out DateTime generalDate))
            {
                return generalDate.ToString("yyyy-MM-dd");
            }

            // Если ничего не помогло, возвращаем как есть
            return dateString;
        }

        private static bool IsIsoFormat(string dateString)
        {
            return DateTime.TryParseExact(dateString, "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out _);
        }

        private static string GetDatabasePath(string connectionString)
        {
            // Извлекаем путь из строки подключения
            var parts = connectionString.Split(';');
            foreach (var part in parts)
            {
                if (part.Trim().StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))
                {
                    return part.Substring("Data Source=".Length).Trim();
                }
            }
            return string.Empty;
        }
    }
}