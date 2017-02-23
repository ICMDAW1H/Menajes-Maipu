using Menajes_Maipu.Models.ModeloBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menajes_Maipu.Models.DAL.Administrador
{
    public class VentaDetalle
    {
        // usuario
    public Usuario usuario{ get; set; }
    public Producto producto { get; set; }
    public Ventas  ventas{ get; set; }
    public Metodo_pago metodo_pago{ get; set; }
    public Carrito  carrito{ get; set; }
    public Banco_menaje banco_menaje{get; set;}
        public Despacho despacho { get; set; }


    
    }
}

