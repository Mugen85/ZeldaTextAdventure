namespace ZeldaTextAdventure;
// Questo è il file principale del nostro gioco.
// using System.IO; serve per leggere e scrivere file.
using System;
using System.Collections.Generic;
using System.IO;
using ZeldaTextAdventure.Models;

// --- CLASSE PRINCIPALE DEL GIOCO ---
// Questa classe gestirà la logica principale, il caricamento e il ciclo di gioco.

public class Game
{
    private readonly Player _player = new();
    private readonly Dictionary<int, Room> _world = [];
    private readonly Dictionary<int, Monster> _allMonsters = [];

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
            // --- 1. CARICA TUTTI I MOSTRI ---
            string[] monsterLines = File.ReadAllLines("Monsters.txt");
            foreach (string line in monsterLines)
            {
                string[] parts = line.Split(';');
                int id = int.Parse(parts[0]);
                string name = parts[1];
                string weakness = parts[2];
                string unlocksDir = parts[3];
                int unlocksRoom = int.Parse(parts[4]);

                Monster newMonster = new(id, name, weakness, unlocksDir, unlocksRoom);
                _allMonsters.Add(id, newMonster);
            }

            // --- 2. CARICA LE STANZE E ASSEGNA I MOSTRI ---
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

                if (parts[6] != "-1")
                {
                    string[] itemNames = parts[6].Split(',');
                    foreach (string itemName in itemNames)
                    {
                        newRoom.Items.Add(new Item(itemName.Trim()));
                    }
                }

                // Assegna il mostro alla stanza se presente
                int monsterId = int.Parse(parts[7]);
                if (monsterId != -1)
                {
                    newRoom.Monster = _allMonsters[monsterId];
                }

                _world.Add(id, newRoom);
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
    private void Look()
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
    private void Move(string direction)
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
    private void Pick(string itemName)
    {
        Room currentRoom = _world[_player.CurrentRoomId];

        // Cerca l'oggetto nella stanza, ignorando le differenze tra maiuscole e minuscole.
        // FirstOrDefault è un metodo LINQ che restituisce il primo elemento che soddisfa la condizione,
        // o null se non ne trova nessuno. È perfetto per questo scopo.
        Item? itemToPick = currentRoom.Items.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.CurrentCultureIgnoreCase));

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
    private void Drop(string itemName)
    {
        // Cerca l'oggetto nella borsa del giocatore, ignorando le differenze tra maiuscole e minuscole.
        Item? itemToDrop = _player.Bag.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.CurrentCultureIgnoreCase));

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
    private bool Attack()
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
        bool hasWeaknessItem = _player.Bag.Any(item => item.Name.Equals(monster.Weakness, StringComparison.CurrentCultureIgnoreCase));

        if (hasWeaknessItem)
        {
            // --- VITTORIA ---
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Usando il tuo {monster.Weakness}, riesci a sconfiggere {monster.Name}!");
            Console.ResetColor();

            monster.IsAlive = false;

            // Sblocca la nuova uscita
            currentRoom.Exits[monster.UnlocksExitDirection] = monster.UnlocksExitToRoom;
            Console.WriteLine($"Sconfiggendolo, hai aperto un nuovo passaggio verso {monster.UnlocksExitDirection}!");

            return true; // Il gioco continua
        }
        else
        {
            // --- SCONFITTA ---
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"{monster.Name} è troppo forte! Senza l'arma giusta, non hai speranze.");
            Console.ResetColor();

            // Legge e mostra il file di morte e termina il gioco
            Console.WriteLine(File.ReadAllText("EndDead.txt"));

            return false; // Il gioco finisce
        }
    }

    /// <summary>
    /// Il ciclo principale del gioco che continua finché il giocatore non esce.
    /// </summary>
    private void GameLoop()
    {
        bool isPlaying = true;
        Look();

        while (isPlaying)
        {
            Console.WriteLine("\nCosa vuoi fare?");
            string? input = Console.ReadLine()?.ToUpper().Trim();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Per favore, inserisci un comando.");
                continue;
            }

            string[] inputParts = input.Split([' '], 2);
            string command = inputParts[0];
            string argument = inputParts.Length > 1 ? inputParts[1] : "";

            if (command == "MOVE")
            {
                if (string.IsNullOrEmpty(argument)) Console.WriteLine("Dove vuoi andare? Esempio: MOVE NORTH");
                else Move(argument);
            }
            else if (command == "LOOK")
            {
                Look();
            }
            else if (command == "INVENTORY" || command == "I")
            {
                ShowInventory();
            }
            else if (command == "PICK")
            {
                if (string.IsNullOrEmpty(argument)) Console.WriteLine("Cosa vuoi raccogliere? Esempio: PICK SPADA");
                else Pick(argument);
            }
            else if (command == "DROP")
            {
                if (string.IsNullOrEmpty(argument)) Console.WriteLine("Cosa vuoi lasciare? Esempio: DROP SPADA");
                else Drop(argument);
            }
            else if (command == "ATTACK")
            {
                isPlaying = Attack();
            }
            // --- BLOCCO EXIT POTENZIATO ---
            else if (command == "EXIT")
            {
                const int exitRoomId = 1;
                // Controlla se il giocatore sta uscendo dalla stanza iniziale
                if (_player.CurrentRoomId == exitRoomId)
                {
                    // Se sì, controlla se la principessa è stata salvata
                    if (_player.HasRescuedPrincess)
                    {
                        // VITTORIA
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(File.ReadAllText("EndWin.txt"));
                        Console.ResetColor();
                    }
                    else
                    {
                        // SCONFITTA
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(File.ReadAllText("EndLose.txt"));
                        Console.ResetColor();
                    }
                }
                else
                {
                    // Uscita normale da un'altra stanza
                    Console.WriteLine("Grazie per aver giocato!");
                }

                // In ogni caso, il gioco finisce
                isPlaying = false;
            }
            else
            {
                Console.WriteLine("Comando non valido.");
            }
        }
    }

    /// <summary>
    /// Mostra al giocatore gli oggetti contenuti nel suo inventario (borsa).
    /// </summary>
    private void ShowInventory()
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
