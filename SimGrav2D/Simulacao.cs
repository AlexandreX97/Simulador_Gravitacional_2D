using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimGrav2D
{
    public class Simulacao
    {
        public int NumSimulacao { get; set; }
        public string DataSimulacao { get; set; }
        public int QtdCorposInicial { get; set; }
        public int NumInteracoes { get; set; }
        public int TempoInteracoes { get; set; }
        public List<Corpo> Corpos { get; set; }
    }

}