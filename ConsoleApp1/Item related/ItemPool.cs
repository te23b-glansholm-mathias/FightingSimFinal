class ItemPool(string table, Player player) //handles adding items to the player
{
    public string ItemAdded;

    public void AddItem()
    {
        switch (table)
        {
            case "World_1":
                player.ItemsOwned.Add(new HealthPotion("Normal Health Potion", player));
                ItemAdded = "Normal Health Potion";
                break;
        }
    }
}