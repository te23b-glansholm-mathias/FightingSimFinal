using System.Numerics;

class Inventory(Game game) : GameState()
{
    public override void Update()
    {
        List<string> ItemNames = [.. game.Player.ItemsOwned.OrderBy(i => i.GetType().Name).ThenBy(i => i.Name).Select(i => i.Name)];

        List<string> AppendList = [];
        if (game.Player.EquippedCharms.Count > 0) AppendList.Add("[yellow]Unequip Charm[/]");
        if (game.Player.EquippedWeapon.Count > 0) AppendList.Add("[yellow]Unequip Weapon[/]");
        if (game.Player.EquippedArmor.Count > 0) AppendList.Add("[yellow]Unequip Armor[/]");
        AppendList.Add("[grey]Back[/]");

        string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Inventory").AddChoices(ItemNames).AddChoices(AppendList));
        if (choice == "[yellow]Unequip Charm[/]") UnequipItem(typeof(Charm));
        else if (choice == "[yellow]Unequip Weapon[/]") UnequipItem(typeof(Weapon));
        else if (choice == "[yellow]Unequip Armor[/]") UnequipItem(typeof(Armor));
        else if (choice == "[grey]Back[/]") game.GoBack();
        else
        {
            Item ChosenItem = game.Player.ItemsOwned.Find(i => i.Name == choice);
            if (ChosenItem.Use())
            {
                game.Player.ItemsOwned.Remove(ChosenItem);
                AnsiConsole.MarkupLine(ChosenItem.UseMessage + "\n");
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

        if (item is Weapon)
        {
            foreach (Item i in game.Player.EquippedWeapon)
            {
                EquippedItemNames.Add(i.Name);
            }

            string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title($"Switch {item.Name}?").AddChoices(EquippedItemNames.Append("back")));

            Weapon ChosenWeapon = game.Player.EquippedWeapon.Find(i => i.Name == choice);
            if (ChosenWeapon != null)
            {
                game.Player.UnequipWeapon(ChosenWeapon);
                item.Use();

                AnsiConsole.Clear();
                AnsiConsole.MarkupLine(item.UseMessage);
            }
        }

        if (item is Armor)
        {
            foreach (Item i in game.Player.EquippedArmor)
            {
                EquippedItemNames.Add(i.Name);
            }

            string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title($"Switch {item.Name}?").AddChoices(EquippedItemNames.Append("back")));

            Armor ChosenArmor = game.Player.EquippedArmor.Find(i => i.Name == choice);
            if (ChosenArmor != null)
            {
                game.Player.UnequipArmor(ChosenArmor);
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
                AnsiConsole.MarkupLine($"You unequipped {ChosenCharm.Name}");
            }
        }

        if (type == typeof(Weapon))
        {
            foreach (Item i in game.Player.EquippedWeapon)
            {
                EquippedItemNames.Add(i.Name);
            }

            string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title($"Choose a {type.Name} to unequip").AddChoices(EquippedItemNames.Append("back")));

            Weapon ChosenWeapon = game.Player.EquippedWeapon.Find(i => i.Name == choice);
            if (ChosenWeapon != null)
            {
                game.Player.UnequipWeapon(ChosenWeapon);

                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"You unequipped {ChosenWeapon.Name}");
            }
        }

        if (type == typeof(Armor))
        {
            foreach (Item i in game.Player.EquippedArmor)
            {
                EquippedItemNames.Add(i.Name);
            }

            string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title($"Choose a {type.Name} to unequip").AddChoices(EquippedItemNames.Append("back")));

            Armor ChosenArmor = game.Player.EquippedArmor.Find(i => i.Name == choice);
            if (ChosenArmor != null)
            {
                game.Player.UnequipArmor(ChosenArmor);

                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"You unequipped {ChosenArmor.Name}");
            }
        }
    }
}