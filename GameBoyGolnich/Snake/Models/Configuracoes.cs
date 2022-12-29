using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyGolnich.Snake.Models
{
    public class Configuracoes
    {

        public Configuracoes()
        {
            alturaTela = 25; 
            larguraTela = 60;
            tela = new string[larguraTela, alturaTela];
            parede =  "@";
            cobra = "o";
            paredes = new string[larguraTela + 1, alturaTela + 1];
        }

        public  int larguraTela { get; set; }
        public  int alturaTela { get; set; } 
        public string cobra { get; set; } 
        public string parede { get; set; } 
        public string[,] tela { get; set; }

        //Array Bidimensional 
        public bool jogoRodando { get; set; } = true;
        public List<Coordenadas> coordenadasCobra { get; set; } = new List<Coordenadas>();
        public string[,] paredes { get; set; }  
    }
}
