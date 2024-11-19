using AutoMapper;
using SmartParkingSystem.Core.DTOs.ParkingSpot;
using SmartParkingSystem.Core.DTOs.Sensor;
using SmartParkingSystem.Core.Entities;
using SmartParkingSystem.Core.Interfaces;
using SmartParkingSystem.Core.Responses;
using SmartParkingSystem.Core.Specifications;
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
        private readonly ISensorService _sensorService;


        public ParkingSpotService(IRepository<ParkingSpot> parkingSpotRepo, IMapper mapper, ISensorService sensorService)
        {
            _parkingSpotRepo = parkingSpotRepo;
            _mapper = mapper;
            _sensorService = sensorService;
        }

        public async Task<ServiceResponse<ParkingSpotDTO, object>> AddAsync(ParkingSpotCreateDTO parkingSpotCreateDTO)
        {
            var sensorCreateDTO = new SensorCreateDTO
            {
                Status = "None",
                Type = "Default",
                LastActiveDistance = 0.0 
            };

            var sensorResponse = await _sensorService.AddAsync(sensorCreateDTO);

            if (!sensorResponse.Success)
            {
                return new ServiceResponse<ParkingSpotDTO, object>(false, "Failed to create sensor", errors: sensorResponse.Errors);
            }

            var lastSensor = await _sensorService.GetLastInsertedSensorAsync();
            if (lastSensor == null)
            {
                return new ServiceResponse<ParkingSpotDTO, object>(false, "Failed to get last sensor");
            }

            var parkingSpotEntity = _mapper.Map<ParkingSpotCreateDTO, ParkingSpot>(parkingSpotCreateDTO);
            parkingSpotEntity.SensorId = lastSensor.Id;

            await _parkingSpotRepo.Insert(parkingSpotEntity);
            await _parkingSpotRepo.Save();

            return new ServiceResponse<ParkingSpotDTO, object>(
                true,
                "Parking spot created successfully",
                payload: _mapper.Map<ParkingSpotDTO>(parkingSpotEntity)
            );
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

        public async Task<PaginationResponse<List<ParkingSpotDTO>, object>> GetPagedParkingSpotsAsync(string? location, int page, int pageSize)
        {
            var parkingSpots = await _parkingSpotRepo.GetListBySpec(new ParkingSpotSpecification.GetByLocationAndPagination(location, page, pageSize));
            var totalSpots = string.IsNullOrEmpty(location)
                ? await _parkingSpotRepo.GetCountRows()
                : await _parkingSpotRepo.GetCountBySpec(new ParkingSpotSpecification.GetByLocation(location));

            return new PaginationResponse<List<ParkingSpotDTO>, object>(
                true,
                "Parking spots retrieved successfully.",
                payload: _mapper.Map<List<ParkingSpotDTO>>(parkingSpots),
                pageNumber: page,
                pageSize: pageSize,
                totalCount: totalSpots
            );
        }

        public async Task<PaginationResponse<List<ParkingSpotDTO>, object>> GetPagedParkingSpotsByParkingLotAsync(int parkingLotId, string? location, int page, int pageSize)
        {
            var parkingSpots = await _parkingSpotRepo.GetListBySpec(new ParkingSpotSpecification.GetByParkingLotAndLocation(parkingLotId, location, page, pageSize));
            var totalSpots = string.IsNullOrEmpty(location)
                ? await _parkingSpotRepo.GetCountBySpec(new ParkingSpotSpecification.GetByParkingLot(parkingLotId))
                : await _parkingSpotRepo.GetCountBySpec(new ParkingSpotSpecification.GetByParkingLotAndLocationWithoutPagination(parkingLotId, location));

            return new PaginationResponse<List<ParkingSpotDTO>, object>(
                true,
                "Parking spots retrieved successfully.",
                payload: _mapper.Map<List<ParkingSpotDTO>>(parkingSpots),
                pageNumber: page,
                pageSize: pageSize,
                totalCount: totalSpots
            );
        }


        public async Task<ServiceResponse<List<ParkingSpotDTO>, object>> GetAllByParkingLotAsync(int parkingLotId)
        {
            var parkingSpots = await _parkingSpotRepo.GetListBySpec(new ParkingSpotSpecification.GetByParkingLot(parkingLotId));
            return new ServiceResponse<List<ParkingSpotDTO>, object>(true, "Parking spots retrieved successfully.", payload: _mapper.Map<List<ParkingSpotDTO>>(parkingSpots));
        }
     
    }
}
