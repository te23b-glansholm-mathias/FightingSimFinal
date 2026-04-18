class Player : IDamageable  //the player class, which can take damage
{
    //general info
    public string Name { get; private set; }
    public int Level { get; private set; }
    public bool IsAlive { get; private set; } = true;

    //combat stats
    public int Defense { get; private set; } = 0;

    public int MaxHealth => (int)(_baseMaxHealth * MultiplyAll(_maxHealthMultipliers));
    public int RawDamage => (int)(_rawDamage * MultiplyAll(_RawDamageMultipliers));
    public float HealthPotioneffect => _baseHealthPotionEffect * MultiplyAll(_healthPotionEffectMultiplier);

    //inventory
    public List<Item> ItemsOwned { get; } = [];
    public List<Charm> EquippedCharms { get; } = [];
    public List<Weapon> EquippedWeapon { get; private set; } = [];
    public List<Armor> EquippedArmor { get; private set; } = [];

    //load limits
    public int CharmMaxLoad { get; } = 2;
    public int WeaponMaxLoad { get; } = 1;
    public int ArmorMaxLoad { get; } = 1;

    //base stats
    private int _baseMaxHealth = 100;
    private int _rawDamage = 1;
    private int _baseHealthPotionEffect = 1;

    //multipliers
    private List<float> _maxHealthMultipliers = [1];
    private List<float> _RawDamageMultipliers = [1];
    private List<float> _healthPotionEffectMultiplier = [1];

    //backing fields
    private int _health;
    private int _gold;
    private float _precision = 0.6f;

    //events
    public event Action OnHealthUpdate;

    //property logic
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

    public int Gold
    {
        get => _gold;
        private set => _gold = Math.Max(value, 0);
    }

    public Player(string name)
    {
        Name = name;
        _health = MaxHealth; //start at full health

        //start with this weaon and arom
        RawSword stick = new("Stick", this);
        RawArmor woodArmor = new("Wood Armor", this);
        stick.Use();
        woodArmor.Use();

        //starting items
        ItemsOwned.Add(new HealthPotion("Normal Health Potion", this));
        ItemsOwned.Add(new HealthPotion("Normal Health Potion", this));
        ItemsOwned.Add(new HealthPotion("Normal Health Potion", this));
        ItemsOwned.Add(new HealthPotion("Normal Health Potion", this));
    }

    //handles health
    public void Heal(int amount) => Health += amount;
    public void TakeDamage(int amount) => Health -= (int)(amount * (100.0 / (100 + Defense)));

    //handles gold
    public void AddGold(int amount) => Gold += amount;
    public void RemoveGold(int amount) => Gold -= amount;

    // Handles equipping
    public bool TryEquipCharm(Charm charm) => TryEquip(charm, EquippedCharms, CharmMaxLoad);
    public bool TryEquipWeapon(Weapon weapon) => TryEquip(weapon, EquippedWeapon, WeaponMaxLoad);
    public bool TryEquipArmor(Armor armor) => TryEquip(armor, EquippedArmor, ArmorMaxLoad);
    public void UnequipCharm(Charm charm) => Unequip(charm, EquippedCharms);
    public void UnequipWeapon(Weapon weapon) => Unequip(weapon, EquippedWeapon);
    public void UnequipArmor(Armor armor) => Unequip(armor, EquippedArmor);

    private bool TryEquip<T>(T item, List<T> equipped, int maxLoad) where T : Item, IEquipable  //as long as T is a equipable item
    {
        if (equipped.Count >= maxLoad) return false; //if you don't have space

        ItemsOwned.Remove(item);
        equipped.Add(item);
        return true;
    }

    private void Unequip<T>(T item, List<T> equipped) where T : Item, IEquipable //as long as T is a equipable item
    {
        item.RemoveEffect();
        equipped.Remove(item);
        ItemsOwned.Add(item);
    }

    public void Attack(IDamageable target) //handles attacking
    {
        int flatDamage = Random.Shared.Next((int)(RawDamage * _precision), RawDamage);
        target.TakeDamage((int)(flatDamage * (0.1 * (Level - 1) + 1)));
    }

    public void AddRawDamage(int amount) => _rawDamage += amount;
    public void RemoveRawDamage(int amount) => _rawDamage -= amount;
    public void AddRawDamageMultiplier(float multiplier) => _RawDamageMultipliers.Add(1 + multiplier);
    public void RemoveRawDamageMultiplier(float multiplier) => _RawDamageMultipliers.Remove(1 + multiplier);
    public void AddDefense(int amount) => Defense += amount;
    public void RemoveDefense(int amount) => Defense -= amount;
    public void AddHealthPotionMultiplier(float multiplier) => _healthPotionEffectMultiplier.Add(1 + multiplier);
    public void RemoveHealthPotionMultiplier(float multiplier) => _healthPotionEffectMultiplier.Remove(1 + multiplier);

    public void AddMaxHealthMultiplier(float multiplier)
    {
        _maxHealthMultipliers.Add(1 + multiplier);
        UpdateHealth();
    }

    public void RemoveMaxHealthMultiplier(float multiplier)
    {
        _maxHealthMultipliers.Remove(1 + multiplier);
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        Health -= 0;
    }


    private static float MultiplyAll(List<float> values) => values.Aggregate(1f, (acc, m) => acc * m); //calculates the multipliers
}