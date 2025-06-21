// In Models/Item.cs
using System.Text.Json.Serialization;

namespace ZeldaTextAdventure.Models
{
    public class Item
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        // Non abbiamo più bisogno di un costruttore esplicito come public Item(string name).
        // Il deserializer JSON usa il costruttore pubblico senza parametri, che viene
        // creato di default se non ne definiamo altri, per creare l'oggetto
        // e poi imposta la proprietà 'Name' leggendola dal JSON.
    }
}