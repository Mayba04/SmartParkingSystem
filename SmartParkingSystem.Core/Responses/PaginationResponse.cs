using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.Responses
{
    public class PaginationResponse<PayloadType, ErrorType> : ServiceResponse<PayloadType, ErrorType>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;
        public int CountPages { get; set; } = 0;
        public PaginationResponse(
            bool success = false, string message = "", PayloadType payload = default, IEnumerable<ErrorType> errors = default,
            int pageNumber = 1, int pageSize = 10, int totalCount = 0)
            : base(success: success, message: message, payload: payload, errors: errors)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            CountPages = (totalCount / pageSize) + 1;
        }
    }
}
