//Data Acess Layer =  DAL (camada de acesso a dados)
using System;

using System.Data;
using System.Data.SqlClient;

namespace CRUD3Camadas.DataAcess
{
    public class SQLServerDAL
    {
        private SqlConnection obterConexao()
        {
            //"Data Source=localhost;Initial Catalog=db_CRUD3Camadas; Trusted_Connection= Yes"
            string connectionString = CRUD3Camadas.DataAcess.Properties.Settings.Default.StringConexaoSqlServer;
            return new SqlConnection(connectionString);
        }

        private SqlParameterCollection sqlParamterCollection = new SqlCommand().Parameters;

        public void LimparParametros()
        {
            sqlParamterCollection.Clear();
        }

        public void AdicionarParametros(string nomeParametro, object valorParametro)
        {
            //IMPORTANTE: Os nomes dos parametros tem que ser os mesmo da StoredProcedures
            sqlParamterCollection.Add(new SqlParameter(nomeParametro, valorParametro));
        }

        //Inseri, Altera, Exclui dados
        public void Executar(CommandType commandType, string StoredProcedureOuQuery)
        {
            SqlConnection sqlConnection = obterConexao();

            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandText = StoredProcedureOuQuery;
            sqlCommand.CommandTimeout = 240;  //Segundos

            //Adiciona os parametros no comando
            foreach(SqlParameter sqlParameter in sqlParamterCollection)
                sqlCommand.Parameters.AddWithValue(sqlParameter.ParameterName, sqlParameter.Value);

            try
            {
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();                
            }
            catch(Exception ex)
            {
                throw ex;//(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        //Caso o procedimento retorna uma unica celula
        public object ExecutarScalar(CommandType commandType, string StoredProcedureOuQuery)
        {
            SqlConnection sqlConnection = obterConexao();
            
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandText = StoredProcedureOuQuery;
            sqlCommand.CommandTimeout = 240;  //Segundos

            //Adiciona os parametros no comando
            foreach(SqlParameter sqlParameter in sqlParamterCollection)
                sqlCommand.Parameters.AddWithValue(sqlParameter.ParameterName, sqlParameter.Value);

            try
            {
                sqlConnection.Open();
                return sqlCommand.ExecuteScalar();

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        //Retornara varios registros
        public DataTable ExecutarConsulta(CommandType commandType, string StoredProcedureOuQuery)
        {
            SqlConnection sqlConnection = obterConexao();

            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandText = StoredProcedureOuQuery;
            sqlCommand.CommandTimeout = 240;  //Segundos

            //Adiciona os parametros no comando
            foreach(SqlParameter sqlParameter in sqlParamterCollection)
                sqlCommand.Parameters.AddWithValue(sqlParameter.ParameterName, sqlParameter.Value);

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            //Crio dataTable vazio
            DataTable dataTable = new DataTable();
            //Preenche o dataTable
            try
            {
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Retornara apenas um registro (optimizaçao de processamento)
        public SqlDataReader ExecutarConsultaPorPK(CommandType commandType, string StoredProcedureOuQuery)
        {
            SqlConnection sqlConnection = obterConexao();

            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandText = StoredProcedureOuQuery;
            sqlCommand.CommandTimeout = 240;  //Segundos


            //Adiciona os parametros no comando
            foreach(SqlParameter sqlParameter in sqlParamterCollection)
                sqlCommand.Parameters.AddWithValue(sqlParameter.ParameterName, sqlParameter.Value);

            try
            {
                return sqlCommand.ExecuteReader();
            }catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
