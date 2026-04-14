interface IUndead
{
    bool IsDead { get; }
    void Respawn(Player target);
}

class Skeleton : Enemy, IUndead
{
    private int _originalRawDamage;
    private int _buffedRawDamage => _originalRawDamage + 2;
    private readonly int _turnsForRespawn = 3;
    private int _turnsRemainingUntilRespawn;
    private int _boneSmashBuffTurnsRemaining = 0;

    public int BoneSmashBuffTurnsRemaining => _boneSmashBuffTurnsRemaining;

    public Skeleton(string name, int level, EnemySpawner enemySpawner) : base(name, level)
    {
        _enemySpawner = enemySpawner;

        switch (Name)
        {
            case "Skeleton":
                _turnsRemainingUntilRespawn = _turnsForRespawn;
                _attacksOwned.AddRange(new BoneSlap(), new BoneSmash(this));
                Health = (int)(20 * (0.6 * (Level - 1) + 1));
                RawDamage = 6;
                break;
        }
    }

    public override void OnDeath()
    {
        if (_enemySpawner.ActiveEnemies.Count == 0 || _enemySpawner.ActiveEnemies.All(e => e.Health <= 0)) base.OnDeath();
    }

    public override void DoAction(Player target)
    {
        if (!IsDead)
        {
            if (_boneSmashBuffTurnsRemaining > 0)
            {

            }

            base.DoAction(target);
            if (_boneSmashBuffTurnsRemaining > 0) _boneSmashBuffTurnsRemaining--;
        }
        else
        {
            _turnsRemainingUntilRespawn--;
            if (_turnsRemainingUntilRespawn <= 0)
            {
                Respawn(target);
            }
        }
    }

    public void ActivateBoneSmashBuff()
    {
        _boneSmashBuffTurnsRemaining = Math.Min(_boneSmashBuffTurnsRemaining + 2, 4);
    }

    public bool IsDead => Health <= 0;

    public void Respawn(Player target)
    {
        _turnsRemainingUntilRespawn = _turnsForRespawn;
        Health = (int)(20 * (0.6 * (Level - 1) + 1));
    }
}