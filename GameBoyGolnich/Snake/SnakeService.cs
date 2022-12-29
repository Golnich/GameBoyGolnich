using GameBoyGolnich.Snake.Models;

namespace GameBoyGolnich.Snake
{
    public class SnakeService
    {
        public Configuracoes DadosCobra { get; set; } = new Configuracoes();

        public static Direcao Direcao { get; set; }
        Direcao direcaoCobra = Direcao.Direta;
        int placar = 0;
        Random rand = new Random();

        public void IniciarJogo()
        {
            CriarCobre();
            CriarComida();
            LerTeclas();

            //executar determinada ação enquanto o jogo estiver rodando
            while (DadosCobra.jogoRodando)
            {
                Thread.Sleep(30);
                MoverCobra();
                RendenizarCobra();
            }
            FimJogo();
        }

        public void FimJogo()
        {
            Console.WriteLine("FIM DE JOGO \n" + "Total de Pontos: " + placar + "\n");
        }

        public void RendenizarCobra()
        {
            Console.Clear();
            var telaRendenizada = "";

            //Logica para rendenizar cobra e paredes do mapa
            for (var a = 0; DadosCobra.alturaTela > a; a++)
            {
                //parede da esquerda
                telaRendenizada += DadosCobra.parede;
                for (var l = 0; DadosCobra.larguraTela > l; l++)
                {
                    //parede das bordas
                    if (a == 0 && l < DadosCobra.larguraTela || a == DadosCobra.alturaTela - 1 || l == DadosCobra.larguraTela - 1)
                    {
                        telaRendenizada += DadosCobra.parede;
                    }

                    if (!string.IsNullOrWhiteSpace(DadosCobra.tela[l, a]))
                    {
                        telaRendenizada += DadosCobra.tela[l, a];
                    }
                    if (string.IsNullOrWhiteSpace(DadosCobra.tela[l, a]) && a != 0 && a != DadosCobra.alturaTela - 1)
                    {
                        telaRendenizada += " ";
                    }
                }
                //quebrar linha para não desenhar tudo na mesma linha
                telaRendenizada += "\n";
            }
            Console.WriteLine(telaRendenizada);
        }

        public void MoverCobra()
        {
            //separando tamanho da cobra para saber como move-la (ultimo item deve acompanha o pnultimo item)
            var cabecaCobra = DadosCobra.coordenadasCobra[0];
            var raboCobraX = DadosCobra.coordenadasCobra[^1].X;
            var raboCobraY = DadosCobra.coordenadasCobra[^1].Y;

            //Obs: Não precisamos percorrer a posição 0 pois é a cabeça da cobra, a mesma indica a direção
            for (var i = DadosCobra.coordenadasCobra.Count() - 1; i > 0; i--)
            {
                DadosCobra.coordenadasCobra[i].X = DadosCobra.coordenadasCobra[i - 1].X;
                DadosCobra.coordenadasCobra[i].Y = DadosCobra.coordenadasCobra[i - 1].Y;
            }

            //Verificando direção da cabeça e se encostar em alguma parede é fim de jogo
            if (direcaoCobra == Direcao.Direta)
            {
                cabecaCobra.X++;
                if (cabecaCobra.X > DadosCobra.larguraTela - 2)
                {
                    DadosCobra.jogoRodando = false;
                    return;
                }
            }
            if (direcaoCobra == Direcao.Esquerda)
            {
                cabecaCobra.X--;
                if (cabecaCobra.X < 0)
                {
                    DadosCobra.jogoRodando = false;
                    return;
                }
            }

            if (direcaoCobra == Direcao.Cima)
            {
                cabecaCobra.Y--;
                if (cabecaCobra.Y < 1)
                {
                    DadosCobra.jogoRodando = false;
                    return;

                }
            }

            if (direcaoCobra == Direcao.Baixo)
            {
                cabecaCobra.Y++;
                if (cabecaCobra.Y > DadosCobra.alturaTela - 2)
                {
                    DadosCobra.jogoRodando = false;
                    return;
                }
            }

            //Verificando se a cabeça passa por alguma comida
            if (DadosCobra.tela[cabecaCobra.X, cabecaCobra.Y] == "♦")
            {
                //Adicionando ponto no placar
                placar += 1;
                //Gerando mais um caracter para corpo da cobra
                DadosCobra.coordenadasCobra.Add(new Coordenadas(raboCobraX, raboCobraY));
                //Gerando uma nova comida aleatoria
                CriarComida();
            }

            //Verificando se a cobra não bateu no proprio corpo
            if (DadosCobra.tela[cabecaCobra.X, cabecaCobra.Y] == DadosCobra.cobra)
            {
                DadosCobra.jogoRodando = false;
                return;
            }
            AtualizaPosicaoCobra();
        }

