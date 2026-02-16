class Player
{
    public string Name { get; }
    public bool IsAlive { get; private set; } = true;
    public int Defense { get; } = 0;
    public int MaxHealth { get; } = 100;
    public int RawDamage { get; } = 10;
    public int Gold { get; set; }
    public List<Item> ItemsOwned { get; } = [];

    private int _health = 100;
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
        ItemsOwned.Add(new HealthPotion("Normal Health Potion", this));
        ItemsOwned.Add(new HealthPotion("Normal Health Potion", this));
        ItemsOwned.Add(new HealthPotion("Health Potion (?)", this));
    }

    public void TakeDamage(int amount)
    {
        Health -= (int)(amount * (100.0 / (100 + Defense)));
    }

    public void Heal(int amount)
    {
        Health += amount;
    }

    public void AttackEnemy(Enemy target)
    {
        target.TakeDamage(Random.Shared.Next((int)(RawDamage * _precision), RawDamage));
    }
}
