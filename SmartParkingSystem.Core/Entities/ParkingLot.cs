using SmartParkingSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.Entities
{
    public class ParkingLot : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int TotalCapacity { get; set; }
        public double Latitude { get; set; } 
        public double Longitude { get; set; }
        public ICollection<ParkingSpot> ParkingSpots { get; set; }
    }

}
