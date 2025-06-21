using ZeldaTextAdventure.Models; // Importante per usare Verb e Command!

namespace ZeldaTextAdventure
{
    public class CommandParser
    {
        public static Command Parse(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new Command(Verb.UNKNOWN, "");
            }

            string[] inputParts = input.ToUpper().Trim().Split([' '], 2);
            string verbStr = inputParts[0];
            string argument = inputParts.Length > 1 ? inputParts[1] : "";
            var action = verbStr switch
            {
                "MOVE" => Verb.MOVE,
                "LOOK" => Verb.LOOK,
                "PICK" => Verb.PICK,
                "DROP" => Verb.DROP,
                "ATTACK" => Verb.ATTACK,
                "INVENTORY" or "I" => Verb.INVENTORY,
                "EXIT" => Verb.EXIT,
                _ => Verb.UNKNOWN,
            };
            return new Command(action, argument);
        }
    }
}