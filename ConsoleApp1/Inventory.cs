class Inventory(Game game) : GameState()
{
    public override void Update()
    {
        List<string> ItemNames = [.. game.Player.ItemsOwned.Select(i => i.Name)];

        List<string> AppendList = [];
        if (game.Player.EquippedCharms.Count > 0) AppendList.Add("[yellow]Unequip Charm[/]");
        AppendList.Add("[grey]Back[/]");

        string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Inventory").AddChoices(ItemNames).AddChoices(AppendList));
        if (choice == "[yellow]Unequip Charm[/]") UnequipItem(typeof(Charm));
        else if (choice == "[grey]Back[/]") game.GoBack();
        else
        {
            Item ChosenItem = game.Player.ItemsOwned.Find(i => i.Name == choice);
            if (ChosenItem.Use())
            {
                game.Player.ItemsOwned.Remove(ChosenItem);
                AnsiConsole.MarkupLine(ChosenItem.UseMessage);
            }
            else SwitchItem(ChosenItem);
        }
    }

    private void SwitchItem(Item item)
    {
        AnsiConsole.Clear();
        List<string> EquippedItemNames = [];

        if (item is Charm)
        {
            foreach (Item i in game.Player.EquippedCharms)
            {
                EquippedItemNames.Add(i.Name);
            }

            string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title($"Switch {item.Name}?").AddChoices(EquippedItemNames.Append("back")));

            Charm ChosenCharm = game.Player.EquippedCharms.Find(i => i.Name == choice);
            if (ChosenCharm != null)
            {
                game.Player.UnequipCharm(ChosenCharm);
                item.Use();

                AnsiConsole.Clear();
                AnsiConsole.MarkupLine(item.UseMessage);
            }
        }
    }

    private void UnequipItem(Type type)
    {
        AnsiConsole.Clear();
        List<string> EquippedItemNames = [];

        if (type == typeof(Charm))
        {
            foreach (Item i in game.Player.EquippedCharms)
            {
                EquippedItemNames.Add(i.Name);
            }

            string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title($"Choose a {type.Name} to unequip").AddChoices(EquippedItemNames.Append("back")));

            Charm ChosenCharm = game.Player.EquippedCharms.Find(i => i.Name == choice);
            if (ChosenCharm != null)
            {
                game.Player.UnequipCharm(ChosenCharm);

                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"You unequipped {ChosenCharm}");
            }
        }
    }
}