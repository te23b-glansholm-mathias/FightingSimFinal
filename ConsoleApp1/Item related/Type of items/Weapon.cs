abstract class Weapon(string name, Player player) : Item(name, player), IEquipable //main weapon class
{
    public override bool TryUse()
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

class RawSword : Weapon //weapon which only increases raw damage
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