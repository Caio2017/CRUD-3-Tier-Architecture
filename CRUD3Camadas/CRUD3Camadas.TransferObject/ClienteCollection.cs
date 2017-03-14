using System.Collections.Generic;

namespace CRUD3Camadas.TransferObject
{
    public class ClienteCollection : List<Cliente>
    {
        //Permitindo que receba nenhum ou varios clientes pelo Construtor
        public ClienteCollection(params Cliente[] clientes)
        {
			foreach(Cliente cliente in clientes)
				this.Add(cliente);
        }
    }
}
