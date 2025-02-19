﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TomMotos.Conexao;
using TomMotos.Model;

namespace TomMotos.Classes
{
    class VeiculoDAO
    {
        MySqlConnection conexao = ConnectionFactory.getConnection();

        public VeiculoDAO()
        {
        }

        #region METODO LISTAR
        public DataTable ListarTodosVeiculos()
        {
            string sql = @"select tb_veiculo.id_veiculo as 'ID VEICULO', marca_veiculo AS 'MARCA', modelo_veiculo AS 'MODELO', 
            cor_veiculo AS 'COR', ano_veiculo AS 'ANO', km_veiculo AS 'KM RODADO', placa_veiculo AS 'PLACA', 
            obs_veiculo AS 'OBSERVAÇÃO', tb_veiculo.fk_cliente_id AS 'CLIENTE(ID)' , tb_cliente.nome_cliente as 'NOME CLIENTE' from tb_veiculo
            inner join tb_cliente
            on tb_cliente.id_cliente = tb_veiculo.fk_cliente_id";

            MySqlCommand executacmdsql = new MySqlCommand(sql, conexao);

            conexao.Open();
            executacmdsql.ExecuteNonQuery();

            MySqlDataAdapter da = new MySqlDataAdapter(executacmdsql);

            DataTable tabelaVeiculo = new DataTable();
            da.Fill(tabelaVeiculo);

            conexao.Close();

            return tabelaVeiculo;
        }
        #endregion
        
        #region METODO LISTAR CLIENTE
        public DataTable ListarTodosClientes()
        {
            string sql = @"select * from tb_cliente";

            MySqlCommand executacmdsql = new MySqlCommand(sql, conexao);

            conexao.Open();
            executacmdsql.ExecuteNonQuery();

            MySqlDataAdapter da = new MySqlDataAdapter(executacmdsql);

            DataTable tabelaCliente = new DataTable();
            da.Fill(tabelaCliente);

            conexao.Close();

            return tabelaCliente;
        }
        #endregion
        
        #region METODO CADASTRAR

        public void cadastrar(VeiculoModel obj)
        {
                try
                {

                    string insert = @"CALL criacaoVeiculo(@marca, @modelo, @cor ,@ano, @km, @placa, @obs, @fk_cliente_id)";

                    MySqlCommand executacmdsql = new MySqlCommand(insert, conexao);
                    executacmdsql.Parameters.AddWithValue("@marca", obj.marca);
                    executacmdsql.Parameters.AddWithValue("@modelo", obj.modelo);
                    executacmdsql.Parameters.AddWithValue("@cor", obj.cor_veiculo);
                    executacmdsql.Parameters.AddWithValue("@ano", obj.ano_veiculo);
                    executacmdsql.Parameters.AddWithValue("@km", obj.km_veiculo);
                    executacmdsql.Parameters.AddWithValue("@placa", obj.placa_veiculo);
                    executacmdsql.Parameters.AddWithValue("@obs", obj.obs_veiculo);
                    //executacmdsql.Parameters.AddWithValue("@fk_cliente_id", obj.cliente_fk);
                    executacmdsql.Parameters.AddWithValue("@fk_cliente_id", obj.cliente_fk == null ? (object)DBNull.Value : obj.cliente_fk);

                conexao.Open();
                    executacmdsql.ExecuteNonQuery();
                    conexao.Close();
                MessageBox.Show("Cadastrado com sucesso!");
                }

                catch (Exception erro)
                {
                    MessageBox.Show("Cadastrado não Realizado!" + erro.Message);
                }
            conexao.Close();
        }


        #endregion

        #region METODO ALTERAR
        public void alterar(VeiculoModel obj)
        {
            try
            {

                string update = @"call UpdateVeiculo(@marca,@modelo,@cor ,@ano,@km,@placa,@obs,@fk,@id)";
                MySqlCommand executacmdsql = new MySqlCommand(update, conexao);
                executacmdsql.Parameters.AddWithValue("@id", obj.id);
                executacmdsql.Parameters.AddWithValue("@fk", obj.cliente_fk);
                executacmdsql.Parameters.AddWithValue("@marca", obj.marca);
                executacmdsql.Parameters.AddWithValue("@modelo", obj.modelo);
                executacmdsql.Parameters.AddWithValue("@cor", obj.cor_veiculo);
                executacmdsql.Parameters.AddWithValue("@ano", obj.ano_veiculo);
                executacmdsql.Parameters.AddWithValue("@km", obj.km_veiculo);
                executacmdsql.Parameters.AddWithValue("@placa", obj.placa_veiculo);
                executacmdsql.Parameters.AddWithValue("@obs", obj.obs_veiculo);
                executacmdsql.Parameters.AddWithValue("@fk_cliente_id", obj.cliente_fk);
                conexao.Open();
                executacmdsql.ExecuteNonQuery();
                conexao.Close();

                MessageBox.Show("Alterado com Sucesso!");
            }
            catch (Exception erro)
            {
                MessageBox.Show("Alteração não Realizada!" + erro.Message);

            }
            conexao.Close();

        }
        #endregion

        #region METODO EXCLUIR
        public void Excluir(VeiculoModel obj)
        {
            try
            {

                string update = @"Delete from  tb_veiculo  where id_veiculo=@id";
                MySqlCommand executacmdsql = new MySqlCommand(update, conexao);
                executacmdsql.Parameters.AddWithValue("@id", obj.id);
                
                conexao.Open();
                executacmdsql.ExecuteNonQuery();
                conexao.Close();
            }
            catch (Exception erro)
            {
                if (erro.ToString().Contains("Cannot delete or update"))
                    MessageBox.Show("O veiculo está em uso");
                else MessageBox.Show("erro " + erro);
            }
            conexao.Close();

        }
        #endregion

    }
}
