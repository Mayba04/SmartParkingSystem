using AutoMapper;
using SmartParkingSystem.Core.DTOs.Sensor;
using SmartParkingSystem.Core.Entities;
using SmartParkingSystem.Core.Interfaces;
using SmartParkingSystem.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.Services
{
    public class SensorService : ISensorService
    {
        private readonly IRepository<Sensor> _sensorRepo;
        private readonly IMapper _mapper;

        public SensorService(IRepository<Sensor> sensorRepo, IMapper mapper)
        {
            _sensorRepo = sensorRepo;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<SensorDTO, object>> AddAsync(SensorCreateDTO model)
        {
            var sensorEntity = _mapper.Map<SensorCreateDTO, Sensor>(model);
            await _sensorRepo.Insert(sensorEntity);
            await _sensorRepo.Save();
            return new ServiceResponse<SensorDTO, object>(true, "Sensor created successfully", payload: _mapper.Map<SensorDTO>(sensorEntity));
        }

        public async Task<ServiceResponse<SensorDTO, object>> UpdateAsync(SensorUpdateDTO model)
        {
            var sensorEntity = _mapper.Map<SensorUpdateDTO, Sensor>(model);
            await _sensorRepo.Update(sensorEntity);
            await _sensorRepo.Save();
            return new ServiceResponse<SensorDTO, object>(true, "Sensor updated successfully", payload: _mapper.Map<SensorDTO>(sensorEntity));
        }

        public async Task<ServiceResponse<object, object>> DeleteAsync(int id)
        {
            var sensor = await _sensorRepo.GetByID(id);
            if (sensor == null)
            {
                return new ServiceResponse<object, object>(false, "Sensor not found");
            }
            await _sensorRepo.Delete(id);
            await _sensorRepo.Save();
            return new ServiceResponse<object, object>(true, "Sensor deleted successfully");
        }

        public async Task<ServiceResponse<SensorDTO, object>> GetByIdAsync(int id)
        {
            var sensor = await _sensorRepo.GetByID(id);
            if (sensor == null)
            {
                return new ServiceResponse<SensorDTO, object>(false, "Sensor not found");
            }
            return new ServiceResponse<SensorDTO, object>(true, "", payload: _mapper.Map<SensorDTO>(sensor));
        }

        public async Task<ServiceResponse<IEnumerable<SensorDTO>, object>> GetAllAsync()
        {
            var sensors = await _sensorRepo.GetAll();
            return new ServiceResponse<IEnumerable<SensorDTO>, object>(true, "", payload: _mapper.Map<IEnumerable<SensorDTO>>(sensors));
        }
    }
}
