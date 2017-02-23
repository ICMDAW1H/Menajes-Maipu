using Menajes_Maipu.Controllers;
using Menajes_Maipu.Models.ModeloBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menajes_Maipu.Models.DAL.Administrador
{
    class CotizacionDAL
    {
        MenajesbdEntities6 ctx = new MenajesbdEntities6();
        public int Ingresar(int id_user, List<Item> cart, string Total_venta)
        {

            string fecha = DateTime.Now.ToString("MM-dd-yyyy/HH:mm:ss:fff");
            string hora = DateTime.Now.ToString("MM-dd-yyyy");
            try
            {
                Cotizacion cotizacion = new Cotizacion();
                cotizacion.id_user = id_user;
                cotizacion.Total_venta = Int32.Parse(Total_venta);
                cotizacion.Fecha_venta = fecha;
                cotizacion.Hora_venta = hora;
                cotizacion.Estado = "INGRESADO";
                ctx.Cotizacion.Add(cotizacion);
                ctx.SaveChanges();
                //ctx.Ventas.Where(v => v.Fecha_venta == fecha).ToList();

                int id = (from o in ctx.Cotizacion
                          where o.Fecha_venta == fecha
                          select o.Id_cotizacion).SingleOrDefault();

       
              
                Cotizacion_cart carro = new Cotizacion_cart();
                foreach (var item in cart)
                {
                    carro.Id_cotizacion = id;
                    carro.Id_producto = item.p.Id_producto;
                    carro.Precio_p = item.p.Precio_producto;
                    carro.Cantidad = item.Quantity;
                    ctx.Cotizacion_cart.Add(carro);
                    ctx.SaveChanges();
                }     
     

                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
