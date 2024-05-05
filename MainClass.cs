using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CashierManagement.Models;

namespace CashierManagement
{
    public class MainClass
    {
        private const string ConnectionString = "server=localhost,1433;user=sa;password=123abc;database=ApotekXyz";

        public static string LoggedInUsername { get; set; }

        public static SqlConnection Conn = new SqlConnection(ConnectionString);

        public static bool IsValidUser(string username, string password)
        {
            var isValid = false;
            const string query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
            var command = new SqlCommand(query, Conn);

            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);

            var dt = new DataTable();
            var da = new SqlDataAdapter(command);
            da.Fill(dt);
            
            if (dt.Rows.Count > 0)
            {
                isValid = true;
                LoggedInUsername = username;
            }
            
            return isValid;
        }

        public static int FindUserIdByUsername(string username)
        {
            Conn.Open();
            var query = "SELECT Id FROM Users WHERE Username = @Username";
            var command = new SqlCommand(query, Conn);
            
            command.Parameters.AddWithValue("@Username", username);
            var sqlDataReader = command.ExecuteReader();
            if (sqlDataReader.GetInt32(0) > 0)
            {
                return sqlDataReader.GetInt32(0);
            }
            else
            {
                throw new Exception("user not found");
            }
        }

        public static Drug GetDrug(string code)
        {
            const string query = "SELECT * FROM Drugs WHERE Code = @Code";
            Conn.Open();
            var command = new SqlCommand(query, Conn);

            command.Parameters.AddWithValue("@Code", code);
            var sqlDataReader = command.ExecuteReader();

            var drug = new Drug();
            if (sqlDataReader.Read())
            {
                drug.Code = sqlDataReader.GetString(0);
                drug.Name = sqlDataReader.GetString(1);
                drug.Price = sqlDataReader.GetDecimal(2);
                drug.Quantity = sqlDataReader.GetInt32(3);
            }
            else
            {
                throw new Exception("Failed to get data");
            }

            Conn.Close();
            return drug;
        }

        public static List<string> GetDrugCodes()
        {
            var list = new List<string>();
            const string query = "SELECT Code FROM Drugs";
            Conn.Open();
            var command = new SqlCommand(query, Conn);
            var sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                list.Add(sqlDataReader.GetString(0));
            }
            Conn.Close();
            return list;
        }

        public static void SaveTransactionDetail(TransactionDetail detail)
        {
            try
            {
                Conn.Open();
                var sql =
                    "INSERT INTO TransactionDetail (TransactionId, DrugId, Quantity, SubTotal) VALUES (@TransactionId, @DrugId, @Quantity, @SubTotal)";

                var command = new SqlCommand(sql, Conn);
                command.Parameters.AddWithValue("@TransactionId", detail.TransactionId);
                command.Parameters.AddWithValue("@DrugId", detail.DrugId);
                command.Parameters.AddWithValue("@Quantity", detail.Quantity);
                command.Parameters.AddWithValue("@SubTotal", detail.SubTotal);

                var execute = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                Conn.Close();
            }
        }

        public static void SaveTransaction(TransactionModel transaction)
        {
            try
            {
                Conn.Open();
                var sql =
                    "INSERT INTO Trans (Date, Total, UserId) VALUES (@Date, @Total, @UserId)";

                var command = new SqlCommand(sql, Conn);
                command.Parameters.AddWithValue("@Date", transaction.Date);
                command.Parameters.AddWithValue("@Total", transaction.Total);
                command.Parameters.AddWithValue("@UserId", transaction.UserId);

                var execute = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                Conn.Close();
            }
        }
    }
}