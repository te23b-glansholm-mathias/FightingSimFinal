enum AttackType
{
    raw,
    summon
}

abstract class Attack
{
    public string Name { get; }
    public AttackType Type { get; }
    public string AttackMessage { get; protected set; }

    protected Attack(string name, AttackType type)
    {
        Name = name;
        Type = type;
    }

    public abstract void DoAction(Player target, Enemy sender);
}

class Clash : Attack
{
    public Clash() : base("Clash", AttackType.raw) { }

    public override void DoAction(Player target, Enemy sender)
    {
        AttackMessage = $"{sender.Name} uses [red]Clash[/]";
        target.TakeDamage(sender.RawDamage * 3);
    }
}

class Bounce : Attack
{
    public Bounce() : base("Bounce", AttackType.raw) { }

    public override void DoAction(Player target, Enemy sender)
    {
        AttackMessage = $"{sender.Name} uses [red]Bounce[/]";
        target.TakeDamage((int)(sender.RawDamage * 1.5));
    }
}

class Splash : Attack
{
    public Splash() : base("Splash", AttackType.raw) { }

    public override void DoAction(Player target, Enemy sender)
    {
        AttackMessage = $"{sender.Name} uses [red]Splash[/]";
        target.TakeDamage((int)(sender.RawDamage * 0.7));
    }
}

class SummonEnemy : Attack
{
    private readonly EnemySpawner EnemySpawner;
    private readonly Type Summon = typeof(Enemy);
    private string SummonName;
    private readonly int Amount;

    public SummonEnemy(string preset, EnemySpawner enemySpawner) : base("Summon Slime", AttackType.summon)
    {
        EnemySpawner = enemySpawner;

        switch (preset)
        {
            case "Slime (S)":
                SummonName = preset;
                Summon = typeof(Slime);
                Amount = 5;
                break;
        }
    }

    public override void DoAction(Player target, Enemy sender)
    {
        AttackMessage = $"{sender.Name} summons {Amount} {SummonName}";

        for (int i = 0; i < Amount; i++)
        {
            Enemy e = (Enemy)Activator.CreateInstance(Summon, [SummonName, EnemySpawner]);
            EnemySpawner.ActiveEnemies.Add(e);
        }
    }
}