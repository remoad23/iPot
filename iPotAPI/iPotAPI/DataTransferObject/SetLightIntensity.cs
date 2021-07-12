using System.Text.Json.Serialization;

namespace iPotAPI.DataTransferObject
{
    public class SetLightIntensity
    {
        [JsonInclude]
        public double intensity { get; set; }
    }
}