﻿namespace ProyectoMancariBlue.Models.Obj
{
    public class CategoriaProducto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }

        public ICollection<Producto> Productos { get; set; }
    }
}
