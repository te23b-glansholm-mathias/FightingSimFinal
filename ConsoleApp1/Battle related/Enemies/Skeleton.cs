class Skeleton : Enemy, IRespawnable //enemy should be able to respawn after death
{
    private readonly int _originalRawDamage;
    private readonly int _AttackBuffDamage;
    private readonly int _turnsForRespawn;
    private int _turnsRemainingUntilRespawn;
    private int _attackBuffTurnsRemaining = 0;

    public int AttackBuffTurnsRemaining => _attackBuffTurnsRemaining;

    public Skeleton(string name, int level, EnemySpawner enemySpawner) : base(name, level)
    {
        _enemySpawner = enemySpawner;

        switch (Name)
        {
            case "Skeleton":
                _turnsForRespawn = 3;
                _attacksOwned.AddRange(new BoneSlap(), new BoneSmash(this));
                Health = (int)(20 * (0.6 * (Level - 1) + 1));
                _originalRawDamage = (int)(6 * (0.6 * (Level - 1) + 1));
                _AttackBuffDamage = (int)(2 * (0.6 * (Level - 1) + 1));
                break;
        }

        RawDamage = _originalRawDamage;
    }

    public override void OnDeath()
    {
        _turnsRemainingUntilRespawn = _turnsForRespawn;
        if (_enemySpawner.ActiveEnemies.Count == 0 || _enemySpawner.ActiveEnemies.All(e => e.Health <= 0)) base.OnDeath(); //true death if all enemies are deads
    }

    public override void DoAction(Player target)
    {
        if (_enemySpawner.ActiveEnemies.Count == 0 || _enemySpawner.ActiveEnemies.All(e => e.Health <= 0)) base.OnDeath(); //true death if all enemies are dead

        if (IsDead) //if dead, handle respawn countdown
        {
            if (_turnsRemainingUntilRespawn <= 0) //respawn if the timer has run out
            {
                Respawn(target);
            }
            else
            {
                _turnsRemainingUntilRespawn--;
            }
        }
        else //if alive attack as normal
        {
            if (_attackBuffTurnsRemaining > 0)
            {
                RawDamage = _originalRawDamage + _AttackBuffDamage;
                _attackBuffTurnsRemaining--;
            }
            else RawDamage = _originalRawDamage;

            base.DoAction(target);
        }
    }

    public void ActivateDamageBuff() //stackable
    {
        _attackBuffTurnsRemaining += 2;
    }

    public bool IsDead => Health <= 0;

    public void Respawn(Player target)
    {
        _turnsRemainingUntilRespawn = _turnsForRespawn;
        Health = (int)(20 * (0.6 * (Level - 1) + 1));
    }
}