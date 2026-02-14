class Menu(Game game) : GameState()
{
    public override void Update()
    {
        string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title($"What do you want to do? (Gold: {game.Player.Gold})").AddChoices("Explore", "Inventory", "Exit"));

        switch (choice)
        {
            case "Explore":
                game.ChangeState(new Explore(game));
                break;

            case "Inventory":
                game.ChangeState(new Inventory(game));
                break;

            case "Exit":
                Environment.Exit(0);
                break;
        }
    }
}