namespace ZeldaTextAdventure.Models;

/// <summary>
/// Rappresenta un mostro nel gioco.
/// </summary>
public class Monster(int id, string name, string weakness, string unlocksExitDirection, int unlocksExitToRoom)
{
    public int ID { get; set; } = id;
    public string Name { get; set; } = name;
    public bool IsAlive { get; set; } = true;

    // La debolezza del mostro (il nome dell'oggetto che può ucciderlo)
    public string Weakness { get; set; } = weakness;

    // L'uscita che si sblocca alla sua morte
    public string UnlocksExitDirection { get; set; } = unlocksExitDirection;
    public int UnlocksExitToRoom { get; set; } = unlocksExitToRoom;
}
