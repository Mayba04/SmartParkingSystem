using AutoMapper;
using SmartParkingSystem.Core.DTOs.Reservation;
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
            await _reservationRepo.Insert(reservationEntity);
            await _reservationRepo.Save();
            return new ServiceResponse<ReservationDTO, object>(true, "Reservation created successfully", payload: _mapper.Map<ReservationDTO>(reservationEntity));
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
    }
}
