using System.Diagnostics;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
class Player
{
    public string Name { get; }
    public int Level { get; }
    public bool IsAlive { get; private set; } = true;
    public int Defense { get; } = 0;
    public int MaxHealth => (int)(_baseMaxHealth * _maxHealthMultiplier);
    public int RawDamage { get; } = 10;
    public int Gold { get; set; }
    public List<Item> ItemsOwned { get; } = [];
    public List<Charm> EquipedCharms { get; } = [];
    public int CharmMaxLoad { get; } = 1;

    private int _baseMaxHealth = 100;
    private float _maxHealthMultiplier = 1;
    private int _health;
    private float _precision = 0.6f;

    public int Health
    {
        get => _health;
        private set
        {
            _health = Math.Clamp(value, 0, MaxHealth);
            if (_health == 0) IsAlive = false;
        }
    }

    public Player(string name)
    {
        Name = name;
        _health = MaxHealth;

        ItemsOwned.Add(new HealthPotion("Normal Health Potion", this));
        ItemsOwned.Add(new HealthPotion("Normal Health Potion", this));
        ItemsOwned.Add(new HealthPotion("Health Potion (?)", this));
        ItemsOwned.Add(new HealthCharm("Health Charm", this));
        ItemsOwned.Add(new HealthCharm("Health Charm", this));
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
        if (EquipedCharms.Count < CharmMaxLoad)
        {
            EquipedCharms.Add(charm);
            return true;
        }
        else return false;
    }

    public void AttackEnemy(Enemy target)
    {
        int flatDamage = Random.Shared.Next((int)(RawDamage * _precision), RawDamage);
        target.TakeDamage((int)(flatDamage * (0.1 * (Level - 1) + 1)));
    }

    public void AddMaxHealthMultiplier(float amount)
    {
        _maxHealthMultiplier += amount;
        Health = Health;
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}
