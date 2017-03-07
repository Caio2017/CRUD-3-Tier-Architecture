using System;
using System.Windows.Forms;

using CRUD3Camadas.Business;
using CRUD3Camadas.TransferObject;

namespace CRUD3Camadas
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

            //Desativando autogeramento de colunas
            dataGridView1.AutoGenerateColumns = false;
            //Carregar a grid
            AtualizarGridClientes();
            //tiro a seleção do 1º item do datagrid           
            
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            AtualizarGridClientes();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            //verificaçao se possui registro selecionado
            if(dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Nenhum Cliente selecionado", "Não foi possivel excluir", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Realizar a confirmaçao da exclusão
            if(DialogResult.No == MessageBox.Show("Realmente deseja Excluir?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                return;

            //pegar cliente selecionado
            Cliente clienteSelecionado = (Cliente)dataGridView1.SelectedRows[0].DataBoundItem;

            try
            {
                new ClienteBLL().Excluir(clienteSelecionado);
                MessageBox.Show("Cliente Excluido com Sucesso", "Sucesso");
                AtualizarGridClientes();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Não Foi Possivel Excluir \r\n\r\nDetalhes:\r\n"+ex.Message, "Erro Inesperado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            frmCadastro formCadastro = new frmCadastro(enumTela.Inserir, null);

            if(formCadastro.ShowDialog() == DialogResult.OK)
                AtualizarGridClientes();
            //if(new frmCadastro(enumTela.Consultar, null).ShowDialog() == DialogResult.OK)
            //    AtualizarGridClientes();  
        
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnConsultar.Enabled = true;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnConsultar_Click(null, null);
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            Cliente clienteSelecionado = (Cliente)dataGridView1.SelectedRows[0].DataBoundItem;
            new frmCadastro(enumTela.Consultar, clienteSelecionado).Show();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            Cliente clienteSelecionado = (Cliente)dataGridView1.SelectedRows[0].DataBoundItem;
            
            if(new frmCadastro(enumTela.Alterar, clienteSelecionado).ShowDialog() == DialogResult.OK)
                AtualizarGridClientes();
        }

        private void AtualizarGridClientes()
        {
            ClienteBLL clienteNegocios = new ClienteBLL();

            ClienteCollection clientes = new ClienteCollection();
            try
            {
                clientes = clienteNegocios.ConsultarPorNome(txtPesquisar.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Não foi possivel consultar o cliente por Nome /r/nDetalhes: "+ex.Message);
            }
            dataGridView1.DataSource = clientes;
            dataGridView1.Update();
            dataGridView1.Refresh();

            //dataGridView1.ClearSelection();
            //dataGridView1.CurrentRow.Selected = false;
            lblQuantidade.Text = "Quantidade: " + dataGridView1.RowCount.ToString();
        }
    }
}
