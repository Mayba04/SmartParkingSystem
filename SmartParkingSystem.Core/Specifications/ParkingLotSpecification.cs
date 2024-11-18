using Ardalis.Specification;
using SmartParkingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.Specifications
{
    public class ParkingLotSpecification : Specification<ParkingLot>
    {
        public class GetByNameAndLocationWithPagination : Specification<ParkingLot>
        {
            public GetByNameAndLocationWithPagination(string? searchTerm, int page, int pageSize)
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    Query.Where(parkingLot =>
                        parkingLot.Name.Contains(searchTerm) ||
                        parkingLot.Location.Contains(searchTerm));
                }

                Query.Skip((page - 1) * pageSize).Take(pageSize);
            }
        }

        public class GetByNameOrLocation : Specification<ParkingLot>
        {
            public GetByNameOrLocation(string searchTerm)
            {
                Query.Where(parkingLot =>
                    parkingLot.Name.Contains(searchTerm) ||
                    parkingLot.Location.Contains(searchTerm));
            }
        }
    }

}
