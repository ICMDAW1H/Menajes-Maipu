//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Menajes_Maipu.Models.ModeloBD
{
    using System;
    using System.Collections.Generic;
    
    public partial class Despacho
    {
        public int Num_seguimiento { get; set; }
        public string Estado_despacho { get; set; }
        public string Fecha_envio { get; set; }
        public string Fecha_entrega { get; set; }
        public Nullable<int> Id_ventas { get; set; }
        public string Calle { get; set; }
        public Nullable<int> Numero { get; set; }
        public string Comuna { get; set; }
        public string Ciudad { get; set; }
        public string Cod_postal { get; set; }
        public Nullable<int> Num_departamento { get; set; }
        public int Id_despacho { get; set; }
    
        public virtual Ventas Ventas { get; set; }
    }
}
