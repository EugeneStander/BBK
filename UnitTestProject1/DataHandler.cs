using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MainSpace
{
    class DataHandler
    {
        private string connectionString;
        private SqlCommand command;
        private SqlDataAdapter dataAdapter;
        private DataTable dataTable = new DataTable();

        public DataHandler(string connectionName)
        {
            connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        public void ExecuteNonQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }

        public DataTable ExecuteQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                dataTable.Columns.Clear();
                dataTable.Rows.Clear();
                dataAdapter = new SqlDataAdapter(query, connection);
                dataAdapter.Fill(dataTable);
            }
            return dataTable;
        }
    }
}
