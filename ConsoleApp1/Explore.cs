class Explore(Game game) : GameState()
{
    public override void Update()
    {
        switch (Random.Shared.Next(1, 101))
        {
            case <= 33:
                FindItem(new ItemPool("World_1", game.Player));
                game.GoBack();
                break;

            case <= 66:
                FindGold();
                game.GoBack();
                break;

            case <= 100:
                game.PushState(new Battle(game, new EnemySpawner("World_1")));
                break;
        }
    }

    private void FindItem(ItemPool itemPool)
    {
        itemPool.AddItem();
        AnsiConsole.MarkupLine($"You found a {itemPool.ItemAdded}!");
        Console.ReadKey(true);
    }

    private void FindGold()
    {
        int foundGold = Random.Shared.Next(12, 17);
        game.Player.Gold += foundGold;
        AnsiConsole.MarkupLine($"You found [yellow]{foundGold} gold[/]!");
        Console.ReadKey(true);
    }
}