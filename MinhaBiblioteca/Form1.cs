using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MinhaBiblioteca
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GlobalDB.conectar();
            //txtId.Text = proximoId();
            txtId.Text = "0";
        }



        private void btnSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            limparCampos();
        }

        private void limparCampos()
        {
            txtId.Text = proximoId();
            txtTitulo.Text = "";
            txtAutor.Text = "";
            txtAssunto.Text = "";
            txtEditora.Text = "";
            txtTitulo.Focus();
            btnIncluir.Enabled = true;
        }

        private bool validarCampos()
        {
            string mensagem = "Validação dos Campos Obrigatórios\n\n";
            bool status = true;

            if (txtTitulo.Text.Length < 2)
            {
                mensagem += "\n Campo titulo vazio";
                txtTitulo.Focus();
                status = false;
            }
            else
            {
                if (txtAutor.Text.Length == 0)
                {
                    mensagem += "\n Campo autor vazio";
                    txtAutor.Focus();
                    status = false;
                }
            }
            
            if (status == false)
                MessageBox.Show(mensagem, "Dados Incorretos", MessageBoxButtons.OK, MessageBoxIcon.Warning);


            return status;
        }

        private string proximoId()
        {

            int proximoId = 0;

            if (GlobalDB.ultimoId().Equals(null))
                proximoId = 1;
            else
                proximoId = Int32.Parse(GlobalDB.ultimoId()) + 1;


            return proximoId.ToString();

        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            if (validarCampos())
            {
                GlobalDB.cadastrar(txtTitulo.Text, txtAutor.Text, txtAssunto.Text, txtEditora.Text);
                limparCampos();
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            Livro l = new Livro(Int32.Parse(txtId.Text), txtTitulo.Text, txtAutor.Text, txtAssunto.Text, txtEditora.Text);
            GlobalDB.alterar(l);
            limparCampos();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja mesmo excluir este registro?", "Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                GlobalDB.excluir(txtId.Text);

            limparCampos();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            btnIncluir.Enabled = false;
            List<Livro> listaLivro = GlobalDB.consultar(txtTitulo.Text);

            foreach (var l in listaLivro){

                txtId.Text = l.Id.ToString();
                txtTitulo.Text = l.Titulo.ToString();
                txtAutor.Text = l.Autor.ToString();
                txtAssunto.Text = l.Assunto.ToString();
                txtEditora.Text = l.Editora.ToString();


                MessageBox.Show(l.ToString(), "Consulta", MessageBoxButtons.OK, MessageBoxIcon.Information);

/*                var resposta = MessageBox.Show("Gostaria de ver o próximo registro?", "Consulta", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (resposta == DialogResult.No)
                    break;
                else
                    continue;*/
            }
/*
            int contador = 0;
            if (listaPessoas.Count() != 0)
            {
                do
                {
                    txtId.Text = listaPessoas[contador].Id.ToString();
                    txtNome.Text = listaPessoas[contador].Nome.ToString();
                    txtTelefone.Text = listaPessoas[contador].Telefone.ToString();
                    txtEmail.Text = listaPessoas[contador].Email.ToString();
                    
                    if (listaPessoas.Count() > 0)
                        if (MessageBox.Show("Gostaria de ver o próximo registro?", "Consulta", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                            break;

                    contador++;

                } while (contador < listaPessoas.Count());
            }*/
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            string listagem = GlobalDB.listar();
            MessageBox.Show(listagem);
        }
    }
}
