class Inventory(Game game) : GameState()
{
    public override void Update()
    {
        List<string> ItemNames = [];
        foreach (Item i in game.Player.ItemsOwned)
        {
            ItemNames.Add(i.Name);
        }

        string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Inventory").AddChoices(ItemNames.Append("back")));
        if (choice == "back") game.GoBack();
        else
        {
            Item choosenItem = game.Player.ItemsOwned.Find(i => i.Name == choice);
            if (choosenItem.Use()) game.Player.ItemsOwned.Remove(choosenItem);
            AnsiConsole.MarkupLine(choosenItem.UseMessage);
        }
    }
}