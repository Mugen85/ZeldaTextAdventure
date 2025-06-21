namespace ZeldaTextAdventure;
// Questo è il file principale del nostro gioco.
// using System.IO; serve per leggere e scrivere file.
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ZeldaTextAdventure.Models;

// --- CLASSE PRINCIPALE DEL GIOCO ---
// Questa classe gestirà la logica principale, il caricamento e il ciclo di gioco.

public class GameEngine
{
    private readonly Player _player = new();
    private Dictionary<int, Room> _world = [];
    private Dictionary<int, Monster> _allMonsters = [];
    public int PlayerCurrentRoomId => _player.CurrentRoomId;
    public bool PlayerHasRescuedPrincess => _player.HasRescuedPrincess;

    // con 'private set' per renderle di sola lettura dall'esterno
    public string? StartStory { get; private set; }
    public string? EndWin { get; private set; }
    public string? EndLose { get; private set; }
    public string? EndDead { get; private set; }

    public GameEngine()
    {
        LoadGameData();
    }

    /// <summary>
    /// Carica i dati del gioco, in particolare le stanze dal file Rooms.txt
    /// </summary>
    // In GameEngine.cs
    private void LoadGameData()
    {
        Console.WriteLine("Caricamento del mondo di gioco da JSON...");

        // Aggiungi questi using in cima al file GameEngine.cs se mancano
        // using System.Text.Json;
        // using ZeldaTextAdventure.Models;

        try
        {
            // 1. Legge l'intero file JSON in una stringa
            string jsonContent = File.ReadAllText("Data/GameData.json");

            // 2. La magia della deserializzazione!
            // Con una sola riga, C# legge la stringa JSON e popola il nostro oggetto GameData
            var gameData = JsonSerializer.Deserialize<GameData>(jsonContent);

            if (gameData == null)
            {
                Console.WriteLine("ERRORE: Impossibile leggere i dati dal file JSON.");
                return;
            }

            // 3. Popola le stringhe di testo nell'engine
            StartStory = gameData.StartStory;
            EndWin = gameData.EndWin;
            EndLose = gameData.EndLose;
            EndDead = gameData.EndDead;

            // 4. Converte le liste di mostri e stanze in dizionari per un accesso veloce
            _allMonsters = (gameData.Monsters ?? []).ToDictionary(m => m.Id);
            _world = (gameData.Rooms ?? []).ToDictionary(r => r.Id);

            // 5. Passo finale: collega i mostri alle stanze
            foreach (var room in _world.Values)
            {
                if (room.MonsterId != -1 && _allMonsters.TryGetValue(room.MonsterId, out Monster? value))
                {
                    room.Monster = value;
                }
            }

            Console.WriteLine($"Caricamento completato: {_world.Count} stanze e {_allMonsters.Count} tipi di mostri caricati.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRORE durante il caricamento del mondo di gioco: {ex.Message}");
        }
    }

