using SmartParkingSystem.Core.DTOs.ParkingLot;
using SmartParkingSystem.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.Interfaces
{
    public interface IParkingLotService
    {
        Task<ServiceResponse<ParkingLotDTO, object>> AddAsync(ParkingLotCreateDTO model);
        Task<ServiceResponse<ParkingLotDTO, object>> UpdateAsync(ParkingLotUpdateDTO model);
        Task<ServiceResponse<object, object>> DeleteAsync(int id);
        Task<ServiceResponse<ParkingLotDTO, object>> GetByIdAsync(int id);
        Task<ServiceResponse<IEnumerable<ParkingLotDTO>, object>> GetAllAsync();
        Task<PaginationResponse<List<ParkingLotDTO>, object>> GetPagedAsync(string? searchTerm = null, int page = 1, int pageSize = 10);
    }
}
