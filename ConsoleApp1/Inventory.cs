using System.Reflection.Metadata.Ecma335;

class Inventory(Game game) : GameState()
{
    public override void Update()
    {
        game.Player.ItemsOwned.Add(new Consumable("Minor Health Potion"));
        game.Player.ItemsOwned.Add(new Consumable("Minor Health Potion"));
        game.Player.ItemsOwned.Add(new Consumable("Minor Health Potion"));

        List<string> ItemNames = [];
        foreach (Item i in game.Player.ItemsOwned)
        {
            ItemNames.Add(i.Name);
        }

        string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Inventory").AddChoices(ItemNames));
        game.Player.ItemsOwned.Find(i => i.Name == choice).Use();
    }
}

public abstract class Item(string name)
{
    public string Name { get; } = name;

    public abstract void Use();
}

public class Consumable : Item
{
    public Consumable(string Name) : base(Name)
    {

    }

    public override void Use()
    {
        switch (Name)
        {
            case "Minor Health Potion":
                AnsiConsole.MarkupLine("[green]You used a minor health potion and healed 16 HP [/]");
                Console.ReadKey(true);
                break;
        }
    }
}

public class Charm : Item
{
    public Charm(string Name) : base(Name)
    {

    }

    public override void Use()
    {
        throw new NotImplementedException();
    }
}

public class Weapon : Item
{
    public Weapon(string Name) : base(Name)
    {

    }

    public override void Use()
    {
        throw new NotImplementedException();
    }
}

public class Armor : Item
{
    public Armor(string Name) : base(Name)
    {

    }

    public override void Use()
    {
        throw new NotImplementedException();
    }
}