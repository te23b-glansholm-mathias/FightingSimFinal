abstract class Item(string name, Player player)
{
    protected Player Player = player;
    public string Name { get; } = name;

    public string UseMessage { get; protected set; }

    public abstract bool Use();
}

class HealthPotion : Item
{
    protected int Value;

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
                switch (Random.Shared.Next(3))
                {
                    case 0:
                        Value = -100;
                        break;

                    case 1:
                        Value = -50;
                        break;

                    case 2:
                        Value = 50;
                        break;

                    case 3:
                        Value = 100;
                        break;
                }
                break;
        }
    }

    public override bool Use()
    {
        if (Value > 0) Value = (int)(Value * Player.HealthPotioneffect);

        Player.Heal(Value);
        if (Value >= 0) UseMessage = $"You used a {Name} and healed [Chartreuse2]{Value}[/] HP";
        else UseMessage = $"You used a {Name} and Lost [Red]{Value}[/] HP";
        return true;
    }
}

interface IEquipable
{
    void AddEffect();
    void RemoveEffect();
}

class ItemPool(string table, Player player)
{
    public string ItemAdded;

    public void AddItem()
    {
        switch (table)
        {
            case "World_1":
                player.ItemsOwned.Add(new HealthPotion("Normal Health Potion", player));
                ItemAdded = "Normal Health Potion";
                break;
        }
    }
}