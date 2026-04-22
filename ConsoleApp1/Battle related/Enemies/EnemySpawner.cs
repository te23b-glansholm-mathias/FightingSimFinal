class EnemySpawner
{
    public List<Enemy> ActiveEnemies = []; //currently active enemies 

    public EnemySpawner(string table) //on new instance choose preset
    {
        switch (table)
        {
            case "World_1":
                switch (Random.Shared.Next(5))
                {
                    case 0:
                        ActiveEnemies.Add(new GoblinKing("Goblin King", 1, this));
                        break;

                    case 1:
                        ActiveEnemies.Add(new Slime("Slime (L)", 1, this));
                        break;

                    case 2:
                        ActiveEnemies.Add(new Slime("Slime (M)", 1, this));
                        ActiveEnemies.Add(new Slime("Slime (M)", 1, this));
                        break;

                    case 3:
                        ActiveEnemies.Add(new Goblin("Goblin", 1, this));
                        break;

                    case 4:
                        ActiveEnemies.Add(new Skeleton("Skeleton", 1, this));
                        ActiveEnemies.Add(new Skeleton("Skeleton", 1, this));
                        ActiveEnemies.Add(new Skeleton("Skeleton", 1, this));
                        break;
                }

                break;
        }
    }
}