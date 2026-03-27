using System;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;  // Necessário para Parallel.Invoke

namespace SimGrav2D
{
    public class Universo
    {
        private Corpo[] corpos; // Vetor de corpos no universo
        private int capacidadeMaxima;
        private int cont; // Controle do número de corpos
        private const double G = 6.67430e-11; // constante gravitacional

        // Construtor
        public Universo(int capacidade)
        {
            capacidadeMaxima = capacidade;
            corpos = new Corpo[capacidade];
            cont = 0;
        }

        public int Cont => cont;
        public static double ConstanteGravitacional => G;

        // Método para adicionar um corpo ao universo
        public void AdicionarCorpo(Corpo c)
        {
            if (cont < capacidadeMaxima) corpos[cont++] = c;
            else throw new InvalidOperationException("Capacidade máxima atingida.");
        }

        // Método para remover um corpo pelo índice
        public void RemoverCorpo(int index)
        {
            if (index < 0 || index >= cont) throw new ArgumentOutOfRangeException(nameof(index));
            for (int i = index; i < cont - 1; i++) corpos[i] = corpos[i + 1];
            corpos[--cont] = null;
        }

        // Método para retornar uma cópia dos corpos atuais
        public Corpo[] ObterSnapshot()
        {
            Corpo[] snapshot = new Corpo[cont];
            Array.Copy(corpos, snapshot, cont);
            return snapshot;
        }

        // Método para executar a simulação física
        public void Rodar(int iteracoes, double deltaT)
        {
            // Vetores para acumular acelerações de cada corpo (uma por iteração)
            double[] acelX = new double[cont];
            double[] acelY = new double[cont];

            for (int it = 0; it < iteracoes; it++)
            {
                // Parallel.Invoke para zerar as acelerações acumuladas no início de cada iteração de forma paralela
                Parallel.Invoke(
                    () => Array.Clear(acelX, 0, cont),  // Zera o vetor da aceleração X
                    () => Array.Clear(acelY, 0, cont)   // Zera o vetor da aceleração Y
                );

                for (int i = 0; i < cont; i++)
                {
                    for (int j = i + 1; j < cont; j++)
                    {
                        // O var serve para declarar variáveis locais com tipo definido automaticamente pelo compilador
                        var ci = corpos[i];
                        var cj = corpos[j];

                        // Calcula a distância entre os dois corpos
                        double dx = cj.PosX - ci.PosX;
                        double dy = cj.PosY - ci.PosY;
                        double dist2 = dx * dx + dy * dy; // dist² = (x2 - x1)² + (y2 - y1)²
                        double dist = Math.Sqrt(dist2); // dist = √dist² (Math.Sqrt que retorna a raiz quadrada positiva)

                        // Tratamento de colisões
                        if (dist < ci.Raio + cj.Raio)
                        {
                            double massaTotal = ci.Massa + cj.Massa;
                            // Q = m.v
                            // Q = (m1 * v1 + m2 * v2) / (m1 + m2)
                            double novaVX = (ci.VelX * ci.Massa + cj.VelX * cj.Massa) / massaTotal;
                            double novaVY = (ci.VelY * ci.Massa + cj.VelY * cj.Massa) / massaTotal;

                            // Atualiza as propriedades do corpo após a colisão
                            ci.Massa = massaTotal;
                            ci.VelX = novaVX;
                            ci.VelY = novaVY;
                            ci.PosX = (ci.PosX + cj.PosX) / 2;
                            ci.PosY = (ci.PosY + cj.PosY) / 2;
                            ci.Densidade = (ci.Densidade + cj.Densidade) / 2;
                            ci.AtualizarRaio();

                            RemoverCorpo(j);
                            j--;
                            continue;
                        }

                        // Cálculo da força gravitacional
                        // F = G * m1 * m2 / d²
                        double forca = G * ci.Massa * cj.Massa / dist2;

                        // Componentes da força no eixo X e Y (vetor unitário * força)
                        double fx = forca * (dx / dist); // = F * (dx / dist)
                        double fy = forca * (dy / dist); // = F * (dy / dist)

                        // Acumula aceleração em cada corpo
                        // a = F/m
                        // Para ci: aceleração na direção do vetor de j para i
                        acelX[i] += fx / ci.Massa;
                        acelY[i] += fy / ci.Massa;

                        // Para cj: aceleração na direção oposta (ação e reação)
                        acelX[j] -= fx / cj.Massa; 
                        acelY[j] -= fy / cj.Massa;
                    }
                }

                // Atualização paralela de velocidade e posições
                Parallel.For(0, cont, i =>
                {
                    var c = corpos[i];

                    // Atualiza velocidade com aceleração acumulada
                    // v = v0 + a * Δt
                    c.VelX += acelX[i] * deltaT;
                    c.VelY += acelY[i] * deltaT;

                    // Atualiza posição com a nova velocidade
                    // s = s0 + v * Δt
                    c.PosX += c.VelX * deltaT;
                    c.PosY += c.VelY * deltaT;
                });
            }
        }

