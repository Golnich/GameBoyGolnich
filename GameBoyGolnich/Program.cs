using GameBoyGolnich.Snake;

IniciarGameBoy();

void IniciarGameBoy()
{

    Console.Clear();
    Console.WriteLine("___BEM VINDO AO GAMEBOY DO GOLNICH___" + "\n");
    Console.WriteLine("Selecione um jogo:");
    Console.WriteLine("1 - Snake" + "\n");
    var jogoSelecionado = Console.ReadLine();
    JogoSelecionado(jogoSelecionado);

    Console.WriteLine("Selecione:" + "\n");
    Console.WriteLine("1 - Jogar novamente");
    Console.WriteLine("2 - Voltar ao menu");
    var opt = Console.ReadLine();
    Console.Clear();
    if (opt == "1")
    {
        JogoSelecionado(jogoSelecionado);

    }
    else
    {
        IniciarGameBoy();
    }
}

void JogoSelecionado(string idJogo)
{
    Cronometro();
    if (idJogo == "1")
    {
        new SnakeService().IniciarJogo();
    }
    else
    {
        IniciarGameBoy();
    }
}

void Cronometro()
{
    for (var i = 4; i > 0; i--)
    {
        Thread.Sleep(1000);
        Console.Clear();
        Console.WriteLine("INICIANDO JOGO EM: " + i);
    }
}