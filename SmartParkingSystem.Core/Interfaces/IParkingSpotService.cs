using SmartParkingSystem.Core.DTOs.ParkingSpot;
using SmartParkingSystem.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.Interfaces
{
    public interface IParkingSpotService
    {
        Task<ServiceResponse<ParkingSpotDTO, object>> AddAsync(ParkingSpotCreateDTO model);
        Task<ServiceResponse<ParkingSpotDTO, object>> UpdateAsync(ParkingSpotUpdateDTO model);
        Task<ServiceResponse<object, object>> DeleteAsync(int id);
        Task<ServiceResponse<ParkingSpotDTO, object>> GetByIdAsync(int id);
        Task<ServiceResponse<IEnumerable<ParkingSpotDTO>, object>> GetAllAsync();
    }
}
