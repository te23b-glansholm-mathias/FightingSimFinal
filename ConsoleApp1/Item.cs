using System.Collections;

abstract class Item(string name, Player player)
{
    protected Player Player = player;
    public string Name { get; } = name;
    protected int Value;

    public string UseMessage { get; protected set; }

    public abstract bool Use();
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

    public override bool Use()
    {
        Player.Heal(Value);
        UseMessage = $"You used a {Name} and healed [Chartreuse2]{Value}[/] HP";
        return true;
    }
}

abstract class Charm(string name, Player player) : Item(name, player)
{
    public abstract void Remove();
}

class HealthCharm(string name, Player player) : Charm(name, player)
{
    float HealthPotionBuff = 25;
    float MaxHealthBuff = 0.15f;

    public override bool Use()
    {
        if (Player.TryEquipCharm(this))
        {
            Player.AddMaxHealthMultiplier(MaxHealthBuff);
            UseMessage = $"Increased your max HP with {MaxHealthBuff}%";
            return true;
        }
        else UseMessage = "No";
        return false;
    }

    public override void Remove()
    {
        Player.AddMaxHealthMultiplier(-MaxHealthBuff);
    }
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