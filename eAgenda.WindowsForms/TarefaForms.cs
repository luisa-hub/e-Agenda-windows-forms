using eAgenda.Controladores.Shared;
using eAgenda.Controladores.TarefaModule;
using eAgenda.Dominio.TarefaModule;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eAgenda.WindowsForms
{
    public partial class TarefaForms : Form
    {
        protected ControladorTarefa controlador;
        public TarefaForms(ControladorTarefa controladorTarefa)
        {
            InitializeComponent();
            this.controlador = controladorTarefa;
            PreencherTabelaPendente();
            PreencherTabelaConcluida();
        }



        private void PreencherTabelaPendente()
        {
           dataGridTarefas.Refresh();
           tb_tarefapendente.Clear();

            dataGridTarefas.DataSource = tb_tarefapendente;

            //adicionado as linhas no datagrid

            List<Tarefa> tarefas = controlador.SelecionarTodasTarefasPendentes();

            foreach (var tarefa in tarefas)
            {
                DataRow linha = tb_tarefapendente.NewRow();

                linha["Id"] = tarefa.Id;
                linha["Nome"] = tarefa.Titulo;
                linha["Data Conclusão"] = tarefa.DataConclusao;
                linha["Data Início"] = tarefa.DataCriacao;
                linha["Prioridade"] = tarefa.Prioridade;
                linha["Percentual"] = tarefa.Percentual;

                tb_tarefapendente.Rows.Add(linha);
            }
            


        }

        private void PreencherTabelaConcluida()
        {
            dataGridTarefaConcluidas.Refresh();
            tb_tarefasconcluidas.Clear();
            dataGridTarefaConcluidas.DataSource = tb_tarefasconcluidas;

            //adicionado as linhas no datagrid

            List<Tarefa> tarefas = controlador.SelecionarTodasTarefasConcluidas();

            foreach (var tarefa in tarefas)
            {
                DataRow linha = tb_tarefasconcluidas.NewRow();

                linha["Id"] = tarefa.Id;
                linha["Nome"] = tarefa.Titulo;
                linha["Data Conclusão"] = tarefa.DataConclusao;
                linha["Data Início"] = tarefa.DataCriacao;
                linha["Prioridade"] = tarefa.Prioridade;
                linha["Percentual"] = tarefa.Percentual;

                tb_tarefasconcluidas.Rows.Add(linha);
            }



        }


        private void bt_gravar_Click(object sender, EventArgs e)
        {
            Tarefa tarefa = CriarTarefa();

            string resultadoValidacao = controlador.InserirNovo(tarefa);

            if (resultadoValidacao == "ESTA_VALIDO")
                MessageBox.Show("Sucesso!");
            else
            {
                MessageBox.Show(resultadoValidacao);

            }


            PreencherTabelaPendente();
            limparCampos();
        }

        private Tarefa CriarTarefa()
        {
            string titulo = tb_name.Text;

            int prioridade = cb_prioridadeTarefa.SelectedIndex;

            Dominio.TarefaModule.Tarefa tarefa = new Dominio.TarefaModule.Tarefa(titulo, DateTime.Now.Date, (PrioridadeEnum)prioridade);
            return tarefa;
        }


        private void limparCampos()
        {

            tb_name.Text = "";
            cb_prioridadeTarefa.SelectedItem = null;
        }

        private void bt_excluir_Click(object sender, EventArgs e)
        {

            if (dataGridTarefas.RowCount == 0)
                return;


            int id = Convert.ToInt32(dataGridTarefas.CurrentRow.Cells["Id"].Value);

            bool numeroEncontrado = controlador.Existe(id);

            if (numeroEncontrado == false)            {

                controlador.Excluir(id);
                return;
            }

            bool conseguiuExcluir = controlador.Excluir(id);

            if (conseguiuExcluir)
            {
                MessageBox.Show("Sucesso");
                
            }


            else
            {
                MessageBox.Show("Erro");

            }

            PreencherTabelaPendente();
        }

        private void bt_excluirConcluida_Click(object sender, EventArgs e)
        {
            if (dataGridTarefaConcluidas.RowCount == 0)
                return;


            int id = Convert.ToInt32(dataGridTarefaConcluidas.CurrentRow.Cells["Id"].Value);

            bool numeroEncontrado = controlador.Existe(id);

            if (numeroEncontrado == false)
            {

                controlador.Excluir(id);
                return;
            }

            bool conseguiuExcluir = controlador.Excluir(id);

            if (conseguiuExcluir)
            {
                MessageBox.Show("Sucesso");

            }


            else
            {
                MessageBox.Show("Erro");

            }

            PreencherTabelaConcluida();
        }

        private void bt_atualizar_Click(object sender, EventArgs e)
        {
            int id = 0;
            if (dataGridTarefaConcluidas.SelectedRows.Count == 1)
            {
                id = Convert.ToInt32(dataGridTarefas.CurrentRow.Cells["Id"].Value);
            }

            else if (dataGridTarefas.SelectedRows.Count == 1)
                id = Convert.ToInt32(dataGridTarefas.CurrentRow.Cells["Id"].Value);

            else
                return;

            bool numeroEncontrado = controlador.Existe(id);

            if (numeroEncontrado == false)
            {
                MessageBox.Show("Erro!");

                return;
            }

            
            int novoPercentual = Convert.ToInt32(tb_porcentagem.Text);

            controlador.AtualizarPercentual(id, novoPercentual);

            MessageBox.Show("Sucesso!");

            PreencherTabelaPendente();
            PreencherTabelaConcluida();
        }

        private void bt_editarPendente_Click(object sender, EventArgs e)
        {
            if (dataGridTarefas == null)
                return;


            int id = Convert.ToInt32(dataGridTarefas.CurrentRow.Cells["Id"].Value);

            bool numeroEncontrado = controlador.Existe(id);
            if (numeroEncontrado == false)
            {
                MessageBox.Show("Erro!");

                return;
            }

            Tarefa tarefa = CriarTarefa();


            string resultadoValidacao = controlador.Editar(id, tarefa);

            if (resultadoValidacao == "ESTA_VALIDO")
                MessageBox.Show("Sucesso!");
            else
            {
                MessageBox.Show(resultadoValidacao);

            }

            PreencherTabelaPendente();
            limparCampos();
        }

       
        private void bt_editarConcluida_Click(object sender, EventArgs e)
        {
            if (dataGridTarefaConcluidas == null)
                return;


            int id = Convert.ToInt32(dataGridTarefaConcluidas.CurrentRow.Cells["Id"].Value);

            bool numeroEncontrado = controlador.Existe(id);
            if (numeroEncontrado == false)
            {
                MessageBox.Show("Erro!");

                return;
            }

            Tarefa tarefa = CriarTarefa();


            string resultadoValidacao = controlador.Editar(id, tarefa);

            if (resultadoValidacao == "ESTA_VALIDO")
                MessageBox.Show("Sucesso!");
            else
            {
                MessageBox.Show(resultadoValidacao);
                return;
            }

            PreencherTabelaConcluida();
            limparCampos();
        }
    }
}
