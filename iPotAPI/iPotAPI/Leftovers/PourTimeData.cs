using System;
using System.Text.Json.Serialization;

namespace iPotAPI.DataTransferObject
{
    public class PourTimeData
    {
        [JsonInclude]
        public int Percentage { get; set; }
        [JsonInclude]
        public string Time { get; set; }
    }
}