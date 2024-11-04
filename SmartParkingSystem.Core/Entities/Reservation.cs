using SmartParkingSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.Entities
{
    public class Reservation : IEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ParkingSpotId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public AppUser User { get; set; }
        public ParkingSpot ParkingSpot { get; set; }
    }

}
