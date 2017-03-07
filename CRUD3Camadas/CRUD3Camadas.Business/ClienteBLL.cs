//Business Logic Layer = BLL (camada de regras de negocios)
using System;

using System.Data;
using CRUD3Camadas.DataAcess;
using CRUD3Camadas.TransferObject;

namespace CRUD3Camadas.Business
{
    public class ClienteBLL
    {
        SQLServerDAL acessoDadosSqlServer = new SQLServerDAL();

        public void Inserir(Cliente cliente)
        {
            acessoDadosSqlServer.LimparParametros();
            //IMPORTANTE: Os nomes dos parametros tem que ser os mesmo da StoredProcedures
            acessoDadosSqlServer.AdicionarParametros("@nome", cliente.Nome);
            acessoDadosSqlServer.AdicionarParametros("@sexo", cliente.Sexo);
            acessoDadosSqlServer.AdicionarParametros("@tel", cliente.Telefone);
            acessoDadosSqlServer.AdicionarParametros("@dtnasc", cliente.DataNascimento);
            acessoDadosSqlServer.AdicionarParametros("@email", cliente.Email);
            acessoDadosSqlServer.AdicionarParametros("@limite", cliente.LimiteCompra);
            try
            {                
                acessoDadosSqlServer.Executar(CommandType.StoredProcedure, "spInserirCliente");
            }
            catch
            {
                throw;
            }
        }

        public void Alterar(Cliente cliente)
        {
            acessoDadosSqlServer.LimparParametros();
            acessoDadosSqlServer.AdicionarParametros("@idCliente", cliente.Id);
            acessoDadosSqlServer.AdicionarParametros("@nome", cliente.Nome);
            acessoDadosSqlServer.AdicionarParametros("@sexo", cliente.Sexo);
            acessoDadosSqlServer.AdicionarParametros("@tel", cliente.Telefone);
            acessoDadosSqlServer.AdicionarParametros("@email", cliente.Email);
            acessoDadosSqlServer.AdicionarParametros("@dtnasc", cliente.DataNascimento);
            acessoDadosSqlServer.AdicionarParametros("@limite", cliente.LimiteCompra);
            try
            {
                acessoDadosSqlServer.Executar(CommandType.StoredProcedure, "spAlterarCliente");
            }
            catch
            {
                throw;
            }
        }

        public void Excluir(Cliente cliente)
        {
            acessoDadosSqlServer.LimparParametros();
            acessoDadosSqlServer.AdicionarParametros("@idCliente", cliente.Id);
            try
            {
                acessoDadosSqlServer.Executar(CommandType.StoredProcedure, "spExcluirCliente");
            }
            catch
            {
                throw;
            }
        }

        public void ExcluirPeloID(int idCliente)
        {
            acessoDadosSqlServer.LimparParametros();
            acessoDadosSqlServer.AdicionarParametros("@idCliente", idCliente);
            try
            {
                acessoDadosSqlServer.Executar(CommandType.Text, "DELETE FROM Cliente where Id = @id");//"DELETE FROM Cliente WHERE Id = @Id");
                //acessoDadosSqlServer.Executar(CommandType.StoredProcedure, "spExcluirCliente");
            }
            catch
            {
                throw;
            }
        }

        public ClienteCollection ConsultarPorNome(string nome)
        {
            ClienteCollection clienteColecao = null;
            acessoDadosSqlServer.LimparParametros();
            acessoDadosSqlServer.AdicionarParametros("@Nome", nome);

            try
            {
                DataTable dataTableCliente = acessoDadosSqlServer.ExecutarConsulta(CommandType.StoredProcedure, "spConsultarClientePorNome");

                if(dataTableCliente.Rows.Count > 0)
                {                    
                    clienteColecao = new ClienteCollection();

                    //Percorre o dataTable e transformar em coleção cliente
                    //Para cada linha da linha da tabela
                    foreach(DataRow linha in dataTableCliente.Rows)
                    {
                        //Criar um cliente
                        Cliente cliente = new Cliente();
                        //Adicionar os dados da linha nele
                        cliente.Id = (int)linha["Id"];
                        cliente.Nome = (string)linha["Nome"];
                        cliente.Sexo = Convert.ToChar(linha["Sexo"]);
                        cliente.Telefone = (string)linha["Telefone"];
                        cliente.Email = (string)linha["Email"];
                        cliente.DataNascimento = (DateTime)linha["DataNascimento"];
                        cliente.LimiteCompra = float.Parse(linha["LimiteCompra"].ToString());
                        //Adicionar na coleçao
                        clienteColecao.Add(cliente);
                    }                    
                }
            }
            catch
            {
                throw;
            }

            return clienteColecao;
        }

        public Cliente ConsultarPorId(int idCliente)
        {
            Cliente cliente = null;
            acessoDadosSqlServer.LimparParametros();
            acessoDadosSqlServer.AdicionarParametros("@Id", idCliente);

            try
            {
                System.Data.SqlClient.SqlDataReader dr;
                dr = acessoDadosSqlServer.ExecutarConsultaPorPK(CommandType.StoredProcedure, "spConsultarClientePorID");
                if(dr.HasRows)
                {   
                    cliente = new Cliente();
                    while(dr.Read())
                    {
                        cliente.Id = idCliente;
                        cliente.Nome = (string)dr["Nome"];
                        cliente.Sexo = (char)dr["Sexo"];
                        cliente.Telefone = (string)dr["Telefone"];
                        cliente.Email = (string)dr["Email"];
                        cliente.DataNascimento = (DateTime)dr["DataNascimento"];
                        cliente.LimiteCompra = (float)dr["LimiteCompra"];
                    }
                }

            }
            catch
            {                
                throw;
            }

            return cliente;
        }
    }
}
