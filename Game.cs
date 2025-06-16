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
            // Legge tutte le righe dal file Rooms.txt. Ogni riga è una stanza.
            string[] roomLines = File.ReadAllLines("Rooms.txt");

            // Itera su ogni riga (cioè su ogni stanza) letta dal file.
            foreach (string line in roomLines)
            {
                // Divide la riga in parti usando il punto e virgola come separatore.
                string[] parts = line.Split(';');

                // Estrae le informazioni e le converte nel tipo corretto.
                // int.Parse() converte una stringa (es. "1") in un numero intero (es. 1).
                int id = int.Parse(parts[0]);
                string description = parts[1];

                // Crea un nuovo oggetto Room con l'ID e la descrizione.
                Room newRoom = new(id, description);

                // Aggiunge le uscite alla stanza. Se l'uscita è -1, viene ignorata.
                if (int.Parse(parts[2]) != -1) newRoom.Exits["NORTH"] = int.Parse(parts[2]);
                if (int.Parse(parts[3]) != -1) newRoom.Exits["EAST"] = int.Parse(parts[3]);
                if (int.Parse(parts[4]) != -1) newRoom.Exits["SOUTH"] = int.Parse(parts[4]);
                if (int.Parse(parts[5]) != -1) newRoom.Exits["WEST"] = int.Parse(parts[5]);

                // TODO: Aggiungeremo la logica per caricare Oggetti e Mostri in seguito.
                // Per ora, ci concentriamo sulle stanze e le loro connessioni.

                // Aggiunge la stanza appena creata e configurata al nostro mondo di gioco.
                // La chiave è l'ID della stanza, il valore è l'oggetto Room stesso.
                _world.Add(id, newRoom);
            }

            // Messaggio di successo per farci sapere che tutto è andato bene.
            Console.WriteLine($"Caricamento completato: {_world.Count} stanze caricate in memoria.");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("ERRORE: Impossibile trovare il file 'Rooms.txt'. Assicurati che esista e che sia impostato su 'Copia se più recente'.");
        }
        catch (Exception ex)
        {
            // Cattura qualsiasi altro errore durante il caricamento per evitare che il programma si blocchi.
            Console.WriteLine($"ERRORE durante il caricamento del mondo di gioco: {ex.Message}");
        }
    }

    /// <summary>
    /// Descrive la stanza corrente, incluse le uscite, gli oggetti e i mostri.
    /// </summary>
    private void Look()
    {
        // 1. Prende la stanza attuale dal dizionario _world usando l'ID della stanza del giocatore.
        Room currentRoom = _world[_player.CurrentRoomId];

        // 2. Stampa la descrizione della stanza.
        Console.WriteLine(currentRoom.Description);

        // 3. Mostra tutte le uscite possibili dalla stanza.
        Console.WriteLine("Puoi andare verso:");
        foreach (var exit in currentRoom.Exits)
        {
            // exit.Key è la direzione (es. "NORTH"), exit.Value è l'ID della stanza di destinazione.
            Console.WriteLine($"- {exit.Key}");
        }

        // TODO: In seguito, qui mostreremo anche gli oggetti e i mostri presenti.
    }

    /// <summary>
    /// Il ciclo principale del gioco che continua finché il giocatore non esce.
    /// </summary>
    private void GameLoop()
    {
        bool isPlaying = true;

        // Appena il gioco inizia, facciamo un "LOOK" automatico per descrivere la prima stanza.
        Look();

        while (isPlaying)
        {
            Console.WriteLine("\nCosa vuoi fare?");
            string? input = Console.ReadLine()?.ToUpper();

            // Ora il nostro switch capisce anche "LOOK"
            switch (input)
            {
                case "LOOK":
                    Look();
                    break;
                case "EXIT":
                    Console.WriteLine("Grazie per aver giocato!");
                    isPlaying = false;
                    break;

                // TODO: Qui aggiungeremo il caso per "MOVE"

                default:
                    Console.WriteLine("Comando non valido.");
                    break;
            }
        }
    }
}
