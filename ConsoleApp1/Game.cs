global using Spectre.Console;
using System.Diagnostics;

Game game = new();

class Game
{
    private GameState _currentState;
    public Player Player { get; } = new(AnsiConsole.Ask<string>("What's your name?"));
    public List<Enemy> ActiveEnemies = [];

    public Game()
    {
        ChangeState(new Menu(this));

        while (true) _currentState.Update();
    }

    public void ChangeState(GameState state)
    {
        AnsiConsole.Clear();
        _currentState = state;
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