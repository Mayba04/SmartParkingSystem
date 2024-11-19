using SmartParkingSystem.Core.DTOs.ParkingSpot;
using SmartParkingSystem.Core.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.DTOs.Reservation
{
    public class ReservationExtendedDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ParkingSpotId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string UserFullName { get; set; } // Ім'я користувача
        public string ParkingSpotLocation { get; set; } // Локація паркомісця
        public string ParkingLotName { get; set; } // Назва паркінгу
    }

}
