using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.DTOs.Sensor
{
    public class SensorDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public double LastActiveDistance { get; set; }
    }
}
