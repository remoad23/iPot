using System.Text.Json.Serialization;

namespace iPotAPI.DataTransferObject
{
    public class SetUpTimeData
    {
        [JsonInclude]
        public string uptimeStart { get; set; }
        [JsonInclude]
        public string uptimeEnd { get; set; }
    }
}