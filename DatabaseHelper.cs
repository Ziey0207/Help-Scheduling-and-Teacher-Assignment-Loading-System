using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        // New async versions for real-time validation
        public static async Task<int> ExecuteNonQueryAsync(string query, MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = GetConnection())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public static async Task<object> ExecuteScalarAsync(string query, MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = GetConnection())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    return await cmd.ExecuteScalarAsync();
                }
            }
        }

        public static async Task<MySqlDataReader> ExecuteReaderAsync(string query, MySqlParameter[] parameters)
        {
            MySqlConnection conn = GetConnection();
            await conn.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
        }

        // Async version of TestConnection
        public static async Task<bool> TestConnectionAsync()
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    await conn.OpenAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Connecting to database: " + ex.Message);
                return false;
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

        // Add these new methods to your DatabaseHelper class
        public static int RecordLogin(int userId, string sessionToken, bool rememberMe)
        {
            Debug.WriteLine($"Recording login - UserID: {userId}, Token: {sessionToken}, RememberMe: {rememberMe}");

            string query = @"INSERT INTO user_sessions
                   (user_id, login_time, session_token, remember_me)
                   VALUES (@userId, NOW(), @token, @rememberMe)";

            int result = ExecuteNonQuery(query, new MySqlParameter[] {
        new MySqlParameter("@userId", userId),
        new MySqlParameter("@token", sessionToken),
        new MySqlParameter("@rememberMe", rememberMe)
    });

            Debug.WriteLine($"Login recorded - Rows affected: {result}");
            return result;
        }

        public static int RecordLogout(string sessionToken, bool explicitLogout = true)
        {
            Debug.WriteLine($"Recording logout for token: {sessionToken}, Explicit: {explicitLogout}");

            string query = @"UPDATE user_sessions
                    SET logout_time = NOW(),
                        logout_type = @logoutType
                    WHERE session_token = @token";

            return ExecuteNonQuery(query, new MySqlParameter[] {
                new MySqlParameter("@token", sessionToken),
                new MySqlParameter("@logoutType", explicitLogout ? "user" : "system")
            });
        }

        public static string CreateNewSession(int userId, bool rememberMe)
        {
            // Generate new token and record login
            string newToken = Guid.NewGuid().ToString();

            string query = @"INSERT INTO user_sessions
                   (user_id, login_time, session_token, remember_me)
                   VALUES (@userId, NOW(), @token, @rememberMe)";

            ExecuteNonQuery(query, new MySqlParameter[] {
        new MySqlParameter("@userId", userId),
        new MySqlParameter("@token", newToken),
        new MySqlParameter("@rememberMe", rememberMe)
    });

            return newToken;
        }

        public static bool IsValidSession(string sessionToken)
        {
            string query = @"SELECT COUNT(*) FROM user_sessions
                   WHERE session_token = @token";
            return Convert.ToInt32(ExecuteScalar(query,
                new MySqlParameter[] { new MySqlParameter("@token", sessionToken) })) > 0;
        }

        public static bool CheckTokenExists(string sessionToken)
        {
            string query = "SELECT COUNT(*) FROM user_sessions WHERE session_token = @token";
            int count = Convert.ToInt32(ExecuteScalar(query,
                new MySqlParameter[] { new MySqlParameter("@token", sessionToken) }));
            return count > 0;
        }

        public static bool ValidateSession(string sessionToken)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM user_sessions
                        WHERE session_token = @token";

                Debug.WriteLine($"Validating session token: {sessionToken}");
                int activeSessions = Convert.ToInt32(ExecuteScalar(query,
                    new MySqlParameter[] { new MySqlParameter("@token", sessionToken) }));

                Debug.WriteLine($"Session validation found {activeSessions} active sessions");
                return activeSessions > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Session validation error: {ex.Message}");
                return false;
            }
        }

        public static int GetUserId(string username)
        {
            string query = "SELECT id FROM admins WHERE username = @username";
            return Convert.ToInt32(ExecuteScalar(query,
                new MySqlParameter[] { new MySqlParameter("@username", username) }));
        }

        public static string GetAdminNameById(int adminId)
        {
            string query = "SELECT username FROM admins WHERE id = @adminId";
            return ExecuteScalar(query,
                new MySqlParameter[] { new MySqlParameter("@adminId", adminId) })?.ToString();
        }

        public static int? GetAdminIdBySession(string sessionToken)
        {
            string query = "SELECT user_id FROM user_sessions WHERE session_token = @token";
            object result = ExecuteScalar(query,
                new MySqlParameter[] { new MySqlParameter("@token", sessionToken) });
            return result != null ? Convert.ToInt32(result) : (int?)null;
        }
    }
}