﻿using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using mx.edu.uttt.dma.webapi.Validaciones;

namespace mx.edu.uttt.dma.webapi.DTOs
{
    public class PostCreacionDTO
    {
        [Required]
        [StringLength(50)]
        public string Titulo { get; set; }
        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }
        public string MeGusta { get; set; }
        [ValidacionPesoImagen(PesoMaximoMegaBytes: 4)]
        [TipoArchivoValidacion(tipoArchivo: TipoArchivo.Imagen)]
        public IFormFile Imagen { get; set; }
        //Relacion de Tabla
        public int IdGenero { get; set; }
        //Relacion de Tabla
        public int IdUsuario { get; set; }
    }
}
