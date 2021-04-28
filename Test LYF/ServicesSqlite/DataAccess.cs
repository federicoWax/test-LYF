using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data.SQLite;

namespace Test_LYF.ServicesSqlite
{
    public class DataAccess
    {
        private const string DBName = "database.sqlite";
        private const string query = "CREATE TABLE IF NOT EXISTS Payments (" +
         "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
         "Customer VARCHAR(100) NOT NULL," +
         "Account INTEGER NOT NULL," +
         "Debt REAL NOT NULL," +
         "Paid REAL NOT NULL," +
         "Date DATETIME NOT NULL" +
         ");";

        private bool IsDbRecentlyCreated = false;

        public void InitializeDatabase()
        {
            if (!File.Exists(Path.GetFullPath(DBName)))
            {
                SQLiteConnection.CreateFile(DBName);
                IsDbRecentlyCreated = true;
            }

            using (var connection = GetInstance())
            {
                if (IsDbRecentlyCreated)
                {
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private static SQLiteConnection GetInstance()
        {
            SQLiteConnection db = new SQLiteConnection(string.Format("Data Source={0};Version=3;", DBName));

            db.Open();

            return db;
        }

        public void addPayment(string customer, int account, double debt, double paid )
        {
            using (var connection = GetInstance())
            {
                var query = "INSERT INTO Payments (Customer, Account, Debt, Paid, Date) VALUES (?, ?, ?, ? , ?)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.Add(new SQLiteParameter("Customer", customer));
                    command.Parameters.Add(new SQLiteParameter("Account", account));
                    command.Parameters.Add(new SQLiteParameter("Debt", debt));
                    command.Parameters.Add(new SQLiteParameter("Paid", paid));
                    command.Parameters.Add(new SQLiteParameter("Date", DateTime.Now));
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
