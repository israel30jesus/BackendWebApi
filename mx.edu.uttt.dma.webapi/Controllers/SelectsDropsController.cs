﻿using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using mx.edu.uttt.dma.webapi.Services;

namespace mx.edu.uttt.dma.webapi.Controllers
{
    [ApiController]
    //Ruta de acceso o url de acceso
    [Route("webapi/selects")]
    public class SelectsDropsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEncriptacionService _encriptacionService;
        public SelectsDropsController(ApplicationDbContext context,
            IMapper mapper, IEncriptacionService encriptacionService)
        {
            this._context = context;
            this._mapper = mapper;
            this._encriptacionService = encriptacionService;
        }
    }
}
