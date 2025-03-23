using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Scheduling_and_Teacher_Loading_Assignment_System
{
    internal class DatabaseHelper
    {
        private static string connectionString = "Server=localhost;Database=school_management;Uid=root;Pwd=;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        // Method to execute a non-query (INSERT, UPDATE, DELETE)
        public static int ExecuteNonQuery(string query, MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        // Method to execute a scalar query (SELECT single value)
        public static object ExecuteScalar(string query, MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    return cmd.ExecuteScalar();
                }
            }
        }

        // Method to execute a reader query (SELECT multiple rows)
        public static MySqlDataReader ExecuteReader(string query, MySqlParameter[] parameters)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
        }

        public bool TestConnection()
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Connecting to database: " + ex.Message);
                return false;
            }
        }
    }
}