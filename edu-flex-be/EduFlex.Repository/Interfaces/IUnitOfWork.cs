﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangeAsync();
    }
}