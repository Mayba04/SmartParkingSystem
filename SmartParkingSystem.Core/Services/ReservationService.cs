using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartParkingSystem.Core.DTOs.Reservation;
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
    public class ReservationService : IReservationService
    {
        private readonly IRepository<Reservation> _reservationRepo;
        private readonly IMapper _mapper;

        public ReservationService(IRepository<Reservation> reservationRepo, IMapper mapper)
        {
            _reservationRepo = reservationRepo;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<ReservationDTO, object>> AddAsync(ReservationCreateDTO model)
        {
            var reservationEntity = _mapper.Map<ReservationCreateDTO, Reservation>(model);

            var conflictingReservations = await _reservationRepo.GetListBySpec(new ReservationSpecification.GetByParkingSpotAndTime(
                reservationEntity.ParkingSpotId,
                reservationEntity.StartTime,
                reservationEntity.EndTime
            ));

            if (conflictingReservations.Any())
            {
                return new ServiceResponse<ReservationDTO, object>(
                    false,
                    "Cannot create reservation. The parking spot is already reserved during the selected time period."
                );
            }

            await _reservationRepo.Insert(reservationEntity);
            await _reservationRepo.Save();

            return new ServiceResponse<ReservationDTO, object>(
                true,
                "Reservation created successfully",
                payload: _mapper.Map<ReservationDTO>(reservationEntity)
            );
        }


        public async Task<ServiceResponse<ReservationDTO, object>> UpdateAsync(ReservationUpdateDTO model)
        {
            var reservationEntity = _mapper.Map<ReservationUpdateDTO, Reservation>(model);
            await _reservationRepo.Update(reservationEntity);
            await _reservationRepo.Save();
            return new ServiceResponse<ReservationDTO, object>(true, "Reservation updated successfully", payload: _mapper.Map<ReservationDTO>(reservationEntity));
        }

        public async Task<ServiceResponse<object, object>> DeleteAsync(int id)
        {
            var reservation = await _reservationRepo.GetByID(id);
            if (reservation == null)
            {
                return new ServiceResponse<object, object>(false, "Reservation not found");
            }
            await _reservationRepo.Delete(id);
            await _reservationRepo.Save();
            return new ServiceResponse<object, object>(true, "Reservation deleted successfully");
        }

        public async Task<ServiceResponse<ReservationDTO, object>> GetByIdAsync(int id)
        {
            var reservation = await _reservationRepo.GetByID(id);
            if (reservation == null)
            {
                return new ServiceResponse<ReservationDTO, object>(false, "Reservation not found");
            }
            return new ServiceResponse<ReservationDTO, object>(true, "", payload: _mapper.Map<ReservationDTO>(reservation));
        }

        public async Task<ServiceResponse<IEnumerable<ReservationDTO>, object>> GetAllAsync()
        {
            var reservations = await _reservationRepo.GetAll();
            return new ServiceResponse<IEnumerable<ReservationDTO>, object>(true, "", payload: _mapper.Map<IEnumerable<ReservationDTO>>(reservations));
        }

        public async Task<PaginationResponse<List<ReservationExtendedDTO>, object>> GetPagedReservationsAsync(int page, int pageSize, string? globalQuery, DateTime? startDate, DateTime? endDate)
        {
            DateTime? utcStartDate = startDate?.ToUniversalTime();
            DateTime? utcEndDate = endDate?.ToUniversalTime();

            var reservations = await _reservationRepo.GetListBySpec(new ReservationSpecification.GetFilteredReservations(globalQuery, utcStartDate, utcEndDate, page, pageSize));
            var totalReservations = await _reservationRepo.GetCountBySpec(new ReservationSpecification.GetFilteredReservationsWithoutPagination(globalQuery, utcStartDate, utcEndDate));

            return new PaginationResponse<List<ReservationExtendedDTO>, object>(
                true,
                "Reservations retrieved successfully.",
                payload: _mapper.Map<List<ReservationExtendedDTO>>(reservations),
                pageNumber: page,
                pageSize: pageSize,
                totalCount: totalReservations
            );
        }

        public async Task<PaginationResponse<List<ReservationExtendedDTO>, object>> GetPagedReservationsByUserIdAsync(string userId, int page, int pageSize, string? globalQuery)
        {
            var reservations = await _reservationRepo.GetListBySpec(new ReservationSpecification.GetFilteredReservationsByUserId(userId, globalQuery, page, pageSize));
            var totalReservations = await _reservationRepo.GetCountBySpec(new ReservationSpecification.GetFilteredReservationsWithoutPaginationByUserId(userId, globalQuery));

            return new PaginationResponse<List<ReservationExtendedDTO>, object>(
                true,
                "Reservations retrieved successfully.",
                payload: _mapper.Map<List<ReservationExtendedDTO>>(reservations),
                pageNumber: page,
                pageSize: pageSize,
                totalCount: totalReservations
            );
        }



    }

}

