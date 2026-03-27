using System.Drawing;
using System.Windows.Forms;

namespace SimGrav2D
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            menuStripPrincipal = new MenuStrip();
            menuArquivo = new ToolStripMenuItem();
            menuArquivoCarregar = new ToolStripMenuItem();
            menuArquivoSalvar = new ToolStripMenuItem();
            menuArquivoSair = new ToolStripMenuItem();
            menuSimulacao = new ToolStripMenuItem();
            menuSimulacaoIniciar = new ToolStripMenuItem();
            menuSimulacaoPausar = new ToolStripMenuItem();
            menuSimulacaoReiniciar = new ToolStripMenuItem();
            menuExibicao = new ToolStripMenuItem();
            menuExibicaoTrajetorias = new ToolStripMenuItem();
            menuExibicaoVetores = new ToolStripMenuItem();
            panelLateral = new Panel();
            btnCarregar = new Button();
            txtCodigoSimulacao = new TextBox();
            txtMassaMax = new TextBox();
            txtMassaMin = new TextBox();
            label5 = new Label();
            label4 = new Label();
            txtDeltaT = new NumericUpDown();
            label3 = new Label();
            btnReiniciar = new Button();
            label2 = new Label();
            txtQtdIteracoes = new NumericUpDown();
            label1 = new Label();
            txtQtdCorpos = new NumericUpDown();
            btnPausar = new Button();
            btnIniciar = new Button();
            statusPrincipal = new StatusStrip();
            statusInfo = new ToolStripStatusLabel();
            panelSimulacao = new Panel();
            menuStripPrincipal.SuspendLayout();
            panelLateral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtDeltaT).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtQtdIteracoes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtQtdCorpos).BeginInit();
            statusPrincipal.SuspendLayout();
            SuspendLayout();
            // 
            // menuStripPrincipal
            // 
            menuStripPrincipal.BackColor = SystemColors.ControlDark;
            menuStripPrincipal.ImageScalingSize = new Size(20, 20);
            menuStripPrincipal.Items.AddRange(new ToolStripItem[] { menuArquivo, menuSimulacao, menuExibicao });
            menuStripPrincipal.Location = new Point(0, 0);
            menuStripPrincipal.Name = "menuStripPrincipal";
            menuStripPrincipal.Padding = new Padding(7, 3, 0, 3);
            menuStripPrincipal.Size = new Size(914, 30);
            menuStripPrincipal.TabIndex = 0;
            menuStripPrincipal.Text = "menuStrip1";
            // 
            // menuArquivo
            // 
            menuArquivo.DropDownItems.AddRange(new ToolStripItem[] { menuArquivoCarregar, menuArquivoSalvar, menuArquivoSair });
            menuArquivo.Name = "menuArquivo";
            menuArquivo.Size = new Size(75, 24);
            menuArquivo.Text = "Arquivo";
            // 
            // menuArquivoCarregar
            // 
            menuArquivoCarregar.Name = "menuArquivoCarregar";
            menuArquivoCarregar.Size = new Size(181, 26);
            menuArquivoCarregar.Text = "Carregar";
            // 
            // menuArquivoSalvar
            // 
            menuArquivoSalvar.Name = "menuArquivoSalvar";
            menuArquivoSalvar.Size = new Size(181, 26);
            menuArquivoSalvar.Text = "Salvar Estado";
            // 
            // menuArquivoSair
            // 
            menuArquivoSair.Name = "menuArquivoSair";
            menuArquivoSair.Size = new Size(181, 26);
            menuArquivoSair.Text = "Sair";
            // 
            // menuSimulacao
            // 
            menuSimulacao.DropDownItems.AddRange(new ToolStripItem[] { menuSimulacaoIniciar, menuSimulacaoPausar, menuSimulacaoReiniciar });
            menuSimulacao.Name = "menuSimulacao";
            menuSimulacao.Size = new Size(92, 24);
            menuSimulacao.Text = "Simulação";
            // 
            // menuSimulacaoIniciar
            // 
            menuSimulacaoIniciar.Name = "menuSimulacaoIniciar";
            menuSimulacaoIniciar.Size = new Size(149, 26);
            menuSimulacaoIniciar.Text = "Iniciar";
            // 
            // menuSimulacaoPausar
            // 
            menuSimulacaoPausar.Name = "menuSimulacaoPausar";
            menuSimulacaoPausar.Size = new Size(149, 26);
            menuSimulacaoPausar.Text = "Pausar";
            // 
            // menuSimulacaoReiniciar
            // 
            menuSimulacaoReiniciar.Name = "menuSimulacaoReiniciar";
            menuSimulacaoReiniciar.Size = new Size(149, 26);
            menuSimulacaoReiniciar.Text = "Reiniciar";
            // 
            // menuExibicao
            // 
            menuExibicao.DropDownItems.AddRange(new ToolStripItem[] { menuExibicaoTrajetorias, menuExibicaoVetores });
            menuExibicao.Name = "menuExibicao";
            menuExibicao.Size = new Size(79, 24);
            menuExibicao.Text = "Exibição";
            // 
            // menuExibicaoTrajetorias
            // 
            menuExibicaoTrajetorias.Name = "menuExibicaoTrajetorias";
            menuExibicaoTrajetorias.Size = new Size(216, 26);
            menuExibicaoTrajetorias.Text = "Mostrar Trajetórias";
            // 
            // menuExibicaoVetores
            // 
            menuExibicaoVetores.Name = "menuExibicaoVetores";
            menuExibicaoVetores.Size = new Size(216, 26);
            menuExibicaoVetores.Text = "Mostrar Vetores";
            // 
            // panelLateral
            // 
            panelLateral.BackColor = SystemColors.ControlDark;
            panelLateral.Controls.Add(btnCarregar);
            panelLateral.Controls.Add(txtCodigoSimulacao);
            panelLateral.Controls.Add(txtMassaMax);
            panelLateral.Controls.Add(txtMassaMin);
            panelLateral.Controls.Add(label5);
            panelLateral.Controls.Add(label4);
            panelLateral.Controls.Add(txtDeltaT);
            panelLateral.Controls.Add(label3);
            panelLateral.Controls.Add(btnReiniciar);
            panelLateral.Controls.Add(label2);
            panelLateral.Controls.Add(txtQtdIteracoes);
            panelLateral.Controls.Add(label1);
            panelLateral.Controls.Add(txtQtdCorpos);
            panelLateral.Controls.Add(btnPausar);
            panelLateral.Controls.Add(btnIniciar);
            panelLateral.Dock = DockStyle.Left;
            panelLateral.Location = new Point(0, 30);
            panelLateral.Margin = new Padding(3, 4, 3, 4);
            panelLateral.Name = "panelLateral";
            panelLateral.Size = new Size(90, 570);
            panelLateral.TabIndex = 1;
            // 
            // btnCarregar
            // 
            btnCarregar.Location = new Point(1, 525);
            btnCarregar.Margin = new Padding(3, 4, 3, 4);
            btnCarregar.Name = "btnCarregar";
            btnCarregar.Size = new Size(86, 37);
            btnCarregar.TabIndex = 0;
            btnCarregar.Text = "Carregar";
            btnCarregar.UseVisualStyleBackColor = true;
            btnCarregar.Click += btnCarregar_Click;
            // 
            // txtCodigoSimulacao
            // 
            txtCodigoSimulacao.Location = new Point(4, 491);
            txtCodigoSimulacao.Name = "txtCodigoSimulacao";
            txtCodigoSimulacao.Size = new Size(83, 27);
            txtCodigoSimulacao.TabIndex = 17;
            // 
            // txtMassaMax
            // 
            txtMassaMax.Location = new Point(0, 263);
            txtMassaMax.Margin = new Padding(3, 4, 3, 4);
            txtMassaMax.Name = "txtMassaMax";
            txtMassaMax.Size = new Size(83, 27);
            txtMassaMax.TabIndex = 16;
            txtMassaMax.Text = "1e19";
            // 
            // txtMassaMin
            // 
            txtMassaMin.Location = new Point(1, 204);
            txtMassaMin.Margin = new Padding(3, 4, 3, 4);
            txtMassaMin.Name = "txtMassaMin";
            txtMassaMin.Size = new Size(82, 27);
            txtMassaMin.TabIndex = 15;
            txtMassaMin.Text = "1e18";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(14, 180);
            label5.Name = "label5";
            label5.Size = new Size(79, 20);
            label5.TabIndex = 14;
            label5.Text = "Massa Min";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 239);
            label4.Name = "label4";
            label4.Size = new Size(82, 20);
            label4.TabIndex = 13;
            label4.Text = "Massa Max";
            // 
            // txtDeltaT
            // 
            txtDeltaT.Location = new Point(3, 145);
            txtDeltaT.Margin = new Padding(3, 4, 3, 4);
            txtDeltaT.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            txtDeltaT.Name = "txtDeltaT";
            txtDeltaT.Size = new Size(80, 27);
            txtDeltaT.TabIndex = 10;
            txtDeltaT.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 121);
            label3.Name = "label3";
            label3.Size = new Size(57, 20);
            label3.TabIndex = 9;
            label3.Text = "Delta T";
            // 
            // btnReiniciar
            // 
            btnReiniciar.Location = new Point(1, 390);
            btnReiniciar.Margin = new Padding(3, 4, 3, 4);
            btnReiniciar.Name = "btnReiniciar";
            btnReiniciar.Size = new Size(86, 31);
            btnReiniciar.TabIndex = 8;
            btnReiniciar.Text = "Reiniciar";
            btnReiniciar.UseVisualStyleBackColor = true;
            btnReiniciar.Click += btnReiniciar_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(0, 63);
            label2.Name = "label2";
            label2.Size = new Size(106, 20);
            label2.TabIndex = 7;
            label2.Text = "Qtd Interacoes";
            // 
            // txtQtdIteracoes
            // 
            txtQtdIteracoes.Increment = new decimal(new int[] { 10000, 0, 0, 0 });
            txtQtdIteracoes.Location = new Point(3, 87);
            txtQtdIteracoes.Margin = new Padding(3, 4, 3, 4);
            txtQtdIteracoes.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            txtQtdIteracoes.Name = "txtQtdIteracoes";
            txtQtdIteracoes.Size = new Size(80, 27);
            txtQtdIteracoes.TabIndex = 6;
            txtQtdIteracoes.Value = new decimal(new int[] { 100000, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 4);
            label1.Name = "label1";
            label1.Size = new Size(85, 20);
            label1.TabIndex = 5;
            label1.Text = "Qtd Corpos";
            // 
            // txtQtdCorpos
            // 
            txtQtdCorpos.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            txtQtdCorpos.Location = new Point(3, 28);
            txtQtdCorpos.Margin = new Padding(3, 4, 3, 4);
            txtQtdCorpos.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            txtQtdCorpos.Name = "txtQtdCorpos";
            txtQtdCorpos.Size = new Size(80, 27);
            txtQtdCorpos.TabIndex = 4;
            // 
            // btnPausar
            // 
            btnPausar.Location = new Point(1, 351);
            btnPausar.Margin = new Padding(3, 4, 3, 4);
            btnPausar.Name = "btnPausar";
            btnPausar.Size = new Size(86, 31);
            btnPausar.TabIndex = 3;
            btnPausar.Text = "Pausar";
            btnPausar.UseVisualStyleBackColor = true;
            btnPausar.Click += btnPausar_Click;
            // 
            // btnIniciar
            // 
            btnIniciar.Location = new Point(1, 312);
            btnIniciar.Margin = new Padding(3, 4, 3, 4);
            btnIniciar.Name = "btnIniciar";
            btnIniciar.Size = new Size(86, 31);
            btnIniciar.TabIndex = 2;
            btnIniciar.Text = "Iniciar";
            btnIniciar.UseVisualStyleBackColor = true;
            btnIniciar.Click += btnIniciar_Click;
            // 
            // statusPrincipal
            // 
            statusPrincipal.ImageScalingSize = new Size(20, 20);
            statusPrincipal.Items.AddRange(new ToolStripItem[] { statusInfo });
            statusPrincipal.Location = new Point(90, 574);
            statusPrincipal.Name = "statusPrincipal";
            statusPrincipal.Padding = new Padding(1, 0, 16, 0);
            statusPrincipal.Size = new Size(824, 26);
            statusPrincipal.TabIndex = 2;
            statusPrincipal.Text = "statusStrip1";
            // 
            // statusInfo
            // 
            statusInfo.Name = "statusInfo";
            statusInfo.Size = new Size(150, 20);
            statusInfo.Text = "Corpos: 0 | Tempo: 0s";
            // 
            // panelSimulacao
            // 
            panelSimulacao.BackColor = Color.Black;
            panelSimulacao.Dock = DockStyle.Fill;
            panelSimulacao.Location = new Point(90, 30);
            panelSimulacao.Margin = new Padding(3, 4, 3, 4);
            panelSimulacao.Name = "panelSimulacao";
            panelSimulacao.Size = new Size(824, 544);
            panelSimulacao.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 600);
            Controls.Add(panelSimulacao);
            Controls.Add(statusPrincipal);
            Controls.Add(panelLateral);
            Controls.Add(menuStripPrincipal);
            MainMenuStrip = menuStripPrincipal;
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Simulador Gravitacional 2D";
            menuStripPrincipal.ResumeLayout(false);
            menuStripPrincipal.PerformLayout();
            panelLateral.ResumeLayout(false);
            panelLateral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)txtDeltaT).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtQtdIteracoes).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtQtdCorpos).EndInit();
            statusPrincipal.ResumeLayout(false);
            statusPrincipal.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        private MenuStrip menuStripPrincipal;
        private ToolStripMenuItem menuArquivo;
        private ToolStripMenuItem menuArquivoCarregar;
        private ToolStripMenuItem menuArquivoSalvar;
        private ToolStripMenuItem menuArquivoSair;
        private ToolStripMenuItem menuSimulacao;
        private ToolStripMenuItem menuSimulacaoIniciar;
        private ToolStripMenuItem menuSimulacaoPausar;
        private ToolStripMenuItem menuSimulacaoReiniciar;
        private ToolStripMenuItem menuExibicao;
        private ToolStripMenuItem menuExibicaoTrajetorias;
        private ToolStripMenuItem menuExibicaoVetores;
        private Panel panelLateral;
        private Button btnPausar;
        private Button btnIniciar;
        private Button btnCarregar;
        private StatusStrip statusPrincipal;
        private ToolStripStatusLabel statusInfo;
        private Panel panelSimulacao;
        private Label label1;
        private NumericUpDown txtQtdCorpos;
        private Label label2;
        private NumericUpDown txtQtdIteracoes;
        private Button btnReiniciar;
        private Label label3;
        private NumericUpDown txtDeltaT;
        private Label label5;
        private Label label4;
        private TextBox txtMassaMax;
        private TextBox txtMassaMin;
        private TextBox txtCodigoSimulacao;
    }
}