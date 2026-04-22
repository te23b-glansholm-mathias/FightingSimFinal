abstract class Charm(string name, Player player) : Item(name, player), IEquipable //main charm class
{
    public override bool TryUse()
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
    private const float _healthPotionBuff = 0.25f; //amount to increase the effect of health potions by
    private const float _maxHealthBuff = 0.15f; //amount to increase max health by

    public override void AddEffect()
    {
        Player.AddMaxHealthMultiplier(_maxHealthBuff);
        Player.AddHealthPotionMultiplier(_healthPotionBuff);
        UseMessage = $"1. Increased your max HP with {(int)(_maxHealthBuff * 100)}%\n2. increased the effect of Health Potions by {(int)(_healthPotionBuff * 100)}%";
    }

    public override void RemoveEffect()
    {
        Player.RemoveMaxHealthMultiplier(_maxHealthBuff);
        Player.RemoveHealthPotionMultiplier(_healthPotionBuff);
    }
}

class AttackCharm(string name, Player player) : Charm(name, player)
{
    private const float _rawAttackBuff = 0.3f; //amount to increase raw attack by

    public override void AddEffect()
    {
        Player.AddRawDamageMultiplier(_rawAttackBuff);
        UseMessage = $"Increased your raw damage with {(int)(_rawAttackBuff * 100)}%";
    }

    public override void RemoveEffect()
    {
        Player.RemoveRawDamageMultiplier(_rawAttackBuff);
    }
}

class BerserkerCharm(string name, Player player) : Charm(name, player)
{
    private const float _missingHealthNeeded = 30f; //amount of health needed to be missing for the buff to increase
    private const float _rawAttackBuff = 0.1f; //amount (%) to increase raw attack by for every "_missingHealthNeeded" missing HP

    private Action _berserkerEffect; //the current effect
    private float _currentBuffAmount = 0; //amount of times the buff is currently applied

    public override void AddEffect()
    {
        _berserkerEffect = () =>
        {
            if (_currentBuffAmount > 0) Player.RemoveRawDamageMultiplier(_currentBuffAmount); //removes old amount

            int steps = (int)((Player.MaxHealth - Player.Health) / _missingHealthNeeded); //how many times to buff
            _currentBuffAmount = steps * _rawAttackBuff;

            if (_currentBuffAmount > 0) Player.AddRawDamageMultiplier(_currentBuffAmount); //adds new amount
        };

        Player.OnHealthUpdate += _berserkerEffect;
        _berserkerEffect();
        UseMessage = $"Increases your raw damage by {(int)(_rawAttackBuff * 100)}% for every {_missingHealthNeeded} missing HP";
    }

    public override void RemoveEffect()
    {
        Player.OnHealthUpdate -= _berserkerEffect; //removes the effect

        if (_currentBuffAmount > 0) Player.RemoveRawDamageMultiplier(_currentBuffAmount);
        _currentBuffAmount = 0;
    }
}
