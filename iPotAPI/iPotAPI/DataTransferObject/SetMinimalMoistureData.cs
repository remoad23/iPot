using System.Text.Json.Serialization;

namespace iPotAPI.DataTransferObject
{
    public class SetMinimalMoistureData
    {
        [JsonInclude]
        public double MinimalMoisture { get; set; }
    }
}