abstract class Attack(string name) //all attacks have a name
{
    public string Name { get; } = name;
    public string AttackMessage { get; protected set; } //message after attack performed

    public abstract void DoAction(Player target, Enemy sender);
}

//basic attacks
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

//special attacks
class Heal(string name, int amount) : Attack(name)
{
    private int _amount = amount;

    public override void DoAction(Player target, Enemy sender)
    {
        sender.Health += _amount; //heal the enemy
    }
}

class SummonEnemy : Attack
{
    private readonly EnemySpawner EnemySpawner;
    private readonly Type Summon = typeof(Enemy);
    private readonly string SummonName;
    private readonly int Amount;

    public SummonEnemy(string preset, EnemySpawner enemySpawner, int amount = 1) : base("Summon Slime") //follows a preset to spawn amount of enemies
    {
        Amount = amount;
        EnemySpawner = enemySpawner;

        switch (preset)
        {
            case "Slime (S)": //if to spawn slimes
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
            Enemy e = (Enemy)Activator.CreateInstance(Summon, [SummonName, sender.Level, EnemySpawner]); //i dont like this part
            EnemySpawner.ActiveEnemies.Add(e);
        }
    }
}

class GoldThrow : Attack
{
    private Goblin _goblin; //goblin only attack
    public GoldThrow() : base("Gold Throw") { }

    public GoldThrow(Goblin goblin) : base("Gold Throw")
    {
        _goblin = goblin;
    }

    public override void DoAction(Player target, Enemy sender)
    {
        int _damage = (int)(sender.RawDamage * _goblin.Gold * 0.2 * (0.4 * (sender.Level - 1) + 1)); //deals damage based on gold
        if (_damage > 0)
        {
            AttackMessage = $"{sender.Name} uses [red]{Name}[/], spends its own gold,";
            target.TakeDamage(_damage);
            _goblin.ClearGold(); //removes gold
        }
        else
        {
            AttackMessage = $"{_goblin.Name} uses {Name}, but, they had no gold";
        }

    }
}

class GoldSteal : Attack //steals amount of gold
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

class BoneSmash(Skeleton skeleton) : Attack("Bone Smash") //buffs skeleton damage
{
    private Skeleton _skeleton = skeleton;

    public override void DoAction(Player target, Enemy sender)
    {
        _skeleton.ActivateDamageBuff();
        AttackMessage = $"{sender.Name} uses [red]{Name}[/] and buffs themselves!";
    }
}

class GreedyStrike(GoblinKing goblinKing) : Attack("Greedy Strike") //deals bonus damage depending greedbonus
{
    private GoblinKing _goblinKing = goblinKing;

    public override void DoAction(Player target, Enemy sender)
    {
        int baseDamage = (int)(sender.RawDamage * 1.5 * (0.4 * (sender.Level - 1) + 1));
        int totalDamage = baseDamage + _goblinKing.GreedBonus;

        AttackMessage = $"{sender.Name} uses [red]{Name}[/]";
        target.TakeDamage(totalDamage);
    }
}