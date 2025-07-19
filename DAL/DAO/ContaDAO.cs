using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ACBWeb.Models;
using ACBWeb.DAL;

namespace ACBWeb.DAL.DAO
{
    public class ContaDAO
    {
        public List<Conta> GetContas(int IdCliente)
        {
            var lista = new List<Conta>();

            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return lista;

                string sql = @"SELECT C.*, 
                               SUM(CP.Quantidade * CP.Valor_Unitario) AS Valor_Total                               
                               FROM Contas C
                               LEFT JOIN Contas_Produtos CP ON C.ID_Conta = CP.ID_Conta 
                               WHERE ID_Cliente = @IdCliente 
                               AND C.Situacao IN (0, 2)
                               GROUP BY C.ID_Conta, C.ID_Cliente
                               ORDER BY C.Data_Cadastro";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdCliente", IdCliente);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Conta
                            {
                                IdConta = reader.GetNullableInt("ID_Conta"),
                                IdCliente = reader.GetNullableInt("ID_Cliente"),
                                Situacao = reader.GetNullableInt("Situacao"),
                                DataPagamento = reader.GetNullableDateTime("Data_Pagamento"),
                                ValorPagamento = reader.GetNullableDecimal("Valor_Pagamento"),
                                ObservacaoPagamento = reader.GetNullableString("Observacao_Pagamento"),
                                ValorTotal = reader.GetNullableDecimal("Valor_Total"),
                                DataCadastro = reader.GetNullableDateTime("Data_Cadastro")
                            });
                        }
                    }
                }
            }

            return lista;
        }
        public Conta BuscarPorId(int idConta)
        {
            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return null;

                string sql = @"SELECT * FROM Contas C 
                             INNER JOIN Clientes Cli ON C.ID_Cliente = Cli.ID_Cliente 
                             WHERE ID_Conta = @IdConta";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdConta", idConta);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Conta
                            {
                                IdConta = reader.GetNullableInt("ID_Conta"),
                                IdCliente = reader.GetNullableInt("ID_Cliente"),
                                NomeCliente = reader.GetNullableString("Nome"),
                                Situacao = reader.GetNullableInt("Situacao"),
                                DataPagamento = reader.GetNullableDateTime("Data_Pagamento"),
                                ValorPagamento = reader.GetNullableDecimal("Valor_Pagamento"),
                                ObservacaoPagamento = reader.GetNullableString("Observacao_Pagamento"),
                                DataCadastro = reader.GetNullableDateTime("Data_Cadastro")
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void Salvar(Conta conta)
        {
            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return;

                string sql;

                if (conta.IdConta > 0)
                {
                    sql = @"UPDATE Contas 
                            SET Situacao = @Situacao,
                                Data_Pagamento = @DataPagamento,
                                Valor_Pagamento = @ValorPagamento,
                                Observacao_Pagamento = @ObservacaoPagamento
                            WHERE ID_Conta = @IdConta";
                }
                else
                {
                    sql = @"INSERT INTO Contas 
                            (ID_Cliente, Situacao, Valor_Pagamento, Data_Pagamento, Observacao_Pagamento) 
                            VALUES 
                            (@IdCliente, @Situacao, @ValorPagamento, @DataPagamento, @ObservacaoPagamento)";
                }

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdCliente", conta.IdCliente);
                    cmd.Parameters.AddWithValue("@Situacao", conta.Situacao);
                    cmd.Parameters.AddWithValue("@ValorPagamento", conta.ValorPagamento);
                    cmd.Parameters.AddWithValue("@DataPagamento", conta.DataPagamento);
                    cmd.Parameters.AddWithValue("@ObservacaoPagamento", conta.ObservacaoPagamento);

                    if (conta.IdConta > 0)
                        cmd.Parameters.AddWithValue("@IdConta", conta.IdConta);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void PagarConta(Conta conta)
        {
            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return;

                using (var cmd = new MySqlCommand("PagarConta", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_IdConta", conta.IdConta);
                    cmd.Parameters.AddWithValue("@p_IdCliente", conta.IdCliente);
                    cmd.Parameters.AddWithValue("@p_ValorPagamento", conta.ValorPagamento);
                    cmd.Parameters.AddWithValue("@p_DataPagamento", conta.DataPagamento);
                    cmd.Parameters.AddWithValue("@p_ObservacaoPagamento", conta.ObservacaoPagamento);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Conta Excluir(int idConta)
        {
            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return null;

                string sql = @"UPDATE Contas SET Situacao = 2 WHERE ID_Conta = @ID_Conta";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ID_Conta", idConta);
                    cmd.ExecuteNonQuery();
                }
            }

            return null;
        }
    }
}
