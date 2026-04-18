global using Spectre.Console; //used globally
using System.Diagnostics; //for debugging

Game session = new();   //creates a new game session

class Game
{
    readonly Stack<GameState> _states = []; //stack for gamestates, making it easier to go back
    public Player Player { get; } = new(AnsiConsole.Ask<string>("What's your name?"));
    public List<Enemy> ActiveEnemies = [];  //the enemies which are loaded

    public Game()
    {
        PushState(new Menu(this)); //starts in the menu

        while (true)
        {
            _states.Peek().Update(); //updates the current state
        }
    }

    public void PushState(GameState newState)   //used in other classes to handle transitions
    {
        AnsiConsole.Clear();
        _states.Push(newState);
    }

    public void GoBack(int amount = 1)  //go back 'amount' of states 
    {
        AnsiConsole.Clear();
        for (int i = 0; i < amount && _states.Count > 1; i++)
        {
            _states.Pop();
        }
    }

    public void GameOver()  //on loss
    {
        AnsiConsole.Clear();
        AnsiConsole.WriteLine("You died (loser)!");
        Console.ReadKey(true);
        AnsiConsole.Clear();
        Game game = new();  //restarts the game
    }
}

abstract class GameState() //creates the gamestate type
{
    public abstract void Update();
}