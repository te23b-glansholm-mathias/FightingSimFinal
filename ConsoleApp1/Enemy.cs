abstract class Enemy(string name)
{
    public string Name { get; } = name;
    public int RawDamage { get; protected set; }
    public int Defense { get; protected set; } = 0;

    protected List<Attack> AttacksOwned = [];
    public Attack ActiveAttack { get; protected set; }

    private int _health;
    public int Health
    {
        get => _health;
        protected set
        {
            _health = value;
            if (_health <= 0) OnDeath();
        }
    }

    public abstract void AttackPlayer(Player target);
    public abstract void OnDeath();

    public virtual void TakeDamage(int Amount)
    {
        int reducedDamage = (int)(Amount * (100.0 / (100 + Defense)));
        Health -= reducedDamage;
    }
}

class Slime : Enemy
{
    private int size;
    private EnemySpawner _enemySpawner;

    public Slime(string name, EnemySpawner enemySpawner) : base(name)
    {
        _enemySpawner = enemySpawner;

        switch (Name)
        {
            case "Slime (L)":
                AttacksOwned.AddRange(new Clash(), new SummonEnemy("Slime (S)", enemySpawner));
                size = 3;
                Health = 100;
                RawDamage = 6;
                break;

            case "Slime (M)":
                AttacksOwned.AddRange(new Bounce(), new Splash());
                size = 2;
                Health = 2;
                RawDamage = 4;
                break;

            case "Slime (S)":
                AttacksOwned.Add(new Splash());
                size = 1;
                Health = 2;
                RawDamage = 4;
                break;
        }
    }

    public override void AttackPlayer(Player target)
    {
        Attack current = AttacksOwned[Random.Shared.Next(0, AttacksOwned.Count)];

        ActiveAttack = current;
        current.DoAction(target, this);
    }

    public override void OnDeath()
    {
        switch (size)
        {
            case 3:
                _enemySpawner.ActiveEnemies.Add(new Slime("Slime (M)", _enemySpawner));
                _enemySpawner.ActiveEnemies.Add(new Slime("Slime (M)", _enemySpawner));
                break;

            case 2:
                _enemySpawner.ActiveEnemies.Add(new Slime("Slime (S)", _enemySpawner));
                _enemySpawner.ActiveEnemies.Add(new Slime("Slime (S)", _enemySpawner));
                _enemySpawner.ActiveEnemies.Add(new Slime("Slime (S)", _enemySpawner));
                break;
        }

        _enemySpawner.ActiveEnemies.Remove(this);
    }
}

class EnemySpawner
{
    public List<Enemy> ActiveEnemies = [];

    public EnemySpawner(string table)
    {
        switch (table)
        {
            case "World_1":
                ActiveEnemies.Add(new Slime("Slime (L)", this));
                break;
        }
    }
}