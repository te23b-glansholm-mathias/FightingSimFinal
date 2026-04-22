abstract class Item(string name, Player player)
{
    protected Player Player = player; //need a reference for effects
    public string Name { get; private set; } = name;

    public string UseMessage { get; protected set; } //message that should be displayed after using the item

    public abstract bool TryUse();
}