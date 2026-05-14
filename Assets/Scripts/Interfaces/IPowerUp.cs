public interface IPowerUp
{
    PowerUpType Type { get; }
    void Activate();
    void Deactivate();
    float Duration { get; }
}

