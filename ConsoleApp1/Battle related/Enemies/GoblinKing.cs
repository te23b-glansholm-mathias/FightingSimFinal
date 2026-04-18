class GoblinKing : Goblin
{
    private int _originalDefense;
    private int _greedBonus;
    private int _wealthDefenseBonus;
    private const int _goldNeededForGreedBoost = 50;
    private const int _goldNeededForWealthDefense = 20;

    public int GreedBonus => _greedBonus;
    public int WealthDefense => _wealthDefenseBonus;

    public GoblinKing(string name, int level, EnemySpawner enemySpawner) : base(name, level, enemySpawner)
    {
        switch (Name)
        {
            case "Goblin King":
                Health = (int)(80 * (0.6 * (Level - 1) + 1));
                RawDamage = 12;
                Gold = 30;
                _originalDefense = 5;
                _attacksOwned.AddRange(new Slash(), new GreedyStrike(this));
                break;
        }
    }

    public override void DoAction(Player target)
    {
        _greedBonus = Gold / _goldNeededForGreedBoost * 5;
        _wealthDefenseBonus = Gold / _goldNeededForWealthDefense * 2;
        Defense = _originalDefense + _wealthDefenseBonus;

        int pastHealth = target.Health;
        Attack current = _attacksOwned[Random.Shared.Next(0, _attacksOwned.Count)];

        ActiveAttacks.Add(current);
        current.DoAction(target, this);

        int damage = pastHealth - target.Health;
        StealGold(damage, target);
        turn++;
    }
}