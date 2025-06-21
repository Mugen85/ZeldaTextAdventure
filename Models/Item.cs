namespace ZeldaTextAdventure.Models;

// Questo è il file principale del nostro gioco.
// using System.IO; serve per leggere e scrivere file.
// --- CLASSI PER I DATI DEL GIOCO ---
// Queste classi servono come "modelli" per gli oggetti del nostro gioco.

/// <summary>
/// Rappresenta un oggetto che il giocatore può raccogliere o lasciare.
/// </summary>
public class Item(string name)
{
    public string Name { get; set; } = name;
}
