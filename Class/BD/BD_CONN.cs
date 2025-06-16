using MySql.Data.MySqlClient;
using System;

namespace Infor_Soft_WPF.Class.BD
{
    internal class BD_CONN : IDisposable
    {
        private MySqlConnection _connection;

        public BD_CONN()
        {
            _connection = new MySqlConnection("server=localhost;user=root;password=;database=inforsoft;port=3306");
        }

        public MySqlConnection GetConnection()
        {
            return _connection;
        }

        public void OpenConnection()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                _connection.Open();
        }

        public void CloseConnection()
        {
            if (_connection.State != System.Data.ConnectionState.Closed)
                _connection.Close();
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
