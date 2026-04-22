class Inventory(Game game) : GameState() //the inventory state
{
    public override void Update()
    {
        List<string> ItemNames = [.. game.Player.ItemsOwned.OrderBy(i => i.GetType().Name).ThenBy(i => i.Name).Select(i => i.Name)]; //sort by type then name

        //add last options to the list
        List<string> AppendList = [];
        if (game.Player.EquippedCharms.Count > 0) AppendList.Add("[yellow]Unequip Charm[/]");
        if (game.Player.EquippedWeapon.Count > 0) AppendList.Add("[yellow]Unequip Weapon[/]");
        if (game.Player.EquippedArmor.Count > 0) AppendList.Add("[yellow]Unequip Armor[/]");
        AppendList.Add("[grey]Back[/]");

        string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Inventory").AddChoices(ItemNames).AddChoices(AppendList));
        switch (choice)
        {
            case "[yellow]Unequip Charm[/]":
                HandleUnequip(game.Player.EquippedCharms, game.Player.UnequipCharm);
                break;

            case "[yellow]Unequip Weapon[/]":
                HandleUnequip(game.Player.EquippedWeapon, game.Player.UnequipWeapon);
                break;

            case "[yellow]Unequip Armor[/]":
                HandleUnequip(game.Player.EquippedArmor, game.Player.UnequipArmor);
                break;

            case "[grey]Back[/]":
                game.GoBack();
                break;

            default:
                Item ChosenItem = game.Player.ItemsOwned.Find(i => i.Name == choice);
                ChosenItem.TryUse();

                game.Player.ItemsOwned.Remove(ChosenItem);
                AnsiConsole.MarkupLine(ChosenItem.UseMessage + "\n");
                break;
        }
    }

    //choose a equipped item to unequip
    private void HandleUnequip<T>(List<T> equippedItems, Action<T> unequipAction) where T : Item, IEquipable
    {
        AnsiConsole.Clear();
        List<string> EquippedItemNames = [.. equippedItems.OrderBy(i => i.GetType().Name).ThenBy(i => i.Name).Select(i => i.Name)]; //the names of the equipped items
        string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title($"Choose a {typeof(T).Name} to unequip").AddChoices(EquippedItemNames.Append("[grey]Back[/]")));

        T selected = equippedItems.Find(i => i.Name == choice); //gets the selected item

        unequipAction(selected); //uneqips the item
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine($"You unequipped {selected.Name}");
    }
}