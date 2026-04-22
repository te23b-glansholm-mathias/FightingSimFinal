abstract class Enemy(string name, int level) : IDamageable, IKillable //base class for enemy
{
    protected EnemySpawner _enemySpawner;
    protected List<Attack> _attacksOwned = []; //attacks which the enemy can use
    protected int turn;

    private int _health;

    public string Name { get; } = name;
    public int Level { get; } = level;

    public int RawDamage { get; protected set; }
    public int Defense { get; protected set; } = 0;

    public List<Attack> ActiveAttacks { get; protected set; } = []; //attacks which are currently active

    public int Health
    {
        get => _health;
        set
        {
            _health = Math.Max(value, 0); //cap at 0
            if (_health <= 0) OnDeath();
        }
    }

    public virtual void DoAction(Player target)
    {
        Attack current = _attacksOwned[Random.Shared.Next(0, _attacksOwned.Count)]; //as default choose a random attack

        ActiveAttacks.Add(current);
        current.DoAction(target, this); //perform
        turn++;
    }

    public virtual void OnDeath()
    {
        _enemySpawner.ActiveEnemies.Remove(this); //removes itself on death
    }

    public virtual void TakeDamage(int amount)
    {
        int reducedDamage = (int)(amount * (100.0 / (100 + Defense))); //defense calculations
        Health -= reducedDamage;
    }
}