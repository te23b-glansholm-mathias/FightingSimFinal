class Explore(Game game) : GameState()
{
    public override void Update()
    {
        switch (Random.Shared.Next(1, 101))
        {
            case <= 33:
                FindItem();
                break;

            case <= 66:
                FindGold();
                game.ChangeState(new Menu(game));
                break;

            case <= 100:
                game.ChangeState(new Battle(game, new EnemySpawner("World_1")));
                break;
        }
    }

    private void FindItem() { }
    private void FindGold()
    {
        int foundGold = Random.Shared.Next(12, 17);
        game.Player.Gold += foundGold;
        AnsiConsole.MarkupLine($"You found [yellow]{foundGold} gold[/]!");
        Console.ReadKey(true);
    }
}