class Battle(Game game, EnemySpawner enemySpawner) : GameState()
{
    public override void Update()
    {
        StartFight();

        while (enemySpawner.ActiveEnemies.Count > 0 && game.Player.IsAlive == true)
        {
            PlayerTurn();
            EnemyTurn();
        }

        if (game.Player.IsAlive == false) LoseFight();
        else if (enemySpawner.ActiveEnemies.Count > 0) WinFight();
    }

    private void StartFight()
    {
        foreach (Enemy enemy in enemySpawner.ActiveEnemies)
        {
            AnsiConsole.WriteLine($"{enemy.Name} Attacks you!");
        }

        Console.ReadKey(true);
    }

    private void PlayerTurn()
    {
        AnsiConsole.Clear();
        foreach (Enemy enemy in enemySpawner.ActiveEnemies)
        {
            AnsiConsole.WriteLine($"{enemy.Name} HP: {enemy.Health}");
        }

        string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title($"What do you want to do? (HP: {game.Player.Health})").AddChoices("Attack", "Use Item", "Flee"));

        switch (choice)
        {
            case "Attack":
                AttackEnemy();
                break;

            case "Use Item":
                break;

            case "Flee":
                break;
        }

    }

    private void AttackEnemy()
    {
        Enemy enemy = ChooseEnemy();
        int PastHealth = enemy.Health;

        game.Player.AttackEnemy(enemy);
        AnsiConsole.WriteLine($"You dealt {PastHealth - enemy.Health} to {enemy.Name}!");
        Console.ReadKey(true);
    }

    private Enemy ChooseEnemy()
    {
        List<string> enemyNames = [];
        foreach (Enemy enemy in enemySpawner.ActiveEnemies)
        {
            enemyNames.Add(enemy.Name + $" (HP: {enemy.Health})");
        }

        string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title($"Which enemy?").AddChoices(enemyNames));

        return enemySpawner.ActiveEnemies.Find(e => e.Name + $" (HP: {e.Health})" == choice);
    }

    private void EnemyTurn()
    {
        AnsiConsole.Clear();

        foreach (Enemy enemy in enemySpawner.ActiveEnemies)
        {
            AnsiConsole.WriteLine($"{enemy.Name} HP: {enemy.Health}");
        }

        foreach (Enemy enemy in enemySpawner.ActiveEnemies)
        {
            int PastHealth = game.Player.Health;

            enemy.AttackPlayer(game.Player);
            AnsiConsole.WriteLine($"{enemy.Name} attacks you for {PastHealth - game.Player.Health} damage!");
        }

        Console.ReadKey(true);
    }

    private void WinFight()
    {
        AnsiConsole.Clear();
        AnsiConsole.WriteLine("You defeated all of the enemies!");
        Console.ReadKey(true);
        game.ChangeState(new Menu(game));
    }

    private void LoseFight()
    {
        game.GameOver();
    }
}

class EnemySpawner
{
    public List<Enemy> ActiveEnemies = [];

    public EnemySpawner(string table)
    {
        switch (table)
        {
            case "World_1":
                ActiveEnemies.Add(new Slime("Slime (L)", 10, 4, 0, 3, this));
                break;
        }
    }
}