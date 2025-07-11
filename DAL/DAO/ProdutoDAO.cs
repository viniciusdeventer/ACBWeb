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
                             " Imagem, " +
                             " Valor_Produto, " +
                             " Status, " +
                             " Data_Cadastro FROM Produtos";

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
                            Imagem = reader.GetString("Imagem"),
                            ValorProduto = reader.GetDecimal("Valor_Produto"),
                            Status = reader.GetInt32("Status"),
                            DataCadastro = reader.GetDateTime("Data_Cadastro")
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

                string sql = @"SELECT ID_Produto, Nome, Descricao, Imagem
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
        public void Salvar(Produto produto)
        {
            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return;

                string sql;

                if (produto.IdProduto > 0)
                {
                    sql = @"UPDATE Produtos 
                    SET Nome = @Nome,
                        Descricao = @Descricao,
                        Imagem = @Imagem,
                        Valor_Produto = @ValorProduto,
                        Status = @Status
                    WHERE ID_Produto = @IdProduto";
                }
                else
                {
                    sql = @"INSERT INTO Produtos 
                    (Nome, Descricao, Imagem, Valor_Produto, Status)
                    VALUES 
                    (@Nome, @Descricao, @Imagem, @ValorProduto, @Status)";
                }

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nome", produto.Nome);
                    cmd.Parameters.AddWithValue("@Descricao", produto.Descricao);
                    cmd.Parameters.AddWithValue("@Imagem", produto.Imagem);
                    cmd.Parameters.AddWithValue("@ValorProduto", produto.ValorProduto);
                    cmd.Parameters.AddWithValue("@Status", produto.Status);

                    if (produto.IdProduto > 0)
                    {
                        cmd.Parameters.AddWithValue("@IdProduto", produto.IdProduto);
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Excluir(int id)
        {
            using (var conn = Conexao.GetConnection())
            {
                string sql = "UPDATE Produtos SET Status = 0 WHERE ID_Produto = @IdProduto";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdProduto", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
