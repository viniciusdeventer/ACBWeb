using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ACBWeb.Models;
using ACBWeb.DAL;        

namespace ACBWeb.DAL.DAO
{
    public class ProdutoDAO
    {
        public List<Produto> GetProdutos()
        {
            var lista = new List<Produto>();

            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return lista;

                string sql = " SELECT ID_Produto, " +
                             " Nome, " +
                             " Descricao, " +
                             " Valor_Produto, " +
                             " Imagem, " +
                             " Status FROM Produtos";

                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Produto
                        {
                            IdProduto = reader.GetInt32("ID_Produto"),
                            Nome = reader.GetString("Nome"),
                            Descricao = reader.GetString("Descricao"),
                            ValorProduto = reader.GetDecimal("Valor_Produto"),
                            Imagem = reader.GetString("Imagem"),
                            Status = reader.GetInt32("Status")
                        });
                    }
                }
            }

            return lista;
        }

        public List<Produto> BuscarProduto(string termo)
        {
            var lista = new List<Produto>();

            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return lista;

                string sql = @"SELECT ID_Produto, Nome, Descricao, Ativo 
                               FROM Produtos 
                               WHERE Nome LIKE @Termo";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Termo", "%" + termo + "%");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Produto
                            {
                                IdProduto = reader.GetInt32("ID_Produto"),
                                Nome = reader.GetString("Nome"),
                                Descricao = reader.GetString("Descricao"),
                                Status = reader.GetInt32("Status")
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public void Excluir(int id)
        {
            using (var conn = Conexao.GetConnection())
            {
                string sql = "UPDATE Produtos SET Status = 0 WHERE Id_Produto = @IdProduto";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdProduto", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
