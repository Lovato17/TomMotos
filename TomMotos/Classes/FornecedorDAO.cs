﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TomMotos.Conexao;
using TomMotos.Model;

namespace TomMotos.Classes
{
    class FornecedorDAO
    {
        MySqlConnection conexao = ConnectionFactory.getConnection();

        public FornecedorDAO()
        {
        }

        #region METODO LISTAR
        public DataTable ListarTodosFornecedores()
        {

            //tem que mudar o select para a storeProcedur, porém tem que criar o metodo cadastra telefone, email e endereco

            string sql = @"select id_fornecedor as 'ID',nome_fornecedor as 'NOME',cnpj_fornecedor as 'CNPJ' from tb_fornecedor";

            MySqlCommand executacmdsql = new MySqlCommand(sql, conexao);

            conexao.Open();
            executacmdsql.ExecuteNonQuery();

            MySqlDataAdapter da = new MySqlDataAdapter(executacmdsql);

            DataTable tabelaFornecedor = new DataTable();
            da.Fill(tabelaFornecedor);

            conexao.Close();

            return tabelaFornecedor;
        }
        #endregion

        #region METODO CADASTRAR

        public void cadastrarFornecedor(FornecedorModel obj)
        {
            
            if (obj.nome == "")
            {
                MessageBox.Show("Preencha todos valores Obrigatorio! = *");
            }
            else
            {
                try
                {
                    string insert = @"CALL criacaoFornecedor(@nome, @cnpj)";

                    MySqlCommand executacmdsql = new MySqlCommand(insert, conexao);
                    executacmdsql.Parameters.AddWithValue("@nome", obj.nome);
                    executacmdsql.Parameters.AddWithValue("@cnpj", FornecedorModel.cnpj);

                    conexao.Open();
                    executacmdsql.ExecuteNonQuery();
                    conexao.Close();
                    MessageBox.Show("Cadastrado com sucesso!");
                }
                catch (Exception erro)
                {
                    MessageBox.Show("Cadastrado não Realizado!" + erro.Message);
                }
              
            }
            conexao.Close();
        }
        #endregion

        #region METODO ALTERAR
        public void alterar(FornecedorModel obj)
        {
            try
            {

                string update = @"CALL UpdateFornecedor (@nome, @cnpj,@id)";
                MySqlCommand executacmdsql = new MySqlCommand(update, conexao);
                executacmdsql.Parameters.AddWithValue("@id", obj.id);
                executacmdsql.Parameters.AddWithValue("@nome", obj.nome);
                executacmdsql.Parameters.AddWithValue("@cnpj", FornecedorModel.cnpj);
                conexao.Open();
                executacmdsql.ExecuteNonQuery();
                conexao.Close();
                MessageBox.Show("Alterado com Sucesso!");
            }
            catch (Exception erro)
            {
                MessageBox.Show("Aconteceu um Erro" + erro.Message);
            }
            conexao.Close();

        }
        #endregion

        #region METODO EXCLUIR
        public void Excluir(FornecedorModel obj)
        {
            try
            {
                string delete = @"Delete from  tb_usuario  where fk_fornecedor_id=@id";
                string delete2 = @"Delete from  tb_fornecedor  where id_fornecedor=@id";
                MySqlCommand executacmdsql = new MySqlCommand(delete, conexao);
                executacmdsql.Parameters.AddWithValue("@id", obj.id);
                Thread.Sleep(500);
                MySqlCommand executacmdsql2 = new MySqlCommand(delete2, conexao);
                executacmdsql.Parameters.AddWithValue("@id", obj.id);
                conexao.Open();
                executacmdsql.ExecuteNonQuery();
                Thread.Sleep(500);
                executacmdsql2.ExecuteNonQuery();
                conexao.Close();
            }
            catch (Exception erro)
            {
                MessageBox.Show("Aconteceu um Erro" + erro);
            }
            conexao.Close();

        }
        #endregion
    }
}

