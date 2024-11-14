using Ardalis.Specification;
using SmartParkingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SmartParkingSystem.Core.Specifications
{
    public static class RefreshTokenSpecification
    {
        public class GetRefreshToken : Specification<RefreshToken>
        {
            public GetRefreshToken(string refreshToken)
            {
                Query.Where(t => t.Token == refreshToken);
            }
        }
        public class GetAllTokensByUserId : Specification<RefreshToken>
        {
            public GetAllTokensByUserId(string userId)
            {
                Query.Where(t => t.UserId == userId);
            }
        }
    }
}
