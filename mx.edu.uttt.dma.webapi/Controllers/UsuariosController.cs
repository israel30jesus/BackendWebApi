﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mx.edu.uttt.dma.webapi.DTOs;
using mx.edu.uttt.dma.webapi.Entidades;
using mx.edu.uttt.dma.webapi.Services;

namespace mx.edu.uttt.dma.webapi.Controllers
{
    [ApiController]
    //Ruta de acceso o url de acceso
    [Route("webapi/usuarios")]
    //[Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEncriptacionService _encriptacionService;

        public UsuariosController(ApplicationDbContext context,
            IMapper mapper, IEncriptacionService encriptacionService)
        {
            this._context = context;
            this._mapper = mapper;
            this._encriptacionService = encriptacionService;
        }
        // Todos los usuarios
        [HttpGet]
        public async Task<ActionResult<List<UsuarioDTO>>> GetUsers()
        {
            var entidades = await _context.Usuarios.ToListAsync();
            var dtos = _mapper.Map<List<UsuarioDTO>>(entidades);
            //Console.WriteLine(dtos);
            return dtos;
        }
        // Usuario por nombre
        [HttpGet("{usuario}", Name = "obtenerNombreUsuario")]
        public async Task<ActionResult<UsuarioDTO>> GetUser(string usuario)
        {
            var entidad = await _context.Usuarios.FirstOrDefaultAsync(x => x.UsuarioNombre == usuario);
            
            if (entidad == null)
            {
                return NotFound();
            }
            
            return _mapper.Map<UsuarioDTO>(entidad);
        }
        // Usuario por Id
        [HttpGet("{id:int}", Name = "obtenerUsuario")]
        public async Task<ActionResult<UsuarioDTO>> GetUserById(int id)
        {
            try
            {
                var entidad = await _context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == id);

                if (entidad == null)
                {
                    return NotFound();
                }

                return _mapper.Map<UsuarioDTO>(entidad);
            }
            catch (Exception ex)
            {
                return BadRequest("Algo salio mal");
            }
        }
        // Agregar nuevo usuario (Registro)
        
        [HttpPost]
        [Route("registro")]
        public async Task<ActionResult> PostUser(UsuarioCreacionDTO model)
        {
            //var entidadDos = await _context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == 3);
            //_mapper.Map<UsuarioDTO>(entidadDos);

            var encriptacion = _encriptacionService.Encryptword(model.Contrasena);
            var entidad = _mapper.Map<Usuario>(model);
            entidad.Contrasena = encriptacion;
            _context.Add(entidad);
            await _context.SaveChangesAsync();
            var usuarioDTO = _mapper.Map<UsuarioDTO>(entidad);
            return new CreatedAtRouteResult("obtenerUsuario", new { id = usuarioDTO.IdUsuario}, usuarioDTO);
        }
        //Actualizar Usuario
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateUser(int id, UsuarioActualizarDTO model)
        {
            var existe = await _context.Usuarios.AnyAsync(x => x.IdUsuario == id);
            
            if (!existe)
            {
                return NotFound();
            }
            var encriptacion = _encriptacionService.Encryptword(model.Contrasena);
            var entidad = _mapper.Map<Usuario>(model);
            entidad.Contrasena = encriptacion;
            //entidad.token = "Token Secreto";
            entidad.IdUsuario = id;
            _context.Entry(entidad).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("Usuario Actualizado");
        }
        //Eliminar Usuario
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var existe = await _context.Usuarios.AnyAsync(x => x.IdUsuario == id);

            if (!existe)
            {
                return NotFound();
            }
            _context.Remove(new Usuario() { IdUsuario = id });
            await _context.SaveChangesAsync();

            return Ok("Usuario Eliminado");
        }
        // Agregar nuevo usuario (Registro)
        /*
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> UserLogin(UsuarioLoginDTO model)
        {
            var encriptacion = _encriptacionService.Encryptword(model.Contrasena);
            var existe = await _context.Usuarios.FirstOrDefaultAsync(x => x.UsuarioNombre == model.UsuarioNombre
                                                                     && x.Contrasena == encriptacion);
            if (existe != null)
            {
                return Ok("Acceso Correcto");
            }
            return Ok("El usuario no existe");
        }
        */
    }
}
