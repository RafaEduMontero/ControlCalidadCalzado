﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace CapaDatos.Models
{
    public partial class Modelo
    {
        public Modelo()
        {
            OrdenProduccion = new HashSet<OrdenProduccion>();
        }

        public int IdModelo { get; set; }
        public string Sku { get; set; }
        public string Denominacion { get; set; }
        public string LimiteSuperiorReproceso { get; set; }
        public string LimiteInferiorReproceso { get; set; }
        public string LimiteSuperiorObservado { get; set; }
        public string LimiteInferiorObservado { get; set; }

        public virtual ICollection<OrdenProduccion> OrdenProduccion { get; set; }
    }
}