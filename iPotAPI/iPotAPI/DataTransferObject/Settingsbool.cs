using System.Text.Json.Serialization;

namespace iPotAPI.DataTransferObject
{
    public class Settingsbool
    {
        [JsonInclude]
        public bool NotificationWater { get; set; }
        [JsonInclude]
        public bool NotificationLight { get; set; }
        [JsonInclude]
        public bool NotificationMoisture { get; set; }
        [JsonInclude]
        public bool automaticLight { get; set; }
        [JsonInclude]
        public bool automaticWatering { get; set; }
    }
}