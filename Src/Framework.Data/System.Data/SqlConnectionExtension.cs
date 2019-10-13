using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace GoodToCode.Extensions
{
    /// <summary>
    /// Extends System.Type
    /// </summary>
    public static class SqlConnectionExtension
    {
        /// <summary>
        /// Constructs a connection string from multiple string elements
        /// </summary>
        public static void ConnectionString(this SqlConnection connection, string serverName, string databaseName, int timeoutInSeconds = 3)
        {
            StringBuilder connectionString = new StringBuilder();
            connectionString.Append("Data Source=").Append(serverName).Append(";Initial Catalog=");
            connectionString.Append(databaseName).Append(";Persist Security Info=True;Trusted_connection=Yes;").Append(";Connect Timeout=").Append(timeoutInSeconds);
            connection.ConnectionString = connectionString.ToString();
        }

        /// <summary>
        /// Tests a connection to see if can open
        /// </summary>
        /// <param name="connection"></param>
        /// <returns>True if this connection can be opened</returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static bool CanOpen(this SqlConnection connection)
        {
            var returnValue = Defaults.Boolean;

            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    returnValue = true;
                    connection.Close();
                }
            }
            catch
            {
                returnValue = false;
            }
            finally
            {
                connection.Dispose();
            }

            return returnValue;
        }

        /// <summary>
        /// Tests a connection to see if can open
        /// </summary>
        /// <param name="connection">Sql connection to tests</param>
        /// <returns>True if this connection can be opened</returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static async Task<bool> CanOpenAsync(this SqlConnection connection)
        {
            var returnValue = Defaults.Boolean;

            try
            {
                await connection.OpenAsync();
                if (connection.State == ConnectionState.Open)
                {
                    returnValue = true;
                    connection.Close();
                }
            }
            catch
            {
                returnValue = false;
            }
            finally
            {
                connection.Dispose();
            }

            return returnValue;
        }
    }
}
