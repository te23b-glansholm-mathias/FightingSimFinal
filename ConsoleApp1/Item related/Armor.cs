abstract class Armor(string name, Player player) : Item(name, player), IEquipable
{
    public override bool Use()
    {
        if (Player.TryEquipArmor(this))
        {
            AddEffect();
            return true;
        }

        return false;
    }

    public abstract void AddEffect();
    public abstract void RemoveEffect();
}

class RawArmor : Armor
{
    readonly int DefenseIncrease = 1;

    public RawArmor(string Name, Player player) : base(Name, player)
    {
        switch (Name)
        {
            case "Wood Armor":
                DefenseIncrease = 2;
                break;

            case "Bronze Armor":
                DefenseIncrease = 8;
                break;
        }
    }

    public override void AddEffect()
    {
        Player.AddDefense(DefenseIncrease);
        UseMessage = $"Your Defense is now +{DefenseIncrease}";
    }

    public override void RemoveEffect()
    {
        Player.RemoveDefense(DefenseIncrease);
    }
}