using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace iPotAPI.Model
{
    public class PlantState
    {
        [Key]
        public int PlantStateId { get; set; }
        
        public bool NeedsWater { get; set; }

        [ForeignKey("SettingsId")]
        public int? SettingsId { get; set; }
        public float LedIntensity { get; set; }
        public float AmbientLightIntensity { get; set; }
        public byte WaterStorage { get; set; }
        public float MoistureValue { get; set; }
        public Settings Settings { get; set; }
    }
}