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
    public void Start()
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
    /// Carica i dati del gioco, in particolare le stanze dal file Rooms.txt
    /// </summary>
    private void LoadGameData()
    {
        Console.WriteLine("Caricamento del mondo di gioco...");

        try
        {
            string[] roomLines = File.ReadAllLines("Rooms.txt");

            foreach (string line in roomLines)
            {
                string[] parts = line.Split(';');

                int id = int.Parse(parts[0]);
                string description = parts[1];
                Room newRoom = new(id, description);

                if (int.Parse(parts[2]) != -1) newRoom.Exits["NORTH"] = int.Parse(parts[2]);
                if (int.Parse(parts[3]) != -1) newRoom.Exits["EAST"] = int.Parse(parts[3]);
                if (int.Parse(parts[4]) != -1) newRoom.Exits["SOUTH"] = int.Parse(parts[4]);
                if (int.Parse(parts[5]) != -1) newRoom.Exits["WEST"] = int.Parse(parts[5]);

                // --- NUOVA LOGICA PER GLI OGGETTI ---
                // Controlla se ci sono oggetti nella stanza (se il campo non è "-1")
                if (parts[6] != "-1")
                {
                    // Divide la stringa degli oggetti (es. "SPADA,POZIONE") in un array
                    string[] itemNames = parts[6].Split(',');
                    foreach (string itemName in itemNames)
                    {
                        // Per ogni nome, crea un nuovo oggetto Item e lo aggiunge alla lista della stanza
                        newRoom.Items.Add(new Item(itemName));
                    }
                }

                // TODO: Caricare i Mostri

                _world.Add(id, newRoom);
            }

            Console.WriteLine($"Caricamento completato: {_world.Count} stanze caricate in memoria.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRORE durante il caricamento del mondo di gioco: {ex.Message}");
        }
    }

    /// <summary>
    /// Descrive la stanza corrente, incluse le uscite, gli oggetti e i mostri.
    /// </summary>
    private void Look()
    {
        Room currentRoom = _world[_player.CurrentRoomId];
        Console.WriteLine(currentRoom.Description);

        Console.WriteLine("Puoi andare verso:");
        // Se non ci sono uscite, lo specifica.
        if (currentRoom.Exits.Count == 0)
        {
            Console.WriteLine("- Nessuna uscita da qui.");
        }
        else
        {
            foreach (var exit in currentRoom.Exits)
            {
                Console.WriteLine($"- {exit.Key}");
            }
        }

        // --- NUOVA LOGICA PER MOSTRARE GLI OGGETTI ---
        // Controlla se ci sono oggetti nella stanza
        if (currentRoom.Items.Count != 0) 
        {
            Console.WriteLine("Vedi i seguenti oggetti per terra:");
            foreach (var item in currentRoom.Items)
            {
                Console.WriteLine($"- {item.Name}");
            }
        }

        // TODO: In seguito, qui mostreremo anche i mostri presenti.
    }

    /// <summary>
    /// Prova a spostare il giocatore nella direzione specificata.
    /// </summary>
    /// <param name="direction">La direzione in cui muoversi (es. "NORTH").</param>
    private void Move(string direction)
    {
        // Prende la stanza attuale del giocatore
        Room currentRoom = _world[_player.CurrentRoomId];

        // Controlla se la direzione fornita è una delle uscite valide dalla stanza attuale
        if (currentRoom.Exits.TryGetValue(direction, out int value))
        {
            // Se l'uscita esiste, aggiorna la stanza corrente del giocatore con l'ID della nuova stanza
            _player.CurrentRoomId = value;

            Console.WriteLine($"\nTi sposti verso {direction}...");

            // Chiama Look() per descrivere la nuova stanza in cui sei appena entrato
            Look();
        }
        else
        {
            // Se non c'è un'uscita in quella direzione
            Console.WriteLine("\nNon puoi andare in quella direzione. C'è un muro.");
        }
    }

    /// <summary>
    /// Il ciclo principale del gioco che continua finché il giocatore non esce.
    /// </summary>
    private void GameLoop()
    {
        bool isPlaying = true;
        Look(); // Descrive la stanza iniziale

        while (isPlaying)
        {
            Console.WriteLine("\nCosa vuoi fare?");
            string? input = Console.ReadLine()?.ToUpper().Trim(); // Legge, converte in maiuscolo e toglie spazi inutili

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Per favore, inserisci un comando.");
                continue; // Salta il resto del ciclo e chiede un nuovo input
            }

            // Divide l'input in parti. Es: "MOVE NORTH" diventa ["MOVE", "NORTH"]
            // Il secondo parametro '2' assicura che divida al massimo in 2 parti, utile per comandi futuri
            string[] inputParts = input.Split([' '], 2);
            string command = inputParts[0];
            string argument = inputParts.Length > 1 ? inputParts[1] : "";

            // Gestore dei comandi
            if (command == "MOVE")
            {
                if (string.IsNullOrEmpty(argument))
                {
                    Console.WriteLine("Dove vuoi andare? Esempio: MOVE NORTH");
                }
                else
                {
                    Move(argument);
                }
            }
            else if (command == "LOOK")
            {
                Look();
            }
            else if (command == "EXIT")
            {
                Console.WriteLine("Grazie per aver giocato!");
                isPlaying = false;
            }
            else
            {
                Console.WriteLine("Comando non valido.");
            }
        }
    }
}
