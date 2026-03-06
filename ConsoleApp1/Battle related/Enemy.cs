abstract class Enemy(string name, int level)
{
    public string Name { get; } = name;
    public int Level { get; } = level;
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
    private int _size;
    private EnemySpawner _enemySpawner;

    public Slime(string name, int Level, EnemySpawner enemySpawner) : base(name, Level)
    {
        _enemySpawner = enemySpawner;

        switch (Name)
        {
            case "Slime (L)":
                AttacksOwned.AddRange(new Clash(), new SummonEnemy("Slime (S)", enemySpawner, 1));
                _size = 3;
                Health = (int)(30 * (0.6 * (Level - 1) + 1));
                RawDamage = 6;
                break;

            case "Slime (M)":
                AttacksOwned.AddRange(new Bounce(), new Splash());
                _size = 2;
                Health = (int)(20 * (0.6 * (Level - 1) + 1));
                RawDamage = 4;
                break;

            case "Slime (S)":
                AttacksOwned.Add(new Splash());
                _size = 1;
                Health = (int)(10 * (0.6 * (Level - 1) + 1));
                RawDamage = 2;
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
        switch (_size)
        {
            case 3:
                _enemySpawner.ActiveEnemies.Add(new Slime("Slime (M)", Level, _enemySpawner));
                _enemySpawner.ActiveEnemies.Add(new Slime("Slime (M)", Level, _enemySpawner));
                break;

            case 2:
                _enemySpawner.ActiveEnemies.Add(new Slime("Slime (S)", Level, _enemySpawner));
                _enemySpawner.ActiveEnemies.Add(new Slime("Slime (S)", Level, _enemySpawner));
                _enemySpawner.ActiveEnemies.Add(new Slime("Slime (S)", Level, _enemySpawner));
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
                switch (Random.Shared.Next(2) + 1)
                {
                    case 1:
                        ActiveEnemies.Add(new Slime("Slime (L)", 1, this));
                        break;
                    case 2:
                        ActiveEnemies.Add(new Slime("Slime (M)", 1, this));
                        ActiveEnemies.Add(new Slime("Slime (M)", 1, this));
                        break;
                }

                break;
        }
    }
}