namespace backend.models;

using System;
using System.Text.Json.Serialization;

public sealed class ModelItem
{
   [JsonPropertyName("id")] public int Id { get; set; }
   [JsonPropertyName("name")] public string Name { get; set; }
   [JsonPropertyName("description")] public string Description { get; set; }
   [JsonPropertyName("likes")] public int Likes { get; set; }
   [JsonPropertyName("image")] public string Image { get; set; }
   [JsonPropertyName("category")] public string Category { get; set; }
   [JsonPropertyName("dateAdded")] public DateTime DateAdded { get; set; }
}