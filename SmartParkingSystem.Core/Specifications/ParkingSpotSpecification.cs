using Ardalis.Specification;
using SmartParkingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SmartParkingSystem.Core.Specifications
{
    public class ParkingSpotSpecification : Specification<ParkingSpot>
    {
        public class GetByLocationAndPagination : Specification<ParkingSpot>
        {
            public GetByLocationAndPagination(string? location, int page, int pageSize)
            {
                if (!string.IsNullOrEmpty(location))
                {
                    Query.Where(spot => spot.Location.Contains(location));
                }
                Query.Skip((page - 1) * pageSize).Take(pageSize);
            }
        }

        public class GetByParkingLotAndLocation : Specification<ParkingSpot>
        {
            public GetByParkingLotAndLocation(int parkingLotId, string? location, int page, int pageSize)
            {
                Query.Where(spot => spot.ParkingLotId == parkingLotId);
                if (!string.IsNullOrEmpty(location))
                {
                    Query.Where(spot => spot.Location.Contains(location));
                }
                Query.Skip((page - 1) * pageSize).Take(pageSize);
            }
        }

        public class GetByLocation : Specification<ParkingSpot>
        {
            public GetByLocation(string location)
            {
                Query.Where(spot => spot.Location.Contains(location));
            }
        }

        public class GetByParkingLot : Specification<ParkingSpot>
        {
            public GetByParkingLot(int parkingLotId)
            {
                Query.Where(spot => spot.ParkingLotId == parkingLotId);
            }
        }

        public class GetByParkingLotAndLocationWithoutPagination : Specification<ParkingSpot>
        {
            public GetByParkingLotAndLocationWithoutPagination(int parkingLotId, string? location)
            {
                Query.Where(spot => spot.ParkingLotId == parkingLotId);
                if (!string.IsNullOrEmpty(location))
                {
                    Query.Where(spot => spot.Location.Contains(location));
                }
            }
        }
    }



}
