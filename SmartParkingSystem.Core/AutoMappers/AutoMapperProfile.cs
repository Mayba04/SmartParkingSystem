using AutoMapper;
using SmartParkingSystem.Core.DTOs.ParkingLot;
using SmartParkingSystem.Core.DTOs.ParkingSpot;
using SmartParkingSystem.Core.DTOs.Reservation;
using SmartParkingSystem.Core.DTOs.Sensor;
using SmartParkingSystem.Core.DTOs.User;
using SmartParkingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.AutoMappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ParkingSpotCreateDTO, ParkingSpot>().ReverseMap();
            CreateMap<ParkingSpotUpdateDTO, ParkingSpot>().ReverseMap();
            CreateMap<ParkingSpotDTO, ParkingSpot>().ReverseMap();

            CreateMap<ParkingLotCreateDTO, ParkingLot>().ReverseMap();
            CreateMap<ParkingLotUpdateDTO, ParkingLot>().ReverseMap();
            CreateMap<ParkingLotDTO, ParkingLot>().ReverseMap();

            CreateMap<ReservationCreateDTO, Reservation>().ReverseMap();
            CreateMap<ReservationUpdateDTO, Reservation>().ReverseMap();
            CreateMap<ReservationDTO, Reservation>().ReverseMap();

            CreateMap<SensorCreateDTO, Sensor>().ReverseMap();
            CreateMap<SensorUpdateDTO, Sensor>().ReverseMap();
            CreateMap<SensorDTO, Sensor>().ReverseMap();

            //CreateMap<CreateUserDTO, AppUser>().ReverseMap();
            //CreateMap<DeleteUserDTO, AppUser>().ReverseMap();
            //CreateMap<EditUserDTO, AppUser>().ReverseMap();
            //CreateMap<UpdateUserDTO, AppUser>().ReverseMap();
            CreateMap<UserDTO, AppUser>().ReverseMap();
            //CreateMap<UserSignUpEmailDTO, AppUser>().ReverseMap();
            //CreateMap<RegistrationUserDTO, CreateUserDTO>().ReverseMap();

        }
    }
}
