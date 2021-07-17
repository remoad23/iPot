using System.Text.Json.Serialization;

namespace iPotAPI.DataTransferObject
{
    public class SetNeedsWater
    {
        [JsonInclude]
        public bool NeedsWater { get; set; }
    }
}