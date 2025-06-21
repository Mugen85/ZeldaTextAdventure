// File: Program.cs
using ZeldaTextAdventure.Models; // Assicurati di avere gli using necessari

namespace ZeldaTextAdventure
{
    public class Program
    {
        public static void Main()
        {
            // 1. Inizializzazione
            var engine = new GameEngine();
            bool isPlaying = true;

            Console.WriteLine(File.ReadAllText("Data/Start.txt"));
            Console.WriteLine("\nPremi Invio per iniziare...");
            Console.ReadLine();

            engine.Look();

            // 2. Ciclo di Gioco
            while (isPlaying)
            {
                Console.WriteLine("\nCosa vuoi fare?");
                string? input = Console.ReadLine();
                Command command = CommandParser.Parse(input);

                switch (command.Action)
                {
                    case Verb.MOVE:
                        if (string.IsNullOrEmpty(command.Argument)) Console.WriteLine("Dove vuoi andare?");
                        else engine.Move(command.Argument);
                        break;
                    case Verb.LOOK:
                        engine.Look();
                        break;
                    case Verb.INVENTORY:
                        engine.ShowInventory();
                        break;
                    case Verb.PICK:
                        if (string.IsNullOrEmpty(command.Argument)) Console.WriteLine("Cosa vuoi raccogliere?");
                        else engine.Pick(command.Argument);
                        break;
                    case Verb.DROP:
                        if (string.IsNullOrEmpty(command.Argument)) Console.WriteLine("Cosa vuoi lasciare?");
                        else engine.Drop(command.Argument);
                        break;
                    case Verb.ATTACK:
                        isPlaying = engine.Attack();
                        break;

                    // --- BLOCCO EXIT FINALE E CORRETTO ---
                    case Verb.EXIT:
                        const int exitRoomId = 1;
                        // Controlla lo stato TRAMITE le nuove proprietà dell'engine
                        if (engine.PlayerCurrentRoomId == exitRoomId)
                        {
                            if (engine.PlayerHasRescuedPrincess)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine(File.ReadAllText("Data/EndWin.txt"));
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine(File.ReadAllText("Data/EndLose.txt"));
                                Console.ResetColor();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Grazie per aver giocato!");
                        }
                        isPlaying = false;
                        break;

                    case Verb.UNKNOWN:
                        Console.WriteLine("Comando non valido.");
                        break;
                }
            }
        }
    }
}