using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

delegate void Attack(Player target, Enemy sender);

abstract class Enemy(string name)
{
    public string Name { get; } = name;
    public int RawDamage { get; protected set; }
    public int Defense { get; protected set; }

    protected Attacks Attacks = new();
    protected List<Attack> AttacksOwned = [];

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
    private EnemySpawner EnemySpawner;
    private int size;

    public Slime(string name, EnemySpawner EnemySpawner) : base(name)
    {
        switch (Name)
        {
            case "Slime (L)":
                AttacksOwned.AddRange(Attacks.Clash);
                size = 3;
                Health = 100;
                RawDamage = 6;
                Defense = 1;
                break;

            case "Slime (M)":
                AttacksOwned.AddRange(Attacks.Bounce, Attacks.Splash);
                size = 2;
                Health = 10;
                RawDamage = 4;
                Defense = 0;
                break;

            case "Slime (S)":
                AttacksOwned.Add(Attacks.Splash);
                size = 1;
                Health = 10;
                RawDamage = 4;
                Defense = 0;
                break;
        }
    }

    public override void AttackPlayer(Player target)
    {
        AttacksOwned[Random.Shared.Next(0, AttacksOwned.Count)](target, this);
    }

    public override void OnDeath()
    {
        switch (size)
        {
            case 3:
                EnemySpawner.ActiveEnemies.Add(new Slime("Slime (M)", EnemySpawner));
                EnemySpawner.ActiveEnemies.Add(new Slime("Slime (M)", EnemySpawner));
                break;

            case 2:
                EnemySpawner.ActiveEnemies.Add(new Slime("Slime (S)", EnemySpawner));
                EnemySpawner.ActiveEnemies.Add(new Slime("Slime (S)", EnemySpawner));
                EnemySpawner.ActiveEnemies.Add(new Slime("Slime (S)", EnemySpawner));
                break;
        }

        EnemySpawner.ActiveEnemies.Remove(this);
    }
}

class Attacks()
{
    public Attack Clash = (target, sender) =>
    {
        target.TakeDamage(sender.RawDamage * 3);
    };

    public Attack Bounce = (target, sender) =>
    {
        target.TakeDamage((int)(sender.RawDamage * 1.5));
    };

    public Attack Splash = (target, sender) =>
    {
        target.TakeDamage((int)(sender.RawDamage * 0.7));
    };
}
