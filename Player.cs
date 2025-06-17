namespace ZeldaTextAdventure;
// Questo è il file principale del nostro gioco.
using System.Collections.Generic;

/// <summary>
/// Rappresenta il giocatore.
/// </summary>
public class Player
{
    // L'ID della stanza in cui si trova attualmente il giocatore.
    public int CurrentRoomId { get; set; } = 1; // Inizia sempre dalla stanza 1

    // La "borsa" del giocatore per contenere gli oggetti.
    public List<Item> Bag { get; set; } = [];
}
