using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace SimGrav2D
{
    public class Corpo
    {
        // Atributos privados
        private string nome;
        private double massa; // quilograma (kg)
        private double densidade; // quilograma por metro cúbico (kg/m³)
        private double posX; // metro (m)
        private double posY; // metro (m)
        private double velX; // metro por segundo (m/s)
        private double velY; // metro por segundo (m/s)
        private double raio; // metro (m)


        // Construtor
        public Corpo(string nome, double massa, double densidade, double posX, double posY, double velX = 0, double velY = 0)
        {
            this.nome = nome;
            this.massa = massa;
            this.densidade = densidade > 0 ? densidade : 1.0;
            this.posX = posX;
            this.posY = posY;
            this.velX = velX;
            this.velY = velY;
            AtualizarRaio();
        }

        // Métodos get e set
        public string Nome
        {
            get => nome;
            set => nome = value;
        }

        public double Massa
        {
            get => massa;
            set
            {
                massa = value;
                AtualizarRaio(); // recalcula o raio quando a massa muda
            }
        }

        public double Densidade
        {
            get => densidade;
            set
            {
                densidade = value > 0 ? value : 1.0; // proteção contra valores inválidos (menor ou igual a zero)
                AtualizarRaio(); // recalcula o raio quando a densidade muda
            }
        }

        public double PosX
        {
            get => posX;
            set => posX = value;
        }

        public double PosY
        {
            get => posY;
            set => posY = value;
        }

        public double VelX
        {
            get => velX;
            set => velX = value;
        }

        public double VelY
        {
            get => velY;
            set => velY = value;
        }

        // Encapsulamento do raio, onde esse é calculado automaticamente com base na massa e densidade, não podendo ser definido diretamente por quem usa a classe.
        public double Raio => raio;

        // Atualiza o raio de acordo com a massa e densidade
        public void AtualizarRaio()
        {
            // Fórmula do Raio (r) de uma esfera, derivada da fórmula da Densidade (d = m/V)
            // r = ³√ ( (3 * massa) / (4 * π * densidade) )
            raio = Math.Pow((3.0 * massa) / (4.0 * Math.PI * densidade), 1.0 / 3.0);
        }

        // Retorna o volume em metros cúbico (m³) assumido da esfera
        public double Volume()
        {
            // V = (4/3) * π * r³
            return (4.0 / 3.0) * Math.PI * Math.Pow(raio, 3);
        }
    }
}