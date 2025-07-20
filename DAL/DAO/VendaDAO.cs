using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ACBWeb.Models;
using ACBWeb.DAL;

namespace ACBWeb.DAL.DAO
{
    public class VendaDAO
    {
        public List<Venda> GetVendas(DateTime data)
        {
            var lista = new List<Venda>();

            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return lista;

                string sql = @"SELECT V.*,
                              CASE V.Tipo_Venda 
                                  WHEN 0 THEN P.Nome 
                                  WHEN 1 THEN Cli.Nome
                              END AS Nome_Item,
                              V.Quantidade * V.Valor_Venda AS Subtotal,
                              SUM(V.Quantidade * V.Valor_Venda) OVER() AS Total
                      FROM Vendas V
                      LEFT JOIN Contas C ON V.ID_Item = C.ID_Conta 
                      LEFT JOIN Clientes Cli ON C.ID_Cliente = Cli.ID_Cliente
                      LEFT JOIN Produtos P ON V.ID_Item = P.ID_Produto
                      WHERE DATE(V.Data_Venda) = DATE(@Data)
                      ORDER BY V.ID_Venda";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Data", data);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Venda
                            {
                                IdVenda = reader.GetNullableInt("ID_Venda"),
                                IdItem = reader.GetNullableInt("ID_Item"),
                                NomeItem = reader.GetNullableString("Nome_Item"),
                                DataVenda = reader.GetNullableDateTime("Data_Venda"),
                                Quantidade = reader.GetNullableInt("Quantidade"),
                                ValorVenda = reader.GetNullableDecimal("Valor_Venda"),
                                TipoVenda = reader.GetNullableInt("Tipo_Venda"),
                                Subtotal = reader.GetNullableDecimal("Subtotal"),
                                Total = reader.GetNullableDecimal("Total"),
                                DataCadastro = reader.GetNullableDateTime("Data_Cadastro")
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public Venda BuscarPorId(int idVenda)
        {
            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return null;

                string sql = @"SELECT V.*,
                                      CASE V.Tipo_Venda 
                                          WHEN 0 THEN P.Nome 
                                          WHEN 1 THEN Cli.Nome
                                      END AS Nome_Item,
                                      V.Quantidade * V.Valor_Venda AS Subtotal,
                                      SUM(V.Quantidade * V.Valor_Venda) OVER() AS Total
                              FROM Vendas V
                              LEFT JOIN Contas C ON V.ID_Item = C.ID_Conta 
                              LEFT JOIN Clientes Cli ON C.ID_Cliente = Cli.ID_Cliente
                              LEFT JOIN Produtos P ON V.ID_Item = P.ID_Produto
                              WHERE ID_Venda = @idVenda";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdVenda", idVenda);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Venda
                            {
                                IdVenda = reader.GetNullableInt("ID_Venda"),
                                IdItem = reader.GetNullableInt("ID_Item"),
                                NomeItem = reader.GetNullableString("Nome_Item"),
                                DataVenda = reader.GetNullableDateTime("Data_Venda"),
                                Quantidade = reader.GetNullableInt("Quantidade"),
                                ValorVenda = reader.GetNullableDecimal("Valor_Venda"),
                                TipoVenda = reader.GetNullableInt("Tipo_Venda"),
                                Subtotal = reader.GetNullableDecimal("Subtotal"),
                                Total = reader.GetNullableDecimal("Total"),
                                DataCadastro = reader.GetNullableDateTime("Data_Cadastro")
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void Salvar(Venda venda)
        {
            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return;

                string sql;

                if (venda.IdVenda > 0)
                {
                    sql = @"UPDATE Vendas 
                    SET ID_Item = @IdItem,
                        Data_Venda = @DataVenda,
                        Quantidade = @Quantidade, 
                        Valor_Venda = @ValorVenda
                    WHERE ID_Venda = @IdVenda";
                }
                else
                {
                    sql = @"INSERT INTO Vendas 
                    (ID_Item, Data_Venda, Quantidade, Valor_Venda)
                    VALUES 
                    (@IdItem, @DataVenda, @Quantidade, @ValorVenda)";
                }

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdItem", venda.IdItem);
                    cmd.Parameters.AddWithValue("@DataVenda", venda.DataVenda);
                    cmd.Parameters.AddWithValue("@Quantidade", venda.Quantidade ?? 1);
                    cmd.Parameters.AddWithValue("@ValorVenda", venda.ValorVenda);

                    if (venda.IdVenda > 0)
                    {
                        cmd.Parameters.AddWithValue("@IdVenda", venda.IdVenda);
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Venda Excluir(int idVenda)
        {
            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return null;

                string sql = @"DELETE FROM Vendas WHERE ID_Venda = @IdVenda";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdVenda", idVenda);
                    cmd.ExecuteNonQuery();
                }
            }
            return null;
        }
    }
}