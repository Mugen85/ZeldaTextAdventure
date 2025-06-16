namespace ZeldaTextAdventure;
// Questo è il file principale del nostro gioco.
// using System.IO; serve per leggere e scrivere file.
using System;
using System.Collections.Generic;
using System.IO;

// --- CLASSE PRINCIPALE DEL GIOCO ---
// Questa classe gestirà la logica principale, il caricamento e il ciclo di gioco.

public class Game
{
    private readonly Player _player = new();
    private readonly Dictionary<int, Room> _world = [];

    /// <summary>
    /// Metodo principale che avvia e gestisce il gioco.
    /// </summary>
    public static void Start()
    {
        // 1. Carica i dati del gioco dai file
        LoadGameData();

        // 2. Mostra la storia iniziale
        Console.WriteLine(File.ReadAllText("Start.txt"));
        Console.WriteLine("\nPremi Invio per iniziare...");
        Console.ReadLine();

        // 3. Avvia il ciclo di gioco
        GameLoop();
    }

    /// <summary>
    /// Carica le stanze dal file Rooms.txt
    /// </summary>
    private static void LoadGameData()
    {
        // TODO: Scriveremo il codice per caricare le stanze qui nel prossimo passaggio.
        Console.WriteLine("Caricamento del mondo di gioco...");
    }

    /// <summary>
    /// Il ciclo principale del gioco che continua finché il giocatore non esce.
    /// </summary>
    private static void GameLoop()
    {
        bool isPlaying = true;
        while (isPlaying)
        {
            // TODO: Descriveremo la stanza e chiederemo l'input al giocatore.
            Console.WriteLine("\nCosa vuoi fare?");
            string? input = Console.ReadLine()?.ToUpper(); // Legge l'input e lo converte in maiuscolo

            switch (input)
            {
                case "EXIT":
                    Console.WriteLine("Grazie per aver giocato!");
                    isPlaying = false;
                    break;
                // Aggiungeremo altri comandi (MOVE, LOOK, etc.) qui.
                default:
                    Console.WriteLine("Comando non valido.");
                    break;
            }
        }
    }
}
