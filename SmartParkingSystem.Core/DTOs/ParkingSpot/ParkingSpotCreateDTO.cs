﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.DTOs.ParkingSpot
{
    public class ParkingSpotCreateDTO
    {
        public string Location { get; set; }
        public bool IsOccupied { get; set; }
        public int SensorId { get; set; }
        public int ParkingLotId { get; set; }
    }
}