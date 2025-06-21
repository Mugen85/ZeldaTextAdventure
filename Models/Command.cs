namespace ZeldaTextAdventure.Models
{
    // Un enum rappresenta un set fisso di costanti. È molto più sicuro
    // e pulito che usare stringhe come "MOVE", "LOOK", ecc.
    public enum Verb
    {
        MOVE,
        LOOK,
        PICK,
        DROP,
        ATTACK,
        INVENTORY,
        EXIT,
        UNKNOWN // Per comandi non riconosciuti
    }

    // Questa classe rappresenta un comando del giocatore, già interpretato.
    public class Command(Verb action, string argument)
    {
        public Verb Action { get; } = action;
        public string Argument { get; } = argument;
    }
}