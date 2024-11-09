using EduFlex.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Utils
{
    public class TimeService : ITimeService
    {
        public DateTime GetCurrentTime() => DateTime.UtcNow.ToLocalTime();
    }
}
