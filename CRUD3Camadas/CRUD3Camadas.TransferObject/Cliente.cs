//Data Transfer Object = DTO
using System;

namespace CRUD3Camadas.TransferObject
{
    public class Cliente
    {
        public int Id { get; set; }
        private string nome;
        public char Sexo { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
        public float LimiteCompra { get; set; }

        public string Nome
        { 
            get{ return nome; }
            set{ this.nome = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower()); }      
        }
        //Construtor
        public Cliente()
        {
        
        }

    }
}
