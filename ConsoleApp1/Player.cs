class Player
{
    public string Name { get; }
    public int Level { get; }
    public bool IsAlive { get; private set; } = true;
    public int Defense { get; private set; } = 0;
    public int MaxHealth => (int)(_baseMaxHealth * _maxHealthMultipliers.Aggregate(1f, (acc, m) => acc * m));
    public int RawDamage => (int)(_RawDamage * _RawDamageMultipliers.Aggregate(1f, (acc, m) => acc * m));
    public float HealthPotioneffect => _baseHealthPotionEffect * _healthPotionEffectMultiplier.Aggregate(1f, (acc, m) => acc * m);
    public int Gold { get; private set; }
    public List<Item> ItemsOwned { get; } = [];
    public List<Charm> EquippedCharms { get; } = [];
    public List<Weapon> EquippedWeapon { get; private set; } = [];
    public List<Armor> EquippedArmor { get; private set; } = [];
    public int CharmMaxLoad { get; } = 2;
    public int WeaponMaxLoad { get; } = 1;
    public int ArmorMaxLoad { get; } = 1;

    private int _baseMaxHealth = 100;
    private int _RawDamage = 1;
    private int _baseHealthPotionEffect = 1;
    private List<float> _maxHealthMultipliers = [1];
    private List<float> _RawDamageMultipliers = [1];
    private List<float> _healthPotionEffectMultiplier = [1];
    private int _health;
    private float _precision = 0.6f;

    public event Action OnHealthUpdate;

    public int Health
    {
        get => _health;
        private set
        {
            _health = Math.Clamp(value, 0, MaxHealth);
            OnHealthUpdate?.Invoke();
            if (_health == 0) IsAlive = false;
        }
    }

    public Player(string name)
    {
        Name = name;
        _health = MaxHealth;

        RawSword stick = new("Stick", this);
        stick.Use();

        ItemsOwned.Add(new HealthPotion("Normal Health Potion", this));
        ItemsOwned.Add(new RawArmor("Bronze Armor", this));
        ItemsOwned.Add(new RawArmor("Bronze Armor", this));
        ItemsOwned.Add(new RawArmor("Bronze Armor", this));
    }

    public void TakeDamage(int amount)
    {
        Health -= (int)(amount * (100.0 / (100 + Defense)));
    }

    public void Heal(int amount)
    {
        Health += amount;
    }

    public bool TryEquipCharm(Charm charm)
    {
        if (EquippedCharms.Count < CharmMaxLoad)
        {
            ItemsOwned.Remove(charm);
            EquippedCharms.Add(charm);
            return true;
        }
        else return false;
    }

    public void UnequipCharm(Charm charm)
    {
        charm.RemoveEffect();
        EquippedCharms.Remove(charm);
        ItemsOwned.Add(charm);
    }

    public bool TryEquipWeapon(Weapon weapon)
    {
        if (EquippedWeapon.Count < WeaponMaxLoad)
        {
            ItemsOwned.Remove(weapon);
            EquippedWeapon.Add(weapon);
            return true;
        }
        else return false;
    }

    public void UnequipWeapon(Weapon weapon)
    {
        weapon.RemoveEffect();
        EquippedWeapon.Remove(weapon);
        ItemsOwned.Add(weapon);
    }

    public bool TryEquipArmor(Armor armor)
    {
        if (EquippedArmor.Count < ArmorMaxLoad)
        {
            ItemsOwned.Remove(armor);
            EquippedArmor.Add(armor);
            return true;
        }
        else return false;
    }

    public void UnequipArmor(Armor armor)
    {
        armor.RemoveEffect();
        EquippedArmor.Remove(armor);
        ItemsOwned.Add(armor);
    }

    public void AttackEnemy(Enemy target)
    {
        int flatDamage = Random.Shared.Next((int)(RawDamage * _precision), RawDamage);
        target.TakeDamage((int)(flatDamage * (0.1 * (Level - 1) + 1)));
    }

    public void AddGold(int amount)
    {
        Gold += amount;
    }

    public void AddRawDamage(int amount)
    {
        _RawDamage += amount;
    }
    public void AddDefense(int amount)
    {
        Defense += amount;
    }

    public void AddMaxHealthMultiplier(float multiplier)
    {
        _maxHealthMultipliers.Add(1 + multiplier);
        Health = Health;
    }

    public void RemoveMaxHealthMultiplier(float multiplier)
    {
        _maxHealthMultipliers.Remove(1 + multiplier);
        Health = Health;
    }

    public void AddHealthPotionMultiplier(float multiplier)
    {
        _healthPotionEffectMultiplier.Add(1 + multiplier);
    }

    public void RemoveHealthPotionMultiplier(float multiplier)
    {
        _healthPotionEffectMultiplier.Remove(1 + multiplier);
    }

    public void AddRawDamageMultiplier(float multiplier)
    {
        _RawDamageMultipliers.Add(1 + multiplier);
    }

    public void RemoveRawDamageMultiplier(float multiplier)
    {
        _RawDamageMultipliers.Remove(1 + multiplier);
    }
}
