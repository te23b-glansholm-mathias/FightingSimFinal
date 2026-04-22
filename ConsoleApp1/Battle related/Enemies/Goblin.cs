class Goblin : Enemy
{
    public int Gold { get; protected set; } //goblin specific

    public Goblin(string name, int level, EnemySpawner enemySpawner) : base(name, level)
    {
        _enemySpawner = enemySpawner;

        switch (Name)
        {
            case "Goblin":
                _attacksOwned.AddRange(new Slash(), new GoldThrow(this));
                Health = (int)(46 * (0.6 * (Level - 1) + 1));
                RawDamage = 6;
                Gold = 10; 
                break;
        }
    }

    public override void DoAction(Player target)
    {
        int pastHealth = target.Health;
        int pastGoldCount = Gold;
        Attack current = _attacksOwned[Random.Shared.Next(0, _attacksOwned.Count)];

        ActiveAttacks.Add(current);
        current.DoAction(target, this);

        if (pastGoldCount - Gold == 0) //if it didnt use gold on the attack
        {
            int damage = pastHealth - target.Health;
            StealGold(damage, target); //after each attack stealgold depending on damage
        }

        turn++;
    }

    protected void StealGold(int amount, Player target) //steal gold from player
    {
        GoldSteal steal = new(amount, this);
        ActiveAttacks.Add(steal);
        steal.DoAction(target, this);
        target.RemoveGold(amount);
    }

    public void AddGold(int amount) => Gold += amount;
    public void ClearGold() => Gold = 0;

}