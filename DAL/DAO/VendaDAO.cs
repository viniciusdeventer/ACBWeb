using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ACBWeb.Models;
using ACBWeb.DAL;

namespace ACBWeb.DAL.DAO
{
    public class VendaDAO
    {
        public List<Venda> GetVendas()
        {
            var lista = new List<Venda>();

            using (var conn = Conexao.GetConnection())
            {
                if (conn == null) return lista;

                string sql = @"SELECT 
                                    V.*,
                                    CASE V.Tipo_Venda 
                                        WHEN 1 THEN P.Nome 
                                        WHEN 2 THEN Cli.Nome
                                    END AS Nome_Item
                                FROM Vendas V
                                LEFT JOIN Produtos P ON P.ID_Produto = V.ID_Item
                                LEFT JOIN Contas C ON C.ID_Conta = V.ID_Item
                                LEFT JOIN Clientes Cli ON C.ID_Cliente = Cli.ID_Cliente";

                using (var cmd = new MySqlCommand(sql, conn))
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
                            DataCadastro = reader.GetNullableDateTime("Data_Cadastro")
                        });
                    }
                }
            }

            return lista;
        }
    }
}