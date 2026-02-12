abstract class Enemy(string name, int health, int rawDamage, int defense)
{
    public string Name { get; } = name;
    public int RawDamage { get; } = rawDamage;
    public int Defense { get; } = defense;

    private int _health = health;

    public int Health
    {
        get => _health;
        private set
        {
            _health -= value;
            if (_health <= 0) OnDeath();
        }
    }

    public abstract void AttackPlayer(Player target);
    public abstract void OnDeath();

    public virtual void TakeDamage(int Amount)
    {
        Health = (int)(Amount * (100.0 / (100 + Defense)));
    }
}

class Slime(string name, int health, int rawDamage, int defense, int size, EnemySpawner enemySpawner) : Enemy(name, health, rawDamage, defense)
{
    public override void AttackPlayer(Player target)
    {
        target.TakeDamage(RawDamage);
    }

    public override void OnDeath()
    {
        switch (size)
        {
            case 3:
                enemySpawner.ActiveEnemies.Add(new Slime("Slime (M)", 10, 4, 0, 2, enemySpawner));
                enemySpawner.ActiveEnemies.Add(new Slime("Slime (M)", 10, 4, 0, 2, enemySpawner));
                break;
            case 2:
                enemySpawner.ActiveEnemies.Add(new Slime("Slime (S)", 10, 4, 0, 1, enemySpawner));
                enemySpawner.ActiveEnemies.Add(new Slime("Slime (S)", 10, 4, 0, 1, enemySpawner));
                enemySpawner.ActiveEnemies.Add(new Slime("Slime (S)", 10, 4, 0, 1, enemySpawner));
                break;
        }

        enemySpawner.ActiveEnemies.Remove(this);
    }
}