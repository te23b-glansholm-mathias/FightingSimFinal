interface IRespawnable
{
    bool IsDead { get; }
    void Respawn(Player target);
}