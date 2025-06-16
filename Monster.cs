namespace ZeldaTextAdventure;

/// <summary>
/// Rappresenta un mostro nel gioco.
/// </summary>
public class Monster(string name)
{
    public string Name { get; set; } = name;
    public bool IsAlive { get; set; } = true;
}
