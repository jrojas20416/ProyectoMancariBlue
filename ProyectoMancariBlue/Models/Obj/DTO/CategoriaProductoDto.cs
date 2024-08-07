﻿using Newtonsoft.Json;
using System.ComponentModel;

namespace ProyectoMancariBlue.Models.Obj.DTO
{
    public class CategoriaProductoDto
    {
        public int Id { get; set; }
        [DisplayName("Categoría")]
        public string Descripcion { get; set; }
        [JsonIgnore]

        public ICollection<ProductoDto> Productos { get; set; }
    }
}
