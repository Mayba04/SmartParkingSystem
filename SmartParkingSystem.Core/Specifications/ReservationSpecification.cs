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
    public class ReservationSpecification : Specification<Reservation>
    {
        public class GetFilteredReservations : Specification<Reservation>
        {
            public GetFilteredReservations(string? globalQuery, DateTime? startDate, DateTime? endDate, int page, int pageSize)
            {
                if (!string.IsNullOrEmpty(globalQuery))
                {
                    Query.Where(reservation =>
                        reservation.User.FullName.Contains(globalQuery) ||
                        reservation.User.Email.Contains(globalQuery) ||
                        reservation.User.Id.Contains(globalQuery) ||
                        reservation.ParkingSpot.Location.Contains(globalQuery) ||
                        reservation.ParkingSpot.ParkingLot.Name.Contains(globalQuery) ||
                        reservation.ParkingSpot.ParkingLot.Location.Contains(globalQuery));
                }

                if (startDate.HasValue)
                {
                    Query.Where(reservation => reservation.StartTime >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    Query.Where(reservation => reservation.EndTime <= endDate.Value);
                }

                Query.Include(reservation => reservation.User)
                     .Include(reservation => reservation.ParkingSpot)
                     .ThenInclude(spot => spot.ParkingLot); 

                Query.Skip((page - 1) * pageSize).Take(pageSize);
            }
        }

        public class GetFilteredReservationsWithoutPagination : Specification<Reservation>
        {
            public GetFilteredReservationsWithoutPagination(string? globalQuery, DateTime? startDate, DateTime? endDate)
            {
                if (!string.IsNullOrEmpty(globalQuery))
                {
                    Query.Where(reservation =>
                        reservation.User.FullName.Contains(globalQuery) ||
                        reservation.User.Email.Contains(globalQuery) ||
                        reservation.User.Id.Contains(globalQuery) ||
                        reservation.ParkingSpot.Location.Contains(globalQuery) ||
                        reservation.ParkingSpot.ParkingLot.Name.Contains(globalQuery) ||
                        reservation.ParkingSpot.ParkingLot.Location.Contains(globalQuery));
                }

                if (startDate.HasValue)
                {
                    Query.Where(reservation => reservation.StartTime >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    Query.Where(reservation => reservation.EndTime <= endDate.Value);
                }

                Query.Include(reservation => reservation.User)
                     .Include(reservation => reservation.ParkingSpot)
                     .ThenInclude(spot => spot.ParkingLot);
            }
        }

        public class GetByParkingSpotAndTime : Specification<Reservation>
        {
            public GetByParkingSpotAndTime(int parkingSpotId, DateTime startTime, DateTime endTime)
            {
                Query.Where(reservation => reservation.ParkingSpotId == parkingSpotId &&
                                           ((startTime >= reservation.StartTime && startTime < reservation.EndTime) ||
                                            (endTime > reservation.StartTime && endTime <= reservation.EndTime) ||
                                            (startTime <= reservation.StartTime && endTime >= reservation.EndTime)));
            }
        }

        public class GetFilteredReservationsByUserId : Specification<Reservation>
        {
            public GetFilteredReservationsByUserId(string userId, string? globalQuery, int page, int pageSize)
            {
                Query.Where(reservation => reservation.UserId == userId);

                if (!string.IsNullOrEmpty(globalQuery))
                {
                    Query.Where(reservation =>
                        reservation.User.FullName.Contains(globalQuery) ||
                        reservation.User.Email.Contains(globalQuery) ||
                        reservation.ParkingSpot.Location.Contains(globalQuery) ||
                        reservation.ParkingSpot.ParkingLot.Name.Contains(globalQuery) ||
                        reservation.ParkingSpot.ParkingLot.Location.Contains(globalQuery));
                }

                Query.Include(reservation => reservation.User)
                     .Include(reservation => reservation.ParkingSpot)
                     .ThenInclude(spot => spot.ParkingLot);

                Query.Skip((page - 1) * pageSize).Take(pageSize);
            }
        }

        public class GetFilteredReservationsWithoutPaginationByUserId : Specification<Reservation>
        {
            public GetFilteredReservationsWithoutPaginationByUserId(string userId, string? globalQuery)
            {
                Query.Where(reservation => reservation.UserId == userId);

                if (!string.IsNullOrEmpty(globalQuery))
                {
                    Query.Where(reservation =>
                        reservation.User.FullName.Contains(globalQuery) ||
                        reservation.User.Email.Contains(globalQuery) ||
                        reservation.ParkingSpot.Location.Contains(globalQuery) ||
                        reservation.ParkingSpot.ParkingLot.Name.Contains(globalQuery) ||
                        reservation.ParkingSpot.ParkingLot.Location.Contains(globalQuery));
                }

                Query.Include(reservation => reservation.User)
                     .Include(reservation => reservation.ParkingSpot)
                     .ThenInclude(spot => spot.ParkingLot);
            }
        }


    }


}
