abstract class Weapon(string name, Player player) : Item(name, player), IEquipable
{
    public override bool Use()
    {
        if (Player.TryEquipWeapon(this))
        {
            AddEffect();
            return true;
        }

        return false;
    }

    public abstract void AddEffect();
    public abstract void RemoveEffect();
}

class RawSword : Weapon
{
    readonly int RawDamage = 1;

    public RawSword(string Name, Player player) : base(Name, player)
    {
        switch (Name)
        {
            case "Stick":
                RawDamage = 7;
                break;

            case "Bronze Sword":
                RawDamage = 22;
                break;
        }
    }

    public override void AddEffect()
    {
        Player.AddRawDamage(RawDamage);
        UseMessage = $"Your raw damage is now +{RawDamage}";
    }

    public override void RemoveEffect()
    {
        Player.RemoveRawDamage(RawDamage);
    }
}