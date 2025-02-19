﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TomMotos.Classes;
using TomMotos.Model;

namespace TomMotos.view
{
    public partial class FmrOrcamento : Form
    {
        string[] item = new string[7];
        VendaDAO VendaDAO = new VendaDAO();
        Fmrcaixa fmrCx = new Fmrcaixa();
        ProdutoUsadoDAO ProdutoUsadoDAO = new ProdutoUsadoDAO();
        ServicoDAO ServicoPrestadoDAO = new ServicoDAO();
        FiltroDAO Filtro = new FiltroDAO();

        public FmrOrcamento()
        {
            InitializeComponent();
        }

        private void FmOrcamento_Load(object sender, EventArgs e)
        {
            cxbData.Checked = true;
            dtp1.Value = dtp1.Value.AddDays(-14);
            pesquisarVendaComFiltro(); //Puxa todos os orçamentos de 2 semanas para cá
        }

        private void dgOrcamento_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgOrcamento.CurrentCell.Value.ToString() != "")
                {
                    fmrCx.Show();
                    carregarVenda(int.Parse(dgOrcamento.CurrentRow.Cells[0].Value.ToString())); // ID VENDA
                    fmrCx.lbl_buscarCliente.Text = CaixaModel.fk_cliente_id;
                    fmrCx.lbl_BuscarVeiculo.Text = CaixaModel.fk_veiculo_id;
                    fmrCx.lblSubitotal.Text = string.Format("{0:#,##0.00}", double.Parse(CaixaModel.totalVenda_orcamento));
                    CaixaModel.id_orcamento = dgOrcamento.CurrentRow.Cells[0].Value.ToString();
                    DesabilitarComponentes();
                    this.Close();
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro "+erro.Message);
            }
        }

        public void carregarVenda(int idVenda)
        {
            carregarProdutoUsados(idVenda);
            carregarServicosFeitos(idVenda);
            carregarLblTotal(idVenda);
        }

        public void carregarLblTotal(int idVenda)
        {
            string totalVenda = VendaDAO.listarTotalVendaPorId(idVenda);
            fmrCx.lblSubitotal.Text = totalVenda;
        }
        public void DesabilitarComponentes() {

            fmrCx.btn_BuscarCliente.Enabled = false;
            fmrCx.btn_buscarVeiculo.Enabled = false;
            fmrCx.cBoxOrcamento.Enabled = false;
        }
        public void carregarServicosFeitos(int idVenda)
        {
            try
            {
                int ITEM = 1;
                ArrayList resultado = ServicoPrestadoDAO.listarPorVenda(idVenda);
                if (resultado.Count > 0)
                    for (int i = -1; i < resultado.Count; ITEM++)
                    {
                        if (ITEM == 1) i++;
                        string DESCRICAO = resultado[i].ToString(); i++;
                        string VALOR = resultado[i].ToString(); i++;

                        item[0] = ITEM.ToString();
                        item[1] = DESCRICAO;
                        item[2] = string.Format("{0:#,##0.00}", double.Parse(VALOR));

                        fmrCx.dgServicos.Rows.Add(item);
                    }
            }
            catch (Exception erro) { MessageBox.Show("Test " + erro); }
        }
        public void carregarProdutoUsados(int idVenda)
        {
            try
            {
                int ITEM = 1;
                ArrayList resultado = ProdutoUsadoDAO.listarPorVenda(idVenda);
                if(resultado.Count > 0) {
                    for (int i = -1; i < resultado.Count; ITEM++)
                    {
                        if (ITEM == 1) i++;
                        string CODIGO = resultado[i].ToString(); i++;
                        string DESCRICAO = resultado[i].ToString(); i++;
                        string QTD = resultado[i].ToString(); i++;
                        string VLUNIT = resultado[i].ToString(); i++;
                        string DESCONTO = resultado[i].ToString(); i++;

                        item[0] = ITEM.ToString();
                        item[1] = CODIGO;
                        item[2] = DESCRICAO;
                        item[3] = QTD;
                        item[4] = string.Format("{0:#,##0.00}", double.Parse(VLUNIT));
                        item[5] = string.Format("{0:P}", double.Parse(DESCONTO)/100);
                        item[6] = string.Format("{0:#,##0.00}", double.Parse(QTD) * double.Parse(VLUNIT) - double.Parse(QTD) * double.Parse(VLUNIT) * double.Parse(DESCONTO) / 100);

                        fmrCx.dgProdutos.Rows.Add(item);
                    }
                }
            }
            catch (Exception erro) { MessageBox.Show("Test " + erro); }
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisarVendaComFiltro();
        }

        private void pesquisarVendaComFiltro()
        {
            try
            {
                string campo = cbxBuscar.Text.ToString();
                string finalSQL = "tb_venda.e_orcamento = true";
                if (campo == "ID DO ORÇAMENTO") campo = "tb_venda.id_venda";
                if (campo == "NOME DO CLIENTE") campo = "tb_cliente.nome_cliente";
                if (campo == "CPF DO CLIENTE") campo = "tb_cliente.cpf_cliente";
                if (campo == "CNPJ DO CLIENTE") campo = "tb_cliente.cnpj_cliente";
                if (campo != "") finalSQL += " AND " + campo.ToLower() + " like " + "'%" + txtBuscar.Text.ToString() + "%'";
                if (cxbData.Checked)
                {
                    finalSQL = finalSQL + " AND tb_venda.data_venda BETWEEN ' " + dtp1.Value.ToString("yyyy/MM/dd") + " 00:00:00' AND ' " + dtp2.Value.ToString("yyyy/MM/dd") + " " + "23:59:59'";
                }
                FiltroModel.campoWhere = finalSQL;

                dgOrcamento.DataSource = Filtro.buscaVenda();
            }
            catch (Exception erro) { MessageBox.Show("Ouve um Erro " + erro.Message); }
        }

        private void btn_mostrar_tudo_Click(object sender, EventArgs e)
        {
            dgOrcamento.DataSource = VendaDAO.listarTodos(true); //Puxa todos os orçamentos
            txtBuscar.Text = "";
            cbxBuscar.Text = "";
            cxbData.Checked = false;
        }
    }
}
