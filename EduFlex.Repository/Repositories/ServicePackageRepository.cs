using EduFlex.Domain.Entities;
using EduFlex.Repository.Data;
using EduFlex.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Repository.Repositories
{
    public class ServicePackageRepository : GenericRepository<ServicePackage>, IServicePackageRepository
    {
        public ServicePackageRepository(AppDbContext dbContext, ITimeService timeService, IClaimsService claimsService)
            : base(dbContext, timeService, claimsService)
        {
        }

       
    }
}
