using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CRUD3Camadas.TransferObject;
using CRUD3Camadas.Business;

namespace CRUD3Camadas
{
    public partial class frmCadastro : Form
    {

        enumTela tipoTela;

        public frmCadastro(enumTela acaoTela, Cliente cliente)
        {
            InitializeComponent();            
            
            tipoTela = acaoTela;

            switch(acaoTela)
            {
                case enumTela.Inserir:
                    this.Text = "Cadastro de Cliente";                    
                    btnCadastrarAlterar.Text = "Cadastrar";
                    txtCodigo.Enabled = false;
                    break;
                case enumTela.Alterar:
                    this.Text = "Alterar Cliente";
                    groupBox1.Text = "Alterações";
                    btnCadastrarAlterar.Text = "&Salvar Alterações";
                    CarregarInformacoes(cliente);
                    break;
                case enumTela.Consultar:
                    this.Text = "Consulta de Cliente";
                    groupBox1.Text = "Consulta";
                    btnCadastrarAlterar.Visible = false;
                    btnLimpar.Visible = false;
                    btnCancelar.Text = "Fechar";
                    CarregarInformacoes(cliente);
                    DesabilitarCampos(this);
                    btnCancelar.Focus();
                    break;
                default:
                    break;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {                     
            if(tipoTela == enumTela.Consultar)
            {
                this.Close();                
            }
            else if(tipoTela == enumTela.Inserir)
            {
                //verificar se tem algo nos texto

                //confirmar o fechamento
            }
            else //Alterar
            {
                //verificar se foi alterado

                //confirmar o fechamento
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparTodosCampos(this);
        }

        private void CarregarInformacoes(Cliente cliente)
        {
            txtCodigo.Text = cliente.Id.ToString();
            txtNome.Text = cliente.Nome.ToString();
            dtpDataNascimento.Value = cliente.DataNascimento;

            if((char)cliente.Sexo == 'F')
                rdoFeminino.Checked = true;
            else
                rdoMasculino.Checked = true;

            mtxtTelefone.Text = cliente.Telefone.ToString();
            txtEmail.Text = cliente.Email.ToString();
            txtLimiteCompra.Text = cliente.LimiteCompra.ToString();
        }

        //Método recursivo
        private void DesabilitarCampos(Control controle)
        {
            foreach(Control ct in controle.Controls)
            {
                //Somente os botoes poderá ser manipulados pelo TAB
                if(!(ct is Button))
                    ct.TabStop = false;

                if(ct is TextBox)
                    (ct as TextBox).ReadOnly = true;
                if(ct is MaskedTextBox)
                    (ct as MaskedTextBox).ReadOnly = true;
                if(ct is DateTimePicker)
                    (ct as DateTimePicker).Enabled = false;
                if(ct is RadioButton)
                    (ct as RadioButton).Enabled = false;
                if(ct.HasChildren)
                    DesabilitarCampos(ct);
            }
        }

        //Método recursivo
        private void LimparTodosCampos(Control controle)
        {
            foreach(Control ctr in controle.Controls)
            {
                if(ctr is TextBox)
                    (ctr as TextBox).Text = string.Empty;
                if(ctr is MaskedTextBox)
                    (ctr as MaskedTextBox).Text = string.Empty;
                if(ctr is DateTimePicker)
                    (ctr as DateTimePicker).Value = DateTime.Now;
                if(ctr is RadioButton)
                    (ctr as RadioButton).Checked = false;
                if(ctr.HasChildren)
                    LimparTodosCampos(ctr);
            }
        }

        private void btnCadastrarSalvar_Click(object sender, EventArgs e)
        {
            Cliente cliente = new Cliente();

            cliente.Nome = txtNome.Text;
            cliente.Sexo = rdoMasculino.Checked ? 'M' : 'F';
            cliente.DataNascimento = dtpDataNascimento.Value;
            cliente.Email = txtEmail.Text;
            cliente.Telefone = mtxtTelefone.Text;

            //Validaçao do campo de valor
            float limite;
            bool ValorValido = float.TryParse(txtLimiteCompra.Text, out limite);
            if(!ValorValido)
            {
                MessageBox.Show("O valor da Renda Está incorreto");
                return;
            }
            cliente.LimiteCompra = limite;

            try
            {
                if(tipoTela == enumTela.Inserir)
                    new ClienteBLL().Inserir(cliente);

                else if(tipoTela == enumTela.Alterar)
                {
                    cliente.Id = int.Parse(txtCodigo.Text);
                    new ClienteBLL().Alterar(cliente);
                }

                LimparTodosCampos(this);
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro do Exception:"+ ex.Message);
            }
        }
    }
}
