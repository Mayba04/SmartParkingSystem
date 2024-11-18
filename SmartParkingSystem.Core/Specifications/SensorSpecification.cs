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

    public class SensorSpecification : Specification<Sensor>
    {
        public class LastInsertedSensorSpecification : Specification<Sensor>
        {
            public LastInsertedSensorSpecification()
            {
                Query.OrderByDescending(s => s.Id).Take(1);
            }
        }
    }
}