        public void LerTeclas()
        {
            Thread task = new(LerClickTeclado);
            task.Start();
        }

        public void LerClickTeclado()
        {
            while (DadosCobra.jogoRodando)
            {
                //Lendo tecla pressionada pelo usuario
                var teclaPressionada = Console.ReadKey();


                //Logica: não sera permitido caso a cobra esteja indo em uma direção e o usuario pressionar a direção oposta 
                if (teclaPressionada.Key == ConsoleKey.UpArrow && direcaoCobra != Direcao.Baixo)
                {
                    direcaoCobra = Direcao.Cima;
                }
                if (teclaPressionada.Key == ConsoleKey.DownArrow && direcaoCobra != Direcao.Cima)
                {
                    direcaoCobra = Direcao.Baixo;
                }
                if (teclaPressionada.Key == ConsoleKey.LeftArrow && direcaoCobra != Direcao.Direta)
                {
                    direcaoCobra = Direcao.Esquerda;
                }
                if (teclaPressionada.Key == ConsoleKey.RightArrow && direcaoCobra != Direcao.Esquerda)
                {
                    direcaoCobra = Direcao.Direta;
                }
            }
        }

        public void CriarComida()
        {
            int aleatorioX, aleatorioY;

            do
            {
                //Soretar numero entre 0 e a largura da tela 
                aleatorioX = rand.Next(1, DadosCobra.larguraTela - 2);

                //Soretar numero entre 0 e a altura da tela 
                aleatorioY = rand.Next(1, DadosCobra.alturaTela - 1);


            } while (!string.IsNullOrWhiteSpace(DadosCobra.tela[aleatorioX, aleatorioY])); //Verifica se a posição para criar a comida não contem a cobra

            DadosCobra.tela[aleatorioX, aleatorioY] = "♦";
        }

        public void CriarCobre()
        {
            DadosCobra.coordenadasCobra.Add(new Coordenadas(9, 14));
            DadosCobra.coordenadasCobra.Add(new Coordenadas(8, 14));
            DadosCobra.coordenadasCobra.Add(new Coordenadas(7, 14));


            AtualizaPosicaoCobra();
        }

        public void AtualizaPosicaoCobra()
        {
            //Lendo todas as posições da tela em X e Y
            for (var l = 0; DadosCobra.larguraTela > l; l++)
            {
                for (var a = 0; DadosCobra.alturaTela > a; a++)
                {
                    //Verificando se na largura contem alguma coordenada X e se na altura contem alguma coordenada Y
                    var posicaoDeveConterCobra = DadosCobra.coordenadasCobra.Any(c => c.X == l && c.Y == a);

                    //Printar nova posição
                    if (posicaoDeveConterCobra)
                    {
                        DadosCobra.tela[l, a] = DadosCobra.cobra;
                        continue;
                    }
                    //Apagar posição antiga
                    if (DadosCobra.tela[l, a] == DadosCobra.cobra)
                    {
                        DadosCobra.tela[l, a] = " ";
                    }
                }
            }
        }
    }
}
