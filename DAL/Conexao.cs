using MySql.Data.MySqlClient;

namespace ACBWeb.DAL
{
    public class Conexao
    {
        private static string connectionString = "server=localhost;port=3306;database=ACBarros;user=root;password=;SslMode=none;";

        public static MySqlConnection GetConnection()
        {
            try
            {
                MySqlConnection conexao = new MySqlConnection(connectionString);
                conexao.Open();
                return conexao;
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Erro na conexão com o banco: ", e.Message);
                return null;
            }
        }
    }
}
