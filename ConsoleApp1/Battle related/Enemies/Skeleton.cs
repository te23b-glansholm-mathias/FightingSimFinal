class Skeleton : Enemy, IRespawnable
{
    private int _originalRawDamage;
    private int _BoneSmashBuffDamage;
    private int _turnsForRespawn;
    private int _turnsRemainingUntilRespawn;
    private int _boneSmashBuffTurnsRemaining = 0;

    public int BoneSmashBuffTurnsRemaining => _boneSmashBuffTurnsRemaining;

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
                _BoneSmashBuffDamage = (int)(2 * (0.6 * (Level - 1) + 1));
                break;
        }

        RawDamage = _originalRawDamage;
    }

    public override void OnDeath()
    {
        _turnsRemainingUntilRespawn = _turnsForRespawn;
        if (_enemySpawner.ActiveEnemies.Count == 0 || _enemySpawner.ActiveEnemies.All(e => e.Health <= 0)) base.OnDeath();
    }

    public override void DoAction(Player target)
    {
        if (!IsDead)
        {
            if (_boneSmashBuffTurnsRemaining > 0)
            {
                RawDamage = _originalRawDamage + _BoneSmashBuffDamage;
                _boneSmashBuffTurnsRemaining--;
            }
            else RawDamage = _originalRawDamage;

            base.DoAction(target);
        }
        else
        {
            if (_turnsRemainingUntilRespawn <= 0)
            {
                Respawn(target);
            }
            else
            {
                _turnsRemainingUntilRespawn--;
            }
        }
    }

    public void ActivateBoneSmashBuff()
    {
        _boneSmashBuffTurnsRemaining += 2;
    }

    public bool IsDead => Health <= 0;

    public void Respawn(Player target)
    {
        _turnsRemainingUntilRespawn = _turnsForRespawn;
        Health = (int)(20 * (0.6 * (Level - 1) + 1));
    }
}