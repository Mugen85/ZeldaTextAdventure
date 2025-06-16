namespace ZeldaTextAdventure;
// Questo è il file principale del nostro gioco.
// using System.IO; serve per leggere e scrivere file.
using System.Collections.Generic;

/// <summary>
/// Rappresenta una stanza nel nostro castello.
/// Contiene uscite, oggetti e un possibile mostro.
/// </summary>
public class Room(int id, string description)
{
    public int ID { get; set; } = id;
    public string Description { get; set; } = description;

    // Un dizionario per le uscite. La chiave è la direzione (es. "NORTH"),
    // il valore è l'ID della stanza a cui porta.
    public Dictionary<string, int> Exits { get; set; } = [];

    // Una lista per gli oggetti presenti nella stanza.
    public List<Item> Items { get; set; } = [];

    // Il mostro nella stanza (può essere nullo se non c'è nessun mostro).
    public Monster? Monster { get; set; }
}
