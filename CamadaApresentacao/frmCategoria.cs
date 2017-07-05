﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CamadaNegocio;
using CamadaDados;

namespace CamadaApresentacao
{
    public partial class frmCategoria : Form
    {
        private bool eNovo = false;
        private bool eEditar = false;
        public frmCategoria()
        {
            InitializeComponent();
            this.ttMensagem.SetToolTip(this.txtNome, "Insira o nome da Categoria");
        }

        //Mostrar mensagem de confirmação
        private void MensagemOk(string mensagem)
        {
            MessageBox.Show(mensagem, "Sistema Comercio", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Mostrar mensagem de erro
        private void MensagemErro(string mensagem)
        {
            MessageBox.Show(mensagem, "Sistema Comercio", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Habilitar os text box
        private void Habilitar(bool valor)
        {
            
            this.txtNome.ReadOnly = !valor;
            this.txtDescricao.ReadOnly = !valor;
            this.txtIdCategoria.ReadOnly = !valor;
        }

        //Habilitar os botões
        private void botoes()
        {
            if (this.eNovo || this.eEditar)
            {
                this.Habilitar(true);
                this.btnNovo.Enabled = false;
                this.btnSalvar.Enabled = true;
                this.btnEditar.Enabled = false;
                this.btnCancelar.Enabled = true;
            }
            else
            {
                this.Habilitar(false);
                this.btnNovo.Enabled = true;
                this.btnSalvar.Enabled = false;
                this.btnEditar.Enabled = true;
                this.btnCancelar.Enabled = false;
            }
            
        }

        //ocultar as colunas do grid view
        private void ocultarColunas()
        {
            this.dataLista.Columns[0].Visible = false;
                 

        }

        //mostrar no data grid
        private void Mostrar()
        {
            this.dataLista.DataSource = NCategoria.Mostrar();
            this.ocultarColunas();
            lblTotal.Text = "Total de Registro: " + Convert.ToString(dataLista.Rows.Count);
        }

        //Buscar pelo Nome
        private void BuscarNome()
        {
            this.dataLista.DataSource = NCategoria.BuscarNome(this.txtBuscar.Text);
            this.ocultarColunas();
            lblTotal.Text = "Total de Registro: " + Convert.ToString(dataLista.Rows.Count);
        }

        //Limpar Campos
        private void Limpar()
        {
            this.txtNome.Text = string.Empty;
            this.txtIdCategoria.Text = string.Empty;
            this.txtDescricao.Text = string.Empty;
        }

        private void frmCategoria_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dbcomercioDataSet.Categoria' table. You can move, or remove it, as needed.
            this.categoriaTableAdapter.Fill(this.dbcomercioDataSet.Categoria);
            //this.Top = 0;
            //this.Left = 0;
            this.Mostrar();
            this.Habilitar(false);
            this.botoes();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            this.BuscarNome();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            this.BuscarNome();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            this.eNovo = true;
            this.eEditar = false;
            this.botoes();
            this.Limpar();
            this.Habilitar(true);
            this.txtNome.Focus();
            this.txtIdCategoria.Enabled = false;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (this.txtIdCategoria.Text.Equals(""))
            {
                this.MensagemErro("Selecione um registro para inserir");
            }
            else
            {
                this.eEditar = true;
                this.botoes();
                this.Habilitar(true);
                
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                string resp = "";
                if (txtNome.Text == string.Empty)
                {
                    MensagemErro("Preemcha todos os campos");
                    errorIcone.SetError(txtNome, "Insira o nome");

                }
                else
                {
                    if (this.eNovo)
                    {
                        resp = NCategoria.Inserir(this.txtNome.Text.Trim().ToUpper(), this.txtDescricao.Text.Trim());
                    }
                    else
                    {
                        resp = NCategoria.Editar(Convert.ToInt32(this.txtIdCategoria.Text),
                            this.txtNome.Text.Trim().ToUpper(),
                            this.txtDescricao.Text.Trim());
                    }
                    if (resp.Equals("OK"))
                    {
                        if (this.eNovo)
                        {
                            this.MensagemOk("Registro Salvo com Sucesso");
                        }else
                        {
                            this.MensagemOk("Registro Editado com Sucesso");
                        }
                    }else
                    {
                        this.MensagemErro(resp);
                    }

                    this.eNovo = false;
                    this.eEditar = false;
                    this.botoes();
                    this.Limpar();
                    this.Mostrar();
                }

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void dataLista_DoubleClick(object sender, EventArgs e)
        {
            this.txtIdCategoria.Text = Convert.ToString(this.dataLista.CurrentRow.Cells["idcategoria"].Value);
            this.txtNome.Text = Convert.ToString(this.dataLista.CurrentRow.Cells["nome"].Value);
            this.txtDescricao.Text = Convert.ToString(this.dataLista.CurrentRow.Cells["descricao"].Value);
            this.tabControl1.SelectedIndex = 1;
        }

        private void dataLista_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataLista_DoubleClick_1(object sender, EventArgs e)
        {
            this.txtIdCategoria.Text = Convert.ToString(this.dataLista.CurrentRow.Cells["idcategoria"].Value);
            this.txtNome.Text = Convert.ToString(this.dataLista.CurrentRow.Cells["nome"].Value);
            this.txtDescricao.Text = Convert.ToString(this.dataLista.CurrentRow.Cells["descricao"].Value);
            this.tabControl1.SelectedIndex = 1;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.eNovo = false;
            this.eEditar = false;
            this.botoes();
            this.Habilitar(true);
            this.Limpar();
        }

        private void chkDeletar_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDeletar.Checked)
            {
                this.dataLista.Columns[0].Visible = true;
            }
            else
            {
                this.dataLista.Columns[0].Visible = false;
            }
        }

        private void dataLista_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataLista.Columns["Deletar"].Index)
            {
                DataGridViewCheckBoxCell ChkDeletar = (DataGridViewCheckBoxCell)dataLista.Rows[e.RowIndex].Cells["Deletar"];
                ChkDeletar.Value = !Convert.ToBoolean(ChkDeletar.Value);
            }
        }

        private void btnDeletar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult Opcao;
                Opcao = MessageBox.Show("Realmente Deseja apagar os Registros", "Sistema Comércio", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (Opcao == DialogResult.OK)
                {
                    string Codigo;
                    string Resp = "";

                    foreach (DataGridViewRow row in dataLista.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            Codigo = Convert.ToString(row.Cells[1].Value);
                            Resp = NCategoria.Excluir(Convert.ToInt32(Codigo));

                            if (Resp.Equals("OK"))
                            {
                                this.MensagemOk("Registro Excluido com Sucesso");
                            }
                            else
                            {
                                this.MensagemErro(Resp);
                            }
                        }
                    }

                    this.Mostrar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
    }
}