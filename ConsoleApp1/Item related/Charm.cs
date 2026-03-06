abstract class Charm(string name, Player player) : Item(name, player), IEquipable
{
    public override bool Use()
    {
        if (Player.TryEquipCharm(this))
        {
            AddEffect();
            return true;
        }
        return false;
    }

    public abstract void AddEffect();
    public abstract void RemoveEffect();
}

class HealthCharm(string name, Player player) : Charm(name, player)
{
    readonly float HealthPotionBuff = 0.25f;
    readonly float MaxHealthBuff = 0.15f;

    public override void AddEffect()
    {
        Player.AddMaxHealthMultiplier(MaxHealthBuff);
        Player.AddHealthPotionMultiplier(HealthPotionBuff);
        UseMessage = $"1. Increased your max HP with {(int)(MaxHealthBuff * 100)}%\n2. increased the effect of Health Potions by {(int)(HealthPotionBuff * 100)}%";
    }

    public override void RemoveEffect()
    {
        Player.RemoveMaxHealthMultiplier(MaxHealthBuff);
        Player.RemoveHealthPotionMultiplier(HealthPotionBuff);
    }
}

class AttackCharm(string name, Player player) : Charm(name, player)
{
    readonly float RawAttackBuff = 0.3f;

    public override void AddEffect()
    {
        Player.AddRawDamageMultiplier(RawAttackBuff);
        UseMessage = $"Increased your raw damage with {(int)(RawAttackBuff * 100)}%";
    }

    public override void RemoveEffect()
    {
        Player.RemoveRawDamageMultiplier(RawAttackBuff);
    }
}

class BerserkerCharm(string name, Player player) : Charm(name, player)
{
    readonly float MissingHealthNeeded = 30f;
    readonly float RawAttackBuff = 0.1f;

    private Action _berserkerEffect;
    private float _currentBuffAmount = 0;

    public override void AddEffect()
    {
        _berserkerEffect = () =>
        {
            if (_currentBuffAmount > 0) Player.RemoveRawDamageMultiplier(_currentBuffAmount);

            int steps = (int)((Player.MaxHealth - Player.Health) / MissingHealthNeeded);
            _currentBuffAmount = steps * RawAttackBuff;

            if (_currentBuffAmount > 0) Player.AddRawDamageMultiplier(_currentBuffAmount);
        };

        Player.OnHealthUpdate += _berserkerEffect;
        _berserkerEffect();
        UseMessage = $"Increases your raw damage by {(int)(RawAttackBuff * 100)}% for every {MissingHealthNeeded} missing HP";
    }

    public override void RemoveEffect()
    {
        Player.OnHealthUpdate -= _berserkerEffect;

        if (_currentBuffAmount > 0) Player.RemoveRawDamageMultiplier(_currentBuffAmount);
        _currentBuffAmount = 0;
    }
}
