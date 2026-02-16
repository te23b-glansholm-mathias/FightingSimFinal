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
            choosenItem.Use();
            game.Player.ItemsOwned.Remove(choosenItem);
            AnsiConsole.MarkupLine(choosenItem.UseMessage);
        }
    }
}

abstract class Item(string name, Player player)
{
    protected Player Player = player;
    public string Name { get; } = name;
    protected int Value;

    public string UseMessage { get; protected set; }

    public abstract void Use();
}

class HealthPotion : Item
{
    public HealthPotion(string Name, Player player) : base(Name, player)
    {
        switch (Name)
        {
            case "Minor Health Potion":
                Value = 30;
                break;

            case "Normal Health Potion":
                Value = 50;
                break;

            case "Large Health Potion":
                Value = 120;
                break;

            case "Health Potion (?)":
                Value = Random.Shared.Next(50);
                break;
        }
    }

    public override void Use()
    {
        Player.Heal(Value);
        UseMessage = $"You used a {Name} and healed [Chartreuse2]{Value}[/] HP";
    }
}