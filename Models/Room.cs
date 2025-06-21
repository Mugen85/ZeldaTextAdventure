using System.Text.Json.Serialization;

namespace ZeldaTextAdventure.Models
{
    public class Room
    {
        // --- PROPRIETÀ DA AGGIUNGERE/VERIFICARE ---
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("monsterId")]
        public int MonsterId { get; set; } // Fondamentale per il caricamento

        // Proprietà esistenti
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("exits")]
        public Dictionary<string, int> Exits { get; set; } = [];

        [JsonPropertyName("items")]
        public List<Item> Items { get; set; } = [];

        // Questa proprietà non viene letta dal JSON, ma la popoliamo noi dopo.
        // La lasciamo così com'è.
        [JsonIgnore] // Ignora questa proprietà durante la deserializzazione
        public Monster? Monster { get; set; }
    }
}
