using SmartParkingSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.Entities
{
    public class Sensor : IEntity
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public double LastActiveDistance { get; set; }
        public ICollection<ParkingSpot> ParkingSpots { get; set; }
    }
}
