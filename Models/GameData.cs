// In Models/GameData.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZeldaTextAdventure.Models
{
    // Questa è la classe principale che rappresenta l'intero file JSON
    public class GameData
    {
        // Usiamo un attributo per far corrispondere la chiave "startStory" del JSON
        // alla proprietà "StartStory" di C#, seguendo le convenzioni di nomenclatura.
        [JsonPropertyName("startStory")]
        public string? StartStory { get; set; }

        [JsonPropertyName("endWin")]
        public string? EndWin { get; set; }

        [JsonPropertyName("endLose")]
        public string? EndLose { get; set; }

        [JsonPropertyName("endDead")]
        public string? EndDead { get; set; }

        [JsonPropertyName("monsters")]
        public List<Monster>? Monsters { get; set; }

        [JsonPropertyName("rooms")]
        public List<Room>? Rooms { get; set; }
    }
}