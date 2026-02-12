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
    Attacks attacks = new();

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

        switch (Random.Shared.Next(1, 100))
        {
            case <= 33:
                break;

            case <= 66:
                break;

            case <= 100:
                break;
        }
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

class Attacks()
{
    public delegate void Attack(Player target, Enemy sender);

    public void Clash(Player target, Enemy sender)
    {
        target.TakeDamage(sender.RawDamage * 3);
    }

    public void Bounce(Player target, Enemy sender)
    {
        target.TakeDamage((int)(sender.RawDamage * 1.5));
    }

    public void Splash(Player target, Enemy sender)
    {
        target.TakeDamage((int)(sender.RawDamage * 0.7));
    }
}