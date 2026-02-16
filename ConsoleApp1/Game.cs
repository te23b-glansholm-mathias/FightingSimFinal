global using Spectre.Console;

Game game = new();

class Game
{
    private GameState _currentState;
    private GameState _lastState;
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
        _lastState = _currentState;
        _currentState = state;
    }

    public void GoBack()
    {
        ChangeState(_lastState);
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