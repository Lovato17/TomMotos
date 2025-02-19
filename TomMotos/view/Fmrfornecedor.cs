﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TomMotos.Classes;
using TomMotos.Conexao;
using TomMotos.Model;

namespace TomMotos.view
{

    public partial class Fmrfornecedor : Form
    {
        string idUser,nome;
        bool CNPJ;
        MySqlConnection conexao = ConnectionFactory.getConnection();
        FornecedorDAO Cadastro = new FornecedorDAO();
        FiltroDAO Filtro = new FiltroDAO();
        public Fmrfornecedor()
        {
            InitializeComponent();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                FornecedorModel obj = new FornecedorModel();
                CNPJ = true;
                obj.nome = txt_nome.Text.ToUpper();
                if (txt_cnpj.Text == "  ,   ,   /    -") FornecedorModel.cnpj = null;  // É interessante perceber que isso não deve ser usado para alguns ints, por exemplo de quantidade tendo em vista
                else validarCNPJ();
                if (CNPJ == true)
                {
                    Cadastro.cadastrarFornecedor(obj);

                    dg_fornecedor.DataSource = Cadastro.ListarTodosFornecedores();
                    txt_id.Text = dg_fornecedor.Rows[dg_fornecedor.Rows.Count - 1].Cells[0].Value.ToString();
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro: " + erro.Message);
            }
        }

        private void Fmrfornecedor_Load(object sender, EventArgs e)
        {
            dg_fornecedor.DataSource = Cadastro.ListarTodosFornecedores();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (txt_id.Text != "")
            {
            
            try
            {
                 CNPJ = true;
                FornecedorModel obj = new FornecedorModel();
                obj.id = int.Parse(txt_id.Text);
                obj.nome = txt_nome.Text.ToUpper();
                if (txt_cnpj.Text == "  ,   ,   /    -") FornecedorModel.cnpj = null;  // É interessante perceber que isso não deve ser usado para alguns ints, por exemplo de quantidade tendo em vista
                else validarCNPJ();
                    if (CNPJ == true)
                    {
                        FornecedorDAO dao = new FornecedorDAO();
                        dao.alterar(obj);
                        dg_fornecedor.DataSource = dao.ListarTodosFornecedores();
                    }

                
            }
            catch (Exception erro)
            {
                MessageBox.Show("Aconteceu algum erro" + erro);
            }
            }
            else MessageBox.Show("Escolha um id que deseja Alterar", "Erro",  MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (txt_id.Text != "") { 
            var result = MessageBox.Show("Deseja excluir o Fornecedor " + txt_nome.Text + "?", "EXCLUIR",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Exclamation);
            if (result == DialogResult.Yes)
            {
                try
                {
                    FornecedorModel obj = new FornecedorModel();
                    obj.id = int.Parse(txt_id.Text);

                    FornecedorDAO dao = new FornecedorDAO();
                    dao.Excluir(obj);
                    dg_fornecedor.DataSource = dao.ListarTodosFornecedores();
                    MessageBox.Show("Excluido com Sucesso!");
                }
                catch (Exception erro)
                {
                        MessageBox.Show("Não foi possivel excluir", "EXCLUIR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
            }

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (txt_id.Text != "")
            {
                try
                {
                    nome = ("CADASTRO DE EMAIL DO FORNECEDOR " + txt_nome.Text).ToUpper();
                    string select = "select id_usuario from tb_usuario where fk_fornecedor_id =" + txt_id.Text;
                    MySqlCommand executacmdsql = new MySqlCommand(select, conexao);
                    conexao.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(executacmdsql);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        idUser = ds.Tables[0].Rows[0]["id_usuario"].ToString();
                        EmailModel.id = idUser;
                    }
                    
                    conexao.Close();
                    
                    Fmremail nomeCliente = new Fmremail(nome);
                    nomeCliente.Show();
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.ToString());
                }

            }
            else
            {
                MessageBox.Show("Escolha um Fornecedor que deseja cadastrar o email", "Erro",
             MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (txt_id.Text != "")
            {
                try
                {
                    nome = ("CADASTRO DE ENDEREÇO DO FORNECEDOR " + txt_nome.Text).ToUpper();
                    string select = "select id_usuario from tb_usuario where fk_fornecedor_id =" + txt_id.Text;
                    MySqlCommand executacmdsql = new MySqlCommand(select, conexao);
                    conexao.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(executacmdsql);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        idUser = ds.Tables[0].Rows[0]["id_usuario"].ToString();
                        EnderecoModel.id = idUser;
                    }
                    conexao.Close();
                    

                    Fmrendereco destino = new Fmrendereco(nome);
                    destino.Show();
                }
                catch (Exception erro)
                {
                    MessageBox.Show("Ouve um Erro" + erro);
                }
            }
            else
            {
                MessageBox.Show("Escolha um Fornecedor que deseja cadastrar o endereco", "Erro",
             MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void Fmrfornecedor_FormClosed(object sender, FormClosedEventArgs e)
        {
            Fmrsumario fmrsumario = new Fmrsumario();
            fmrsumario.Show();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbxBuscar.Text != "")
                {
                    string campo = cbxBuscar.Text.ToString() + "_fornecedor";
                    FiltroModel.campoWhere = @"select * from tb_fornecedor where " + campo.ToLower() + " like " + "'%" + txtBuscar.Text.ToString() + "%'";
                    FiltroDAO dao = new FiltroDAO();
                    dg_fornecedor.DataSource = dao.buscaCargo();
                }
            }
            catch (Exception erro) { MessageBox.Show("Ouve um Erro " + erro); }
        }
        private void validarCNPJ()
        {
            //verificação de cnpj
            txt_cnpj.TextMaskFormat = MaskFormat.IncludeLiterals;
            string cnpj = txt_cnpj.Text;
            bool verFal = validacaoTxtDAO.validarCnpj(cnpj);

            if (verFal)
            {
                FornecedorModel.cnpj = txt_cnpj.Text;
            }
            else { MessageBox.Show("CNPJ INVÁLIDO"); CNPJ = false; }
        }

        private void dg_fornecedor_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            txt_id.Text = dg_fornecedor.CurrentRow.Cells[0].Value.ToString();
            txt_nome.Text = dg_fornecedor.CurrentRow.Cells[1].Value.ToString();
            txt_cnpj.Text = dg_fornecedor.CurrentRow.Cells[2].Value.ToString();
        }

        private void btnMostrarTudo_Click(object sender, EventArgs e)
        {
            dg_fornecedor.DataSource = Cadastro.ListarTodosFornecedores();
        }

        private void btnBuscar_Click_1(object sender, EventArgs e)
        {
            if (cbxBuscar.Text != "")
            {
                string campo = cbxBuscar.Text.ToString();
                string finalSQL = "";

                if (campo == "ID") campo = "tb_fornecedor.id_fornecedor";
                if (campo == "NOME") campo = "tb_fornecedor.nome_fornecedor";
                if (campo == "CNPJ") campo = "tb_fornecedor.cnpj_fornecedor";
                finalSQL += campo.ToLower() + " like " + "'%" + txtBuscar.Text.ToString() + "%'";

                FiltroModel.campoWhere = finalSQL;

                dg_fornecedor.DataSource = Filtro.buscaFornecedor();
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txt_id.Text = "";
            txt_nome.Text = "";
            txt_cnpj.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (txt_id.Text != "")
            {
                try
                {
                    nome = ("CADASTRO DE TELEFONE DO FORNECEDOR " + txt_nome.Text).ToUpper();
                    string select = "select id_usuario from tb_usuario where fk_fornecedor_id =" + txt_id.Text;
                    MySqlCommand executacmdsql = new MySqlCommand(select, conexao);
                    conexao.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(executacmdsql);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        idUser = ds.Tables[0].Rows[0]["id_usuario"].ToString();
                        TelefoneModel.id = idUser;
                    }
                    conexao.Close();
                    

                    Fmrtelefone destino = new Fmrtelefone(nome);
                    destino.Show();
                }
                catch (Exception erro)
                {
                    MessageBox.Show("Ouve um Erro" + erro);
                }
            }
            else
            {
                MessageBox.Show("Escolha um Fornecedor que deseja cadastrar o telefone", "Erro",
             MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
