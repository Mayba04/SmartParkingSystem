using SmartParkingSystem.Core.DTOs.Reservation;
using SmartParkingSystem.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.Interfaces
{
    public interface IReservationService
    {
        Task<ServiceResponse<ReservationDTO, object>> AddAsync(ReservationCreateDTO model);
        Task<ServiceResponse<ReservationDTO, object>> UpdateAsync(ReservationUpdateDTO model);
        Task<ServiceResponse<object, object>> DeleteAsync(int id);
        Task<ServiceResponse<ReservationDTO, object>> GetByIdAsync(int id);
        Task<ServiceResponse<IEnumerable<ReservationDTO>, object>> GetAllAsync();
        Task<PaginationResponse<List<ReservationExtendedDTO>, object>> GetPagedReservationsAsync(int page, int pageSize, string? globalQuery, DateTime? startDate, DateTime? endDate);
    }
}
