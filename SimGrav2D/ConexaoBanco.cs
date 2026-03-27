using MySql.Data.MySqlClient;
using System;

namespace SimGrav2D
{
    public class ConexaoBanco
    {
        private static ConexaoBanco? _instancia;
        private readonly string _connectionString;

        private ConexaoBanco()
        {
            _connectionString = "Server=localhost;Database=mydb;Uid=root;Pwd=1234;";

        }

        public static ConexaoBanco Instancia => _instancia ??= new ConexaoBanco();

        public MySqlConnection CriarConexao()
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open(); // 
            return conn;
        }
    }
}
