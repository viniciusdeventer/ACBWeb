using ACBWeb.DAL;
using ACBWeb.Models;
using MySql.Data.MySqlClient;
using System.Text;
using System.Security.Cryptography;

namespace ACBWeb.DAL.DAO
{
    public class LoginDAO
    {
        public static Login Login(string usuario, string senha)
        {
            using (var conn = Conexao.GetConnection())
            {
                string sql = " SELECT ID_Usuario, " +
                             " Nome, " +
                             " Usuario, " +
                             " Senha, " +
                             " Status " +
                             " FROM Usuarios " +
                             " WHERE Usuario = @Usuario " +
                             " AND Status = 1 ";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Usuario", usuario);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string senhaHash = reader["Senha"].ToString();
                            if (senhaHash == GerarHash(senha))
                            {
                                return new Login
                                {
                                    IdUsuario = Convert.ToInt32(reader["ID_Usuario"]),
                                    Nome = reader["Nome"].ToString(),
                                    Usuario = reader["Usuario"].ToString(),
                                    Status = Convert.ToInt32(reader["Status"])
                                };
                            }
                        }
                    }
                }
            }
            return null;
        }
        private static string GerarHash(string senha)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(senha));
                return Convert.ToHexString(bytes);
            }
        }
    }
}
