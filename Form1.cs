using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace SimGrav2D
{
    public partial class Form1 : Form
    {
        // timer que atualiza a simulação a cada tick
        private System.Windows.Forms.Timer timer;

        // objeto que representa o universo com todos os corpos
        private Universo universo;

        // delta t da simulação
        private double dt = 15;

        // iteração atual e total de iterações
        private int iteracao = 0;
        private int totalIteracoes = 0;

        // massas mínimas e máximas para gerar corpos
        private double massaMin = 1e17;
        private double massaMax = 1e20;

        // conexao com o banco e repositório
        private readonly ConexaoBanco conexaoMySQL = ConexaoBanco.Instancia;
        private RepositorioSim repositorio;

        // código da simulação e interação atual
        private int numSimulacaoAtual = 0;
        private int numInteracaoAtual = 0;

        // controle de pausa
        private bool simulacaoPausada = false;

        // intervalo de plotagem no banco
        private int plotInterval = 20;

        // flags de reprodução
        private bool modoReproducao = false;
        private int frameReproducao = 0;
        private int totalFramesReproducao = 0;
        private Corpo[] frameAtualCorpos;

        // lista de todos os frames carregados para reprodução
        private List<Corpo[]> framesReproducao;

        // construtor principal
        public Form1()
        {
            InitializeComponent();

            // testa conexão com o banco ao abrir
            TestarConexao();

            DoubleBuffered = true; // melhora a renderização do painel

            // inicializa timer
            timer = new System.Windows.Forms.Timer { Interval = 16 };
            timer.Tick += Timer_Tick;

            // eventos do painel
            panelSimulacao.Paint += PanelSimulacao_Paint;
            panelSimulacao.Resize += PanelSimulacao_Resize;

            // botões iniciais
            btnIniciar.Enabled = true;
            btnPausar.Enabled = false;

            // menu eventos
            menuArquivoSalvar.Click += btnSalvar_Click;
            menuArquivoCarregar.Click += btnCarregar_Click;
            menuSimulacaoIniciar.Click += btnIniciar_Click;
            menuSimulacaoPausar.Click += btnPausar_Click;
            menuSimulacaoReiniciar.Click += btnReiniciar_Click;

            // inicializa repositório
            repositorio = new RepositorioSim(conexaoMySQL);
        }

        // testa a conexão
        private void TestarConexao()
        {
            try
            {
                using var conn = conexaoMySQL.CriarConexao();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        // atualiza o painel quando ele muda de tamanho
        private void PanelSimulacao_Resize(object sender, EventArgs e)
        {
            panelSimulacao.Invalidate(); // força redraw
        }

        // inicializa a simulação com nCorpos e maxIteracoes
        // cria corpos aleatórios com massa, posição e densidade
        private void InicializarSimulacao(int nCorpos, int maxIteracoes)
        {
            iteracao = 0;
            totalIteracoes = Math.Max(1, maxIteracoes);

            // cria o universo com nCorpos
            universo = new Universo(Math.Max(1, nCorpos));

            var rnd = new Random();
            for (int i = 0; i < nCorpos; i++)
            {
                double massa = massaMin + rnd.NextDouble() * (massaMax - massaMin);
                double densidade = 3000 + rnd.NextDouble() * 5000;
                double x = rnd.NextDouble() * 25_000_000 - 10_000_000;
                double y = rnd.NextDouble() * 15_000_000 - 10_000_000;
                double vx = 0;
                double vy = 0;

                universo.AdicionarCorpo(new Corpo($"C{i + 1}", massa, densidade, x, y, vx, vy));
            }

            // salva estado inicial no banco
            repositorio.SalvarSimTransaction(numSimulacaoAtual, 0, universo.ObterSnapshot());

            AtualizarStatus();
            panelSimulacao.Invalidate();
        }

        // botão iniciar simulação
        private void btnIniciar_Click(object? sender, EventArgs e)
        {
            // pega valores do formulário
            int nCorpos = (int)txtQtdCorpos.Value;
            int maxIt = (int)txtQtdIteracoes.Value;
            dt = (double)txtDeltaT.Value;

            massaMin = double.Parse(txtMassaMin.Text, NumberStyles.Float, CultureInfo.InvariantCulture);
            massaMax = double.Parse(txtMassaMax.Text, NumberStyles.Float, CultureInfo.InvariantCulture);

            // salva simulação no banco e pega id
            numSimulacaoAtual = repositorio.SalvarSimulacao(DateTime.Now, nCorpos, maxIt, (int)dt);
            numInteracaoAtual = 0;

            InicializarSimulacao(nCorpos, maxIt);

            timer.Start();
            btnIniciar.Enabled = false;
            btnPausar.Enabled = true;
        }

        // botão pausar ou continuar
        private void btnPausar_Click(object? sender, EventArgs e)
        {
            if (simulacaoPausada)
            {
                timer.Start();
                btnPausar.Text = "Pausar";
            }
            else
            {
                timer.Stop();
                btnPausar.Text = "Continuar";
            }

            simulacaoPausada = !simulacaoPausada;
        }

        // função chamada a cada tick do timer
        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (!modoReproducao)
            {
                // simulação normal
                for (int i = 0; i < plotInterval; i++)
                {
                    universo.Rodar(1, dt);
                    iteracao++;
                    numInteracaoAtual++;

                    // salva no banco a cada plotInterval ou no final
                    if (numInteracaoAtual % plotInterval == 0 || iteracao >= totalIteracoes)
                        repositorio.SalvarSimTransaction(numSimulacaoAtual, numInteracaoAtual, universo.ObterSnapshot());
                }

                AtualizarStatus();
                panelSimulacao.Invalidate();

                if (iteracao >= totalIteracoes)
                {
                    timer.Stop();
                    MessageBox.Show("Simulação chegou ao limite!");
                }

                return;
            }

            // reprodução de simulação carregada
            frameAtualCorpos = framesReproducao[frameReproducao];
            frameReproducao++;
            statusInfo.Text = $"Reproduzindo: Frame {frameReproducao}/{totalFramesReproducao}";
            panelSimulacao.Invalidate();

            if (frameReproducao >= totalFramesReproducao)
            {
                timer.Stop();
                modoReproducao = false;
                MessageBox.Show("Fim da reprodução!");
            }
        }

        // desenha o painel com os corpos
        private void PanelSimulacao_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.Black); // fundo preto

            Corpo[] corpos;

            if (modoReproducao)
            {
                if (frameAtualCorpos == null) return;
                corpos = frameAtualCorpos;
            }
            else
            {
                if (universo == null) return;
                corpos = universo.ObterSnapshot();
            }

            if (corpos.Length == 0) return;

            // calcula escala e posição para desenhar corpos
            float W = panelSimulacao.Width;
            float H = panelSimulacao.Height;
            float pad = 20f;

            double minX = double.MaxValue, maxX = double.MinValue;
            double minY = double.MaxValue, maxY = double.MinValue;

            foreach (var c in corpos)
            {
                minX = Math.Min(minX, c.PosX);
                maxX = Math.Max(maxX, c.PosX);
                minY = Math.Min(minY, c.PosY);
                maxY = Math.Max(maxY, c.PosY);
            }

            double escalaX = (W - 2 * pad) / (maxX - minX);
            double escalaY = (H - 2 * pad) / (maxY - minY);
            double escala = Math.Min(escalaX, escalaY);

            foreach (var c in corpos)
            {
                float px = (float)(pad + (c.PosX - minX) * escala);
                float py = (float)(pad + (maxY - c.PosY) * escala);
                float pr = (float)Math.Max(2.0, Math.Min(30.0, c.Raio * escala));

                g.FillEllipse(Brushes.White, px - pr, py - pr, pr * 2, pr * 2);
            }
        }

        // reinicia simulação do zero
        private void ReiniciarSimulacao()
        {
            timer.Stop();

            int nCorpos = (int)txtQtdCorpos.Value;
            int maxIt = (int)txtQtdIteracoes.Value;
            dt = (double)txtDeltaT.Value;

            numSimulacaoAtual = repositorio.SalvarSimulacao(DateTime.Now, nCorpos, maxIt, (int)dt);
            numInteracaoAtual = 0;

            InicializarSimulacao(nCorpos, maxIt);

            btnIniciar.Enabled = true;
            btnPausar.Enabled = false;
        }


        // botão salvar estado atual da simulação - não utilizado
        private void btnSalvar_Click(object? sender, EventArgs e)
        {
            if (universo != null)
            {
                repositorio.SalvarSimTransaction(numSimulacaoAtual, iteracao, universo.ObterSnapshot());
                MessageBox.Show("Estado salvo no banco com sucesso!");
            }
        }

        // atualiza informações de status na tela
        private void AtualizarStatus()
        {
            if (universo != null)
            {
                statusInfo.Text = $"Simulação: {numSimulacaoAtual} | Interação: {numInteracaoAtual}/{totalIteracoes} | Corpos: {universo.Cont}";
            }
            else
            {
                statusInfo.Text = "Nenhuma simulação ativa";
            }
        }

        // botão carregar simulação existente
        private void btnCarregar_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtCodigoSimulacao.Text, out int codigo))
            {
                MessageBox.Show("Código inválido!");
                return;
            }

            numSimulacaoAtual = codigo;

            // carrega todos os frames
            framesReproducao = repositorio.CarregarTodosFrames(codigo);
            totalFramesReproducao = framesReproducao.Count;

            if (totalFramesReproducao == 0)
            {
                MessageBox.Show("Nenhum frame encontrado para essa simulação!");
                return;
            }

            frameReproducao = 0;
            modoReproducao = true;

            frameAtualCorpos = framesReproducao[0];
            statusInfo.Text = $"Reproduzindo simulação {codigo} - Frame 1/{totalFramesReproducao}";

            timer.Start();
            panelSimulacao.Invalidate();
        }

        // botão reiniciar
        private void btnReiniciar_Click(object? sender, EventArgs e) => ReiniciarSimulacao();
    }
}
