﻿using EduFlex.Domain.Entities;
using EduFlex.Repository.Data;
using EduFlex.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Repository.Repositories
{
    public class TeachingScheduleRepository : GenericRepository<TeachingSchedule>, ITeachingScheduleRepository
    {
        public TeachingScheduleRepository(AppDbContext dbContext, ITimeService timeService, IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
        }
    }
}