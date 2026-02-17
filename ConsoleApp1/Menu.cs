class Menu(Game game) : GameState()
{
    public override void Update()
    {
        string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title($"{game.Player.Name}: ([DeepSkyBlue1]Health: {game.Player.Health} HP[/]   -   [gold1]Gold: {game.Player.Gold}[/])").AddChoices("Explore", "Inventory", "Exit"));

        switch (choice)
        {
            case "Explore":
                game.PushState(new Explore(game));
                break;

            case "Inventory":
                game.PushState(new Inventory(game));
                break;

            case "Exit":
                Environment.Exit(0);
                break;
        }
    }
}