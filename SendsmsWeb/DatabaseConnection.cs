using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SendsmsWeb
{
    public class DatabaseConnection
    {
        private readonly SqlConnection Connection;

        /// <summary>
        /// Creates a new instance of DatabaseConnection and connects to a database using the specified connection string.
        /// </summary>
        /// <param name="ConnectionString">The connection string used to connect to the MSSQL database.</param>
        public DatabaseConnection(string ConnectionString)
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
        }

        /// <summary>
        /// Excutes an SQL query in the connected database.
        /// </summary>
        /// <param name="Query">The SQL to excecute.</param>
        /// <returns>The amount of rows affected.</returns>
        public async Task<int> SqlQueryAsync(string Query)
        {
            // Create a new SqlCommand
            SqlCommand Cmd = new SqlCommand(Query, Connection);

            // Exceute and return the command
            return await Cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Excutes an SQL query in the connected database.
        /// </summary>
        /// <param name="Query">The SQL to excecute.</param>
        /// <returns>A nested list of the values read from the database.</returns>
        public async Task<List<List<string>>> GetRowsAsync(string Query)
        {
            // Create a new SqlCommand
            SqlCommand Cmd = new SqlCommand(Query, Connection);

            // Create the DataReader
            SqlDataReader DataReader = await Cmd.ExecuteReaderAsync();

            // Create the list to return
            List<List<string>> Rows = new List<List<string>>();

            // Read rows
            if (DataReader.HasRows)
            {
                while (await DataReader.ReadAsync())
                {
                    // Create a list for the current row
                    List<string> Row = new List<string>();

                    // Iterate through each column and add it to the list
                    for (int i = 0; i < DataReader.FieldCount; i++)
                    {
                        // Encapsulate in a try/catch in case of null values
                        try
                        {
                            Row.Add(DataReader.GetString(i));
                        }
                        catch (SqlNullValueException)
                        {
                            Row.Add(null);  
                        }
                    }

                    // Add the row to the return list
                    Rows.Add(Row);
                }
            }

            // Close and dispose
            await DataReader.CloseAsync();
            await Cmd.DisposeAsync();

            // Return the read rows
            return Rows;
        }

        /// <summary>
        /// Escapes all single quotes to avoid SQL injection.
        /// </summary>
        /// <param name="Input">The input string</param>
        /// <returns>The input string with single quotes escaped.</returns>
        public static string EscapeSql(string Input) => Input.Replace("'", "''");

    }
}
