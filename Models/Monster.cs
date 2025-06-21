/// <summary>
/// Rappresenta un mostro nel gioco.
/// </summary>
using System.Text.Json.Serialization;

namespace ZeldaTextAdventure.Models
{
    public class Monster
    {
        // --- PROPRIETÀ DA AGGIUNGERE/VERIFICARE ---
        [JsonPropertyName("id")]
        public int Id { get; set; }

        // Proprietà esistenti
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("isAlive")]
        public bool IsAlive { get; set; } = true;

        [JsonPropertyName("weakness")]
        public string? Weakness { get; set; }

        [JsonPropertyName("unlocksExitDirection")]
        public string? UnlocksExitDirection { get; set; }

        [JsonPropertyName("unlocksExitToRoom")]
        public int UnlocksExitToRoom { get; set; }
    }
}
