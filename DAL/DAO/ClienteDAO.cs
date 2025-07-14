using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ACBWeb.Models;
using ACBWeb.DAL;

namespace ACBWeb.DAL.DAO
{
    public class ClienteDAO
    {
        public List<Cliente> GetClientes()
        {
            var lista = new List<Cliente>();

            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return lista;

                string sql = @"SELECT ID_Cliente, 
                                      Nome, 
                                      CONCAT(
                                        '(', 
                                        SUBSTRING(Telefone, 1, 2), 
                                        ') ', 
                                        SUBSTRING(Telefone, 3, 5), 
                                        '-', 
                                        SUBSTRING(Telefone, 8, 4)
                                      ) AS Telefone,
                                      Observacoes, 
                                      Status, 
                                      Data_Cadastro
                               FROM Clientes 
                               WHERE Status IN (1, 2)
                               ORDER BY Nome ";

                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Cliente
                        {
                            IdCliente = reader.GetNullableInt("ID_Cliente"),
                            Nome = reader.GetNullableString("Nome"),
                            Telefone = reader.GetNullableString("Telefone"),
                            Observacoes = reader.GetNullableString("Observacoes"),
                            Status = reader.GetNullableInt("Status"),
                            DataCadastro = reader.GetNullableDateTime("Data_Cadastro")
                        });
                    }
                }
            }

            return lista;
        }

        public List<Cliente> BuscarCliente(string termo)
        {
            var lista = new List<Cliente>();

            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return lista;

                string sql = @"SELECT ID_Cliente, 
                                      Nome, 
                                      CONCAT(
                                        '(', 
                                        SUBSTRING(Telefone, 1, 2), 
                                        ') ', 
                                        SUBSTRING(Telefone, 3, 5), 
                                        '-', 
                                        SUBSTRING(Telefone, 8, 4)
                                      ) AS Telefone,
                                      Observacoes, 
                                      Status, 
                                      Data_Cadastro
                               FROM Clientes 
                               WHERE Nome LIKE @Termo
                               AND Status IN (1, 2)";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Termo", termo + "%");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Cliente
                            {
                                IdCliente = reader.GetNullableInt("ID_Cliente"),
                                Nome = reader.GetNullableString("Nome"),
                                Telefone = reader.GetNullableString("Telefone"),
                                Observacoes = reader.GetNullableString("Observacoes"),
                                Status = reader.GetNullableInt("Status"),
                                DataCadastro = reader.GetNullableDateTime("Data_Cadastro")
                            });
                        }
                    }
                }
            }

            return lista;
        }
        public Cliente BuscarPorId(int IdCliente)
        {
            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return null;

                string sql = @"SELECT ID_Cliente, 
                                      Nome, 
                                      CONCAT(
                                        '(', 
                                        SUBSTRING(Telefone, 1, 2), 
                                        ') ', 
                                        SUBSTRING(Telefone, 3, 5), 
                                        '-', 
                                        SUBSTRING(Telefone, 8, 4)
                                      ) AS Telefone,
                                      Observacoes, 
                                      Status, 
                                      Data_Cadastro
                               FROM Clientes 
                               WHERE ID_Cliente = @IdCliente
                               AND Status IN (1, 2)";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdCliente", IdCliente);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Cliente
                            {
                                IdCliente = reader.GetNullableInt("ID_Cliente"),
                                Nome = reader.GetNullableString("Nome"),
                                Telefone = reader.GetNullableString("Telefone"),
                                Observacoes = reader.GetNullableString("Observacoes"),
                                Status = reader.GetNullableInt("Status"),
                                DataCadastro = reader.GetNullableDateTime("Data_Cadastro")
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void Salvar(Cliente cliente)
        {
            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return;

                string sql;

                if (cliente.IdCliente > 0)
                {
                    sql = @"UPDATE Clientes 
                    SET Nome = @Nome,
                        Telefone = @Telefone,
                        Observacoes = @Observacoes, 
                        Status = @Status
                    WHERE ID_Cliente = @IdCliente";
                }
                else
                {
                    sql = @"INSERT INTO Clientes 
                    (Nome, Telefone, Observacoes, Status)
                    VALUES 
                    (@Nome, @Telefone, @Observacoes, @Status)";
                }

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                    cmd.Parameters.AddWithValue("@Telefone", cliente.Telefone);
                    cmd.Parameters.AddWithValue("@Observacoes", cliente.Observacoes);
                    cmd.Parameters.AddWithValue("@Status", cliente.Status);

                    if (cliente.IdCliente > 0)
                    {
                        cmd.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
