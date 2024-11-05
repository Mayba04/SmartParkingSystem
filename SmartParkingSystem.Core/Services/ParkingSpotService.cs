using AutoMapper;
using SmartParkingSystem.Core.DTOs.ParkingSpot;
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
    public class ParkingSpotService : IParkingSpotService
    {
        private readonly IRepository<ParkingSpot> _parkingSpotRepo;
        private readonly IMapper _mapper;

        public ParkingSpotService(IRepository<ParkingSpot> parkingSpotRepo, IMapper mapper)
        {
            _parkingSpotRepo = parkingSpotRepo;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<ParkingSpotDTO, object>> AddAsync(ParkingSpotCreateDTO parkingSpotCreateDTO)
        {
            var parkingSpotEntity = _mapper.Map<ParkingSpotCreateDTO, ParkingSpot>(parkingSpotCreateDTO);
            await _parkingSpotRepo.Insert(parkingSpotEntity);
            await _parkingSpotRepo.Save();
            return new ServiceResponse<ParkingSpotDTO, object>(true, "Parking spot created successfully", payload: _mapper.Map<ParkingSpotDTO>(parkingSpotEntity));
        }

        public async Task<ServiceResponse<ParkingSpotDTO, object>> GetByIdAsync(int id)
        {
            var parkingSpot = await _parkingSpotRepo.GetByID(id);
            if (parkingSpot == null)
            {
                return new ServiceResponse<ParkingSpotDTO, object>(false, "Parking spot not found");
            }
            return new ServiceResponse<ParkingSpotDTO, object>(true, "", payload: _mapper.Map<ParkingSpotDTO>(parkingSpot));
        }

        public async Task<ServiceResponse<IEnumerable<ParkingSpotDTO>, object>> GetAllAsync()
        {
            var parkingSpots = await _parkingSpotRepo.GetAll();
            return new ServiceResponse<IEnumerable<ParkingSpotDTO>, object>(true, "", payload: _mapper.Map<IEnumerable<ParkingSpotDTO>>(parkingSpots));
        }

        public async Task<ServiceResponse<object, object>> DeleteAsync(int id)
        {
            var parkingSpot = await _parkingSpotRepo.GetByID(id);
            if (parkingSpot == null)
            {
                return new ServiceResponse<object, object>(false, "Parking spot not found");
            }
            await _parkingSpotRepo.Delete(id);
            await _parkingSpotRepo.Save();
            return new ServiceResponse<object, object>(true, "Parking spot deleted successfully");
        }

        public async Task<ServiceResponse<ParkingSpotDTO, object>> UpdateAsync(ParkingSpotUpdateDTO parkingSpotUpdateDTO)
        {
            var parkingSpotEntity = _mapper.Map<ParkingSpotUpdateDTO, ParkingSpot>(parkingSpotUpdateDTO);
            await _parkingSpotRepo.Update(parkingSpotEntity);
            await _parkingSpotRepo.Save();
            return new ServiceResponse<ParkingSpotDTO, object>(true, "Parking spot updated successfully", payload: _mapper.Map<ParkingSpotDTO>(parkingSpotEntity));
        }
    }
}
