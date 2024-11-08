using SmartParkingSystem.Core.DTOs.Sensor;
using SmartParkingSystem.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.Interfaces
{
    public interface ISensorService
    {
        Task<ServiceResponse<SensorDTO, object>> AddAsync(SensorCreateDTO model);
        Task<ServiceResponse<SensorDTO, object>> UpdateAsync(SensorUpdateDTO model);
        Task<ServiceResponse<object, object>> DeleteAsync(int id);
        Task<ServiceResponse<SensorDTO, object>> GetByIdAsync(int id);
        Task<ServiceResponse<IEnumerable<SensorDTO>, object>> GetAllAsync();
    }
}
