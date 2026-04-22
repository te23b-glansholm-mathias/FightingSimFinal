class HealthPotion : Item
{
    protected int _healAmount;

    public HealthPotion(string Name, Player player) : base(Name, player)
    {
        switch (Name)   //sets heal amount
        {
            case "Minor Health Potion":
                _healAmount = 30;
                break;

            case "Normal Health Potion":
                _healAmount = 50;
                break;

            case "Large Health Potion":
                _healAmount = 120;
                break;

            case "Health Potion (?)":
                switch (Random.Shared.Next(3))
                {
                    case 0:
                        _healAmount = -100;
                        break;

                    case 1:
                        _healAmount = -50;
                        break;

                    case 2:
                        _healAmount = 50;
                        break;

                    case 3:
                        _healAmount = 100;
                        break;
                }
                break;
        }
    }

    public override bool TryUse()
    {
        if (_healAmount >= 0)
        {
            _healAmount = (int)(_healAmount * Player.HealthPotioneffect); //applies healthpotion buff
            Player.Heal(_healAmount);
            UseMessage = $"You used a {Name} and healed [Chartreuse2]{_healAmount}[/] HP";
        }
        else
        {
            Player.TakeDamage(-_healAmount);
            UseMessage = $"You used a {Name} and Lost [Red]{_healAmount}[/] HP";
        }

        return true;
    }
}