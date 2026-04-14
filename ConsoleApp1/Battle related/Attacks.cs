abstract class Attack(string name)
{
    public string Name { get; } = name;
    public string AttackMessage { get; protected set; }

    public abstract void DoAction(Player target, Enemy sender);
}

class Clash : Attack
{
    public Clash() : base("Clash") { }

    public override void DoAction(Player target, Enemy sender)
    {
        AttackMessage = $"{sender.Name} uses [red]{Name}[/]";
        target.TakeDamage((int)(sender.RawDamage * 3 * (0.4 * (sender.Level - 1) + 1)));
    }
}

class Bounce : Attack
{
    public Bounce() : base("Bounce") { }

    public override void DoAction(Player target, Enemy sender)
    {
        AttackMessage = $"{sender.Name} uses [red]{Name}[/]";
        target.TakeDamage((int)(sender.RawDamage * 1.5 * (0.4 * (sender.Level - 1) + 1)));
    }
}

class Slash : Attack
{
    public Slash() : base("Slash") { }

    public override void DoAction(Player target, Enemy sender)
    {
        AttackMessage = $"{sender.Name} uses [red]{Name}[/]";
        target.TakeDamage((int)(sender.RawDamage * 0.8 * (0.4 * (sender.Level - 1) + 1)));
    }
}

class Splash : Attack
{
    public Splash() : base("Splash") { }

    public override void DoAction(Player target, Enemy sender)
    {
        AttackMessage = $"{sender.Name} uses [red]{Name}[/]";
        target.TakeDamage((int)(sender.RawDamage * 0.7 * (0.4 * (sender.Level - 1) + 1)));
    }
}

class BoneSlap : Attack
{
    public BoneSlap() : base("Bone Slap") { }

    public override void DoAction(Player target, Enemy sender)
    {
        AttackMessage = $"{sender.Name} uses [red]{Name}[/]";
        target.TakeDamage((int)(sender.RawDamage * 1.2 * (0.4 * (sender.Level - 1) + 1)));
    }
}

// Specials

class Heal(string name, int amount) : Attack(name)
{
    private int _amount = amount;

    public override void DoAction(Player target, Enemy sender)
    {
        sender.Health += _amount;
    }
}

class SummonEnemy : Attack
{
    private readonly EnemySpawner EnemySpawner;
    private readonly Type Summon = typeof(Enemy);
    private string SummonName;
    private readonly int Amount;

    public SummonEnemy(string preset, EnemySpawner enemySpawner, int amount = 1) : base("Summon Slime")
    {
        Amount = amount;
        EnemySpawner = enemySpawner;

        switch (preset)
        {
            case "Slime (S)":
                SummonName = preset;
                Summon = typeof(Slime);
                break;
        }
    }

    public override void DoAction(Player target, Enemy sender)
    {
        AttackMessage = $"{sender.Name} [yellow]summons[/] {Amount} {SummonName}";

        for (int i = 0; i < Amount; i++)
        {
            Enemy e = (Enemy)Activator.CreateInstance(Summon, [SummonName, sender.Level, EnemySpawner]);
            EnemySpawner.ActiveEnemies.Add(e);
        }
    }
}

class GoldThrow : Attack
{
    private Goblin _goblin;
    public GoldThrow() : base("Gold Throw") { }

    public GoldThrow(Goblin goblin) : base("Gold Throw")
    {
        _goblin = goblin;
    }

    public override void DoAction(Player target, Enemy sender)
    {
        int _damage = (int)(sender.RawDamage * _goblin.Gold * 0.2 * (0.4 * (sender.Level - 1) + 1));
        if (_damage > 0)
        {
            AttackMessage = $"{sender.Name} uses [red]{Name}[/], spends its own gold,";
            target.TakeDamage(_damage);
            _goblin.ClearGold();
        }
        else
        {
            AttackMessage = $"{_goblin.Name} uses {Name}, but, they had no gold";
        }

    }
}

class GoldSteal : Attack
{
    private Goblin _goblin;
    private int _amount;

    public GoldSteal(int amount, Goblin goblin) : base("Gold Throw")
    {
        _amount = amount;
        _goblin = goblin;
    }

    public override void DoAction(Player target, Enemy sender)
    {
        int _goldTaken = Math.Min(target.Gold, _amount);

        if (_goldTaken == 0)
        {
            AttackMessage = $"{sender.Name} tried to steal gold, but, you didn't have any";
        }
        else
        {
            AttackMessage = $"{sender.Name} steals {_amount} gold";
            _goblin.AddGold(_amount);
        }
    }
}

class BoneSmash(Skeleton skeleton) : Attack("Bone Smash")
{
    private Skeleton _skeleton = skeleton;

    public override void DoAction(Player target, Enemy sender)
    {
        _skeleton.ActivateBoneSmashBuff();
        AttackMessage = $"{sender.Name} uses [red]{Name}[/] and buffs themselves!";
    }
}