using ACBWeb.Models;
using MySql.Data.MySqlClient;

namespace ACBWeb.DAL.DAO
{
    public class ContaProdutosDAO
    {
        public List<ContaProdutos> GetContaProdutos(int IdConta)
        {
            var lista = new List<ContaProdutos>();

            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return lista;

                string sql = @"SELECT CP.ID_Item,
	                                  CASE Tipo_Item 
			                               WHEN 0 THEN P.Nome
                                           WHEN 1 THEN 'Saldo Anterior'
	                                  END AS Nome_Item,
                                      CP.ID_Conta,    
                                      CP.ID_Produto,
                                      CP.Quantidade,
                                      CP.Valor_Unitario,
                                      CP.Tipo_Item,
                                      CP.Data_Cadastro,
                                      CP.Quantidade * CP.Valor_Unitario AS Subtotal,
                                      SUM(CP.Quantidade * CP.Valor_Unitario) OVER() AS Total
                              FROM Contas_Produtos CP 
                              INNER JOIN Contas C ON CP.ID_Conta = C.ID_Conta 
                              INNER JOIN Clientes Cli ON C.ID_Cliente = Cli.ID_Cliente
                              LEFT JOIN Produtos P ON CP.ID_Produto = P.ID_Produto 
                              WHERE CP.ID_Conta = @IdConta 
                              ORDER BY Data_Cadastro";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdConta", IdConta);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ContaProdutos
                            {
                                IdItem = reader.GetNullableInt("ID_Item"),
                                NomeItem = reader.GetNullableString("Nome_Item"),
                                IdConta = reader.GetNullableInt("ID_Conta"),
                                IdProduto = reader.GetNullableInt("ID_Produto"),
                                Quantidade = reader.GetNullableInt("Quantidade"),
                                ValorUnitario = reader.GetNullableDecimal("Valor_Unitario"),
                                TipoItem = reader.GetNullableInt("Tipo_Item"),
                                DataCadastro = reader.GetNullableDateTime("Data_Cadastro"),
                                Subtotal = reader.GetNullableDecimal("Subtotal"),
                                Total = reader.GetNullableDecimal("Total")
                            });
                        }
                    }
                }
            }

            return lista;
        }
        public ContaProdutos BuscarPorId(int idItem)
        {
            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return null;

                string sql = @"SELECT CP.ID_Item,
	                                  CASE Tipo_Item 
			                               WHEN 0 THEN P.Nome
                                           WHEN 1 THEN 'Saldo Anterior'
	                                  END AS Nome_Item,
                                      CP.ID_Conta,    
                                      CP.ID_Produto,
                                      CP.Quantidade,
                                      CP.Valor_Unitario,
                                      CP.Tipo_Item,
                                      CP.Data_Cadastro,
                                      CP.Quantidade * CP.Valor_Unitario AS Subtotal,
                                      SUM(CP.Quantidade * CP.Valor_Unitario) OVER() AS Total
                              FROM Contas_Produtos CP 
                              INNER JOIN Contas C ON CP.ID_Conta = C.ID_Conta 
                              INNER JOIN Clientes Cli ON C.ID_Cliente = Cli.ID_Cliente
                              LEFT JOIN Produtos P ON CP.ID_Produto = P.ID_Produto
                              WHERE ID_Item = @IdItem";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdItem", idItem);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ContaProdutos
                            {
                                IdItem = reader.GetNullableInt("ID_Item"),
                                NomeItem = reader.GetNullableString("Nome_Item"),
                                IdConta = reader.GetNullableInt("ID_Conta"),
                                IdProduto = reader.GetNullableInt("ID_Produto"),
                                Quantidade = reader.GetNullableInt("Quantidade"),
                                ValorUnitario = reader.GetNullableDecimal("Valor_Unitario"),
                                TipoItem = reader.GetNullableInt("Tipo_Item"),
                                DataCadastro = reader.GetNullableDateTime("Data_Cadastro"),
                                Subtotal = reader.GetNullableDecimal("Subtotal"),
                                Total = reader.GetNullableDecimal("Total")
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void Salvar(ContaProdutos contaProdutos)
        {
            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return;

                string sql;

                if (contaProdutos.IdItem > 0)
                {
                    sql = @"UPDATE Contas_Produtos 
                            SET ID_Produto = @IdProduto,
                                Quantidade = @Quantidade,
                                Valor_Unitario = @ValorUnitario
                            WHERE ID_Item = @IdItem";
                }
                else
                {
                    sql = @"INSERT INTO Contas_Produtos 
                            (ID_Conta, ID_Produto, Quantidade, Valor_Unitario) 
                            VALUES 
                            (@IdConta, @IdProduto, @Quantidade, @ValorUnitario)";
                }

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdConta", contaProdutos.IdConta);
                    cmd.Parameters.AddWithValue("@IdProduto", contaProdutos.IdProduto);
                    cmd.Parameters.AddWithValue("@Quantidade", contaProdutos.Quantidade);
                    cmd.Parameters.AddWithValue("@ValorUnitario", contaProdutos.ValorUnitario);

                    if (contaProdutos.IdItem > 0)
                        cmd.Parameters.AddWithValue("@IdItem", contaProdutos.IdItem);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
