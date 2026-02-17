global using Spectre.Console;
using System.Diagnostics;

Game game = new();

class Game
{
    private Stack<GameState> _states = [];
    public Player Player { get; } = new(AnsiConsole.Ask<string>("What's your name?"));
    public List<Enemy> ActiveEnemies = [];

    public Game()
    {
        PushState(new Menu(this));

        while (true)
        {
            _states.Peek().Update();
            Debug.WriteLine($"Layers: {_states.Count}");
        }
    }

    public void PushState(GameState newState)
    {
        AnsiConsole.Clear();
        _states.Push(newState);
    }

    public void GoBack(int amount = 1)
    {
        AnsiConsole.Clear();
        for (int i = 0; i < amount && _states.Count > 1; i++)
        {
            _states.Pop();
        }
    }

    public void GameOver()
    {
        AnsiConsole.Clear();
        AnsiConsole.WriteLine("You died (loser)!");
        Console.ReadKey(true);
        AnsiConsole.Clear();
        Game game = new();
    }
}

abstract class GameState()
{
    public abstract void Update();
}