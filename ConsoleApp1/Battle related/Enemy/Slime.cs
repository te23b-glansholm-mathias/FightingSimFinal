class Slime : Enemy
{
    private int _size;

    public Slime(string name, int Level, EnemySpawner enemySpawner) : base(name, Level)
    {
        _enemySpawner = enemySpawner;

        switch (Name)
        {
            case "Slime (L)":
                _attacksOwned.AddRange(new Clash(), new SummonEnemy("Slime (S)", enemySpawner, 1));
                _size = 3;
                Health = (int)(30 * (0.6 * (Level - 1) + 1));
                RawDamage = 6;
                break;

            case "Slime (M)":
                _attacksOwned.AddRange(new Bounce(), new Splash());
                _size = 2;
                Health = (int)(20 * (0.6 * (Level - 1) + 1));
                RawDamage = 4;
                break;

            case "Slime (S)":
                _attacksOwned.Add(new Splash());
                _size = 1;
                Health = (int)(10 * (0.6 * (Level - 1) + 1));
                RawDamage = 2;
                break;
        }
    }

    public override void OnDeath()
    {
        switch (_size)
        {
            case 3:
                _enemySpawner.ActiveEnemies.Add(new Slime("Slime (M)", Level, _enemySpawner));
                _enemySpawner.ActiveEnemies.Add(new Slime("Slime (M)", Level, _enemySpawner));
                break;

            case 2:
                _enemySpawner.ActiveEnemies.Add(new Slime("Slime (S)", Level, _enemySpawner));
                _enemySpawner.ActiveEnemies.Add(new Slime("Slime (S)", Level, _enemySpawner));
                _enemySpawner.ActiveEnemies.Add(new Slime("Slime (S)", Level, _enemySpawner));
                break;
        }

        _enemySpawner.ActiveEnemies.Remove(this);
    }
}