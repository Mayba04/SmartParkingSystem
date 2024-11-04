using SmartParkingSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.Entities
{
    public class ParkingSpot : IEntity
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public bool IsOccupied { get; set; }
        public int SensorId { get; set; }
        public DateTime LastUpdated { get; set; }

        public int ParkingLotId { get; set; }
        public ParkingLot ParkingLot { get; set; }
        public Sensor Sensor { get; set; }
    }

}