    /// <summary>
    /// Descrive la stanza corrente, incluse le uscite, gli oggetti e i mostri.
    /// </summary>
    public void Look()
    {
        Room currentRoom = _world[_player.CurrentRoomId];
        Console.WriteLine(currentRoom.Description);

        Console.WriteLine("Puoi andare verso:");
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

        if (currentRoom.Items.Count != 0)
        {
            Console.WriteLine("Vedi i seguenti oggetti per terra:");
            foreach (var item in currentRoom.Items)
            {
                Console.WriteLine($"- {item.Name}");
            }
        }

        // --- NUOVA LOGICA PER MOSTRARE I MOSTRI ---
        if (currentRoom.Monster != null)
        {
            if (currentRoom.Monster.IsAlive)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nPERICOLO: {currentRoom.Monster.Name} è qui e ti blocca il passaggio!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"\nVedi il corpo senza vita di {currentRoom.Monster.Name} per terra.");
                Console.ResetColor();
            }
        }
    }

    /// <summary>
    /// Prova a spostare il giocatore nella direzione specificata.
    /// </summary>
    /// <param name="direction">La direzione in cui muoversi (es. "NORTH").</param>
    public void Move(string direction)
    {
        Room currentRoom = _world[_player.CurrentRoomId];

        if (currentRoom.Exits.TryGetValue(direction, out int value))
        {
            // Aggiorna la stanza corrente del giocatore
            _player.CurrentRoomId = value;

            Console.WriteLine($"\nTi sposti verso {direction}...");

            // --- NUOVA LOGICA PER SALVARE LA PRINCIPESSA ---
            const int princessRoomId = 8;
            // Controlla se siamo entrati nella stanza della principessa E se non l'avevamo già salvata
            if (_player.CurrentRoomId == princessRoomId && !_player.HasRescuedPrincess)
            {
                _player.HasRescuedPrincess = true;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\nIncredibile! In un angolo della stanza trovi la Principessa Zelda, sana e salva!");
                Console.WriteLine("L'hai salvata! Ora dovete fuggire dal castello!");
                Console.ResetColor();
            }

            // Chiama Look() per descrivere la stanza (nuova o vecchia)
            Look();
        }
        else
        {
            Console.WriteLine("\nNon puoi andare in quella direzione. C'è un muro.");
        }
    }

    /// <summary>
    /// Prova a raccogliere un oggetto dalla stanza e ad aggiungerlo all'inventario del giocatore.
    /// </summary>
    /// <param name="itemName">Il nome dell'oggetto da raccogliere.</param>
    public void Pick(string itemName)
    {
        Room currentRoom = _world[_player.CurrentRoomId];

        // Cerca l'oggetto nella stanza, ignorando le differenze tra maiuscole e minuscole.
        // FirstOrDefault è un metodo LINQ che restituisce il primo elemento che soddisfa la condizione,
        // o null se non ne trova nessuno. È perfetto per questo scopo.
        Item? itemToPick = currentRoom.Items.FirstOrDefault(i => i.Name != null && i.Name.Equals(itemName, StringComparison.CurrentCultureIgnoreCase));
        if (itemToPick != null)
        {
            // L'oggetto è stato trovato nella stanza.

            // Rimuoviamo l'oggetto dalla lista della stanza.
            currentRoom.Items.Remove(itemToPick);

            // Aggiungiamo lo stesso oggetto alla borsa (inventario) del giocatore.
            _player.Bag.Add(itemToPick);

            Console.WriteLine($"\nHai raccolto: {itemToPick.Name}");
        }
        else
        {
            // L'oggetto non è stato trovato.
            Console.WriteLine($"\nNon c'è nessun {itemName} qui.");
        }
    }

    /// <summary>
    /// Prova a lasciare un oggetto dall'inventario del giocatore nella stanza corrente.
    /// </summary>
    /// <param name="itemName">Il nome dell'oggetto da lasciare.</param>
    public void Drop(string itemName)
    {
        // Cerca l'oggetto nella borsa del giocatore, ignorando le differenze tra maiuscole e minuscole.
        // In GameEngine.cs, metodo Drop()
        Item? itemToDrop = _player.Bag.FirstOrDefault(i => i.Name != null && i.Name.Equals(itemName, StringComparison.CurrentCultureIgnoreCase));
        if (itemToDrop != null)
        {
            // L'oggetto è stato trovato nell'inventario.

            // Rimuoviamo l'oggetto dalla borsa del giocatore.
            _player.Bag.Remove(itemToDrop);

            // Aggiungiamo lo stesso oggetto alla stanza corrente.
            Room currentRoom = _world[_player.CurrentRoomId];
            currentRoom.Items.Add(itemToDrop);

            Console.WriteLine($"\nHai lasciato cadere: {itemToDrop.Name}");
        }
        else
        {
            // L'oggetto non è stato trovato nell'inventario.
            Console.WriteLine($"\nNon hai un {itemName} nella borsa.");
        }
    }

    /// <summary>
    /// Gestisce il tentativo del giocatore di attaccare un mostro nella stanza.
    /// </summary>
    /// <returns>Restituisce 'false' se il giocatore muore, altrimenti 'true'.</returns>
    public bool Attack()
    {
        Room currentRoom = _world[_player.CurrentRoomId];

        // Controlla se c'è un mostro nella stanza
        if (currentRoom.Monster == null)
        {
            Console.WriteLine("\nNon c'è niente da attaccare qui.");
            return true; // Il gioco continua
        }

        // Controlla se il mostro è già morto
        if (!currentRoom.Monster.IsAlive)
        {
            Console.WriteLine($"\nAttacchi il corpo senza vita di {currentRoom.Monster.Name}. Non è molto sportivo.");
            return true; // Il gioco continua
        }

        // C'è un mostro vivo, inizia il combattimento!
        Monster monster = currentRoom.Monster;
        Console.WriteLine($"\nTi prepari a combattere contro {monster.Name}!");

        // Controlla se il giocatore ha l'arma giusta (la debolezza del mostro)
        // In GameEngine.cs, metodo Attack()
        bool hasWeaknessItem = _player.Bag.Any(item => item.Name != null && item.Name.Equals(monster.Weakness, StringComparison.CurrentCultureIgnoreCase));
        if (hasWeaknessItem)
        {
            // --- VITTORIA ---
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Usando il tuo {monster.Weakness}, riesci a sconfiggere {monster.Name}!");
            Console.ResetColor();

            monster.IsAlive = false;

            // Aggiungiamo l'uscita SOLO se la direzione è una stringa valida
            if (!string.IsNullOrEmpty(monster.UnlocksExitDirection))
            {
                currentRoom.Exits[monster.UnlocksExitDirection] = monster.UnlocksExitToRoom;
                Console.WriteLine($"Sconfiggendolo, hai aperto un nuovo passaggio verso {monster.UnlocksExitDirection}!");
            }

            return true; // Il gioco continua
        }
        else
        {
            // --- SCONFITTA ---
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"{monster.Name} è troppo forte! Senza l'arma giusta, non hai speranze.");
            Console.ResetColor();

            // Usa la proprietà della classe, non leggere più il file!
            Console.WriteLine(this.EndDead);

            return false; // Il gioco finisce
        }
    }

  

    /// <summary>
    /// Mostra al giocatore gli oggetti contenuti nel suo inventario (borsa).
    /// </summary>
    public void ShowInventory()
    {
        Console.WriteLine("\n--- INVENTARIO ---");
        if (_player.Bag.Count == 0)
        {
            Console.WriteLine("La tua borsa è vuota.");
        }
        else
        {
            Console.WriteLine("Nella tua borsa hai:");
            foreach (var item in _player.Bag)
            {
                Console.WriteLine($"- {item.Name}");
            }
        }
        Console.WriteLine("------------------");
    }
}
