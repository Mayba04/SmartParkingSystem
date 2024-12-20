﻿using AutoMapper;
using SmartParkingSystem.Core.DTOs.ParkingLot;
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
    public class ParkingLotService : IParkingLotService
    {
        private readonly IRepository<ParkingLot> _parkingLotRepo;
        private readonly IMapper _mapper;

        public ParkingLotService(IRepository<ParkingLot> parkingLotRepo, IMapper mapper)
        {
            _parkingLotRepo = parkingLotRepo;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<ParkingLotDTO, object>> AddAsync(ParkingLotCreateDTO model)
        {
            var parkingLotEntity = _mapper.Map<ParkingLotCreateDTO, ParkingLot>(model);
            await _parkingLotRepo.Insert(parkingLotEntity);
            await _parkingLotRepo.Save();
            return new ServiceResponse<ParkingLotDTO, object>(true, "Parking lot created successfully", payload: _mapper.Map<ParkingLotDTO>(parkingLotEntity));
        }

        public async Task<ServiceResponse<ParkingLotDTO, object>> UpdateAsync(ParkingLotUpdateDTO model)
        {
            var parkingLotEntity = _mapper.Map<ParkingLotUpdateDTO, ParkingLot>(model);
            await _parkingLotRepo.Update(parkingLotEntity);
            await _parkingLotRepo.Save();
            return new ServiceResponse<ParkingLotDTO, object>(true, "Parking lot updated successfully", payload: _mapper.Map<ParkingLotDTO>(parkingLotEntity));
        }

        public async Task<ServiceResponse<object, object>> DeleteAsync(int id)
        {
            var parkingLot = await _parkingLotRepo.GetByID(id);
            if (parkingLot == null)
            {
                return new ServiceResponse<object, object>(false, "Parking lot not found");
            }
            await _parkingLotRepo.Delete(id);
            await _parkingLotRepo.Save();
            return new ServiceResponse<object, object>(true, "Parking lot deleted successfully");
        }

        public async Task<ServiceResponse<ParkingLotDTO, object>> GetByIdAsync(int id)
        {
            var parkingLot = await _parkingLotRepo.GetByID(id);
            if (parkingLot == null)
            {
                return new ServiceResponse<ParkingLotDTO, object>(false, "Parking lot not found");
            }
            return new ServiceResponse<ParkingLotDTO, object>(true, "", payload: _mapper.Map<ParkingLotDTO>(parkingLot));
        }

        public async Task<ServiceResponse<IEnumerable<ParkingLotDTO>, object>> GetAllAsync()
        {
            var parkingLots = await _parkingLotRepo.GetAll();
            return new ServiceResponse<IEnumerable<ParkingLotDTO>, object>(true, "", payload: _mapper.Map<IEnumerable<ParkingLotDTO>>(parkingLots));
        }

        public async Task<PaginationResponse<List<ParkingLotDTO>, object>> GetPagedAsync(string? searchTerm = null, int page = 1, int pageSize = 10)
        {
            try
            {
                var parkingLots = await _parkingLotRepo.GetListBySpec(new ParkingLotSpecification.GetByNameAndLocationWithPagination(searchTerm, page, pageSize));

                var totalParkingLots = string.IsNullOrEmpty(searchTerm)
                    ? await _parkingLotRepo.GetCountRows()
                    : await _parkingLotRepo.GetCountBySpec(new ParkingLotSpecification.GetByNameOrLocation(searchTerm));

                return new PaginationResponse<List<ParkingLotDTO>, object>(
                    true,
                    "Parking lots retrieved successfully.",
                    payload: _mapper.Map<List<ParkingLotDTO>>(parkingLots),
                    pageNumber: page,
                    pageSize: pageSize,
                    totalCount: totalParkingLots
                );
            }
            catch (Exception ex)
            {
                return new PaginationResponse<List<ParkingLotDTO>, object>(false, "Failed: " + ex.Message);
            }
        }

    }
}
