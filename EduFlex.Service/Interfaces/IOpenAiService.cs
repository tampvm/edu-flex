﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Interfaces
{
    public interface IOpenAIService
    {
        Task<string> GeneratePlanbookFieldAsync(string prompt);    
    }
}