        // Método para exportar o estado do universo para o formato .txt
        public string ExportarEstado(int iteracoes, double deltaT)
        {
            // StringBuilder é uma classe usada para manipulação eficiente de strings, reduzindo alocações de memória e melhorando desempenho em loops ou grandes manipulações de texto.
            StringBuilder sb = new StringBuilder(); 

            // 1º Cabeçalho das informações sobre os corpos
            sb.AppendLine("<quantidade de corpos>;<quantidade de iterações>;<tempo entre iterações>"); // AppendLine é usado para construir strings de forma eficiente, em cenários que requerem quebras de linha, como geração de texto ou logs.

            // Informações
            sb.AppendLine($"<{cont}>;<{iteracoes}>;<{deltaT.ToString(CultureInfo.InvariantCulture)}>\n");

            // 2º Cabeçalho dos dados dos corpos
            sb.AppendLine("<Nome>;<massa>;<raio>;<PosX>;<PosY>;<VelX>;<VelY>");

            // Linhas dos dados dos corpos
            for (int i = 0; i < cont; i++)
            {
                var c = corpos[i];
                sb.AppendLine($"<{c.Nome}>;<{c.Massa.ToString(CultureInfo.InvariantCulture)}>;<{c.Raio.ToString(CultureInfo.InvariantCulture)}>;" +
                              $"<{c.PosX.ToString(CultureInfo.InvariantCulture)}>;<{c.PosY.ToString(CultureInfo.InvariantCulture)}>;" +
                              $"<{c.VelX.ToString(CultureInfo.InvariantCulture)}>;<{c.VelY.ToString(CultureInfo.InvariantCulture)}>");
            }

            return sb.ToString();
        }

        // Método para importar o estado do universo a partir de um arquivo .txt
        public void ImportarEstado(string conteudo, out int iteracoesCabecalho, out double deltaTCabecalho)
        {
            iteracoesCabecalho = 0;
            deltaTCabecalho = 0;

            var linhas = conteudo.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (linhas.Length < 3) return;

            // Pula a primeira linha que é o cabeçalho, e pega a segunda linha
            var cabecalho = linhas[1].Replace("<", "").Replace(">", "").Split(';');
            int qtdCorpos = int.Parse(cabecalho[0]);
            iteracoesCabecalho = int.Parse(cabecalho[1]);
            deltaTCabecalho = double.Parse(cabecalho[2], CultureInfo.InvariantCulture);

            capacidadeMaxima = qtdCorpos;
            corpos = new Corpo[capacidadeMaxima];
            cont = 0;

            // Começa a iterar no índice 3, que corresponde à quarta linha do arquivo, onde estão os dados do primeiro corpo
            for (int i = 3; i < linhas.Length; i++)
            {
                if (linhas[i].StartsWith("<Nome>")) continue;
                var dados = linhas[i].Replace("<", "").Replace(">", "").Split(';');

                string nome = dados[0];
                double massa = double.Parse(dados[1], CultureInfo.InvariantCulture);
                double raio = double.Parse(dados[2], CultureInfo.InvariantCulture);
                double posX = double.Parse(dados[3], CultureInfo.InvariantCulture);
                double posY = double.Parse(dados[4], CultureInfo.InvariantCulture);
                double velX = double.Parse(dados[5], CultureInfo.InvariantCulture);
                double velY = double.Parse(dados[6], CultureInfo.InvariantCulture);

                var corpo = new Corpo(nome, massa, 1000, posX, posY, velX, velY);
                AdicionarCorpo(corpo);
            }
        }

    }
}              