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

                string sql = @"SELECT * FROM Contas 
                               WHERE ID_Cliente = @IdCliente 
                               ORDER BY Data_Abertura DESC";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdCliente", IdCliente);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Conta
                            {
                                IdConta = reader.GetInt32("ID_Conta"),
                                IdCliente = reader.GetInt32("ID_Cliente"),
                                DataAbertura = reader.GetDateTime("Data_Abertura"),
                                Situacao = reader.GetInt32("Situacao"),
                                DataPagamento = reader.GetNullableDateTime("Data_Pagamento"),
                                ValorPagamento = reader.GetDecimal("Valor_Pagamento"),
                                ObservacaoPagamento = reader.GetNullableString("Observacao_Pagamento"),
                                DataCadastro = reader.GetDateTime("Data_Cadastro")
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public Conta BuscarContaAberta(int idCliente)
        {
            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return null;

                string sql = @"SELECT * FROM Contas 
                               WHERE ID_Cliente = @IdCliente 
                               AND Situacao = 0 ";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdCliente", idCliente);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Conta
                            {
                                IdConta = reader.GetInt32("ID_Conta"),
                                IdCliente = reader.GetInt32("ID_Cliente"),
                                DataAbertura = reader.GetDateTime("Data_Abertura"),
                                Situacao = reader.GetInt32("Situacao"),
                                DataPagamento = reader.GetNullableDateTime("Data_Pagamento"),
                                ValorPagamento = reader.GetDecimal("Valor_Pagamento"),
                                ObservacaoPagamento = reader.GetNullableString("Observacao_Pagamento"),
                                DataCadastro = reader.GetDateTime("Data_Cadastro")
                            };
                        }
                    }
                }
            }

            return null;
        }

        public Conta BuscarPorId(int idConta)
        {
            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return null;

                string sql = "SELECT * FROM Contas WHERE ID_Conta = @IdConta";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdConta", idConta);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Conta
                            {
                                IdConta = reader.GetInt32("ID_Conta"),
                                IdCliente = reader.GetInt32("ID_Cliente"),
                                DataAbertura = reader.GetDateTime("Data_Abertura"),
                                Situacao = reader.GetInt32("Situacao"),
                                DataPagamento = reader.GetNullableDateTime("Data_Pagamento"),
                                ValorPagamento = reader.GetDecimal("Valor_Pagamento"),
                                ObservacaoPagamento = reader.GetNullableString("Observacao_Pagamento"),
                                DataCadastro = reader.GetDateTime("Data_Cadastro")
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
                            (ID_Cliente, Data_Abertura, Situacao, Valor_Pagamento, Data_Pagamento, Observacao_Pagamento) 
                            VALUES 
                            (@IdCliente, @DataAbertura, @Situacao, @ValorPagamento, @DataPagamento, @ObservacaoPagamento)";
                }

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdCliente", conta.IdCliente);
                    cmd.Parameters.AddWithValue("@DataAbertura", conta.DataAbertura);
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
    }
}
