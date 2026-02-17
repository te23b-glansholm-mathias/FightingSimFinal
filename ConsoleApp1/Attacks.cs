abstract class Attack
{
    public string Name { get; }
    public string AttackMessage { get; protected set; }

    protected Attack(string name)
    {
        Name = name;
    }

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

class Splash : Attack
{
    public Splash() : base("Splash") { }

    public override void DoAction(Player target, Enemy sender)
    {
        AttackMessage = $"{sender.Name} uses [red]{Name}[/]";
        target.TakeDamage((int)(sender.RawDamage * 0.7 * (0.4 * (sender.Level - 1) + 1)));
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