// File: Program.cs
using ZeldaTextAdventure.Models; // Assicurati di avere gli using necessari

namespace ZeldaTextAdventure
{
    public class Program
    {
        // In Program.cs
        public static void Main()
        {
            var engine = new GameEngine();
            bool isPlaying = true;

            // Ora prendiamo il testo dall'engine, non più da un file!
            Console.WriteLine(engine.StartStory);
            Console.WriteLine("\nPremi Invio per iniziare...");
            Console.ReadLine();

            engine.Look();

            while (isPlaying)
            {
                Console.WriteLine("\nCosa vuoi fare?");
                string? input = Console.ReadLine();
                Command command = CommandParser.Parse(input);

                switch (command.Action)
                {
                    // ... tutti i case da MOVE a ATTACK rimangono invariati ...
                    case Verb.MOVE: if (string.IsNullOrEmpty(command.Argument)) Console.WriteLine("Dove vuoi andare?"); else engine.Move(command.Argument); break;
                    case Verb.LOOK: engine.Look(); break;
                    case Verb.INVENTORY: engine.ShowInventory(); break;
                    case Verb.PICK: if (string.IsNullOrEmpty(command.Argument)) Console.WriteLine("Cosa vuoi raccogliere?"); else engine.Pick(command.Argument); break;
                    case Verb.DROP: if (string.IsNullOrEmpty(command.Argument)) Console.WriteLine("Cosa vuoi lasciare?"); else engine.Drop(command.Argument); break;
                    case Verb.ATTACK: isPlaying = engine.Attack(); break;

                    case Verb.EXIT:
                        const int exitRoomId = 1;
                        if (engine.PlayerCurrentRoomId == exitRoomId)
                        {
                            // Anche qui, prendiamo il testo dall'engine
                            if (engine.PlayerHasRescuedPrincess)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine(engine.EndWin);
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine(engine.EndLose);
                                Console.ResetColor();
                            }
                        }
                        else { Console.WriteLine("Grazie per aver giocato!"); }
                        isPlaying = false;
                        break;

                    case Verb.UNKNOWN: Console.WriteLine("Comando non valido."); break;
                }
            }
        }
    }
}