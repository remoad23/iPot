using System.ComponentModel.DataAnnotations;

namespace iPotAPI.Model
{
    public class Settings
    {
        [Key]
        public int SettingsId { get; set; }
        public float MoistureMinimum { get; set; }
        public float LightMinimum { get; set; }
        public bool AutomaticWatering { get; set; }
        public bool AutomaticLight { get; set; }
        public bool NotificationWater { get; set; }
        public bool NotificationLight { get; set; }
        public bool NotificationMoisture { get; set; }
        public string UptimeStart { get; set; }
        public string UptimeEnd { get; set; }

        public PlantState PlantState { get; set; }
    }
}