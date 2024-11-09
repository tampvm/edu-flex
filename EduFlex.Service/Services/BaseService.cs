using AutoMapper;
using EduFlex.Repository.Interfaces;
using EduFlex.Service.Common;
using EduFlex.Service.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Services
{
    public abstract class BaseService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly ITimeService _timeService;
        protected readonly IPasswordService _passwordHasher;
        protected readonly IEmailSender _emailSender;
        protected readonly ISmsSender _smsSender;
        protected readonly IMemoryCache _cache;
        protected readonly ITokenService _tokenService;

        public BaseService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ITimeService timeService,
            IPasswordService passwordHasher,
            IEmailSender emailSender,
            ISmsSender smsSender,
            IMemoryCache cache,
            ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _timeService = timeService;
            _passwordHasher = passwordHasher;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _cache = cache;
            _tokenService = tokenService;
        }
    }
}
