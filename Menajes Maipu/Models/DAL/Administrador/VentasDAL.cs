using Menajes_Maipu.Controllers;
using Menajes_Maipu.Models.ModeloBD;
using Menajes_Maipu.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menajes_Maipu.Models.DAL.Administrador
{
    class VentasDAL
    {
        MenajesbdEntities6 ctx = new MenajesbdEntities6();
        Ventas venta = new Ventas();
        Despacho despacho = new Despacho();
        Metodo_pago metodo_pago = new Metodo_pago();
        Carrito carro = new Carrito();
        Debito debito = new Debito();
        public static readonly log4net.ILog log = LogHelper.GetLogger();
        public bool Ingresar(int id_user, List<Item> cart, Despacho d, Metodo_pago metodo, string Total_venta)
        {

            string fecha = DateTime.Now.ToString("MM-dd-yyyy/HH:mm:ss:fff");
            string hora = DateTime.Now.ToString("MM-dd-yyyy");


            try
            {
                venta.id_user = id_user;
                venta.Total_venta = Int32.Parse(Total_venta);
                venta.Fecha_venta = fecha;
                venta.Hora_venta = hora;
                venta.Estado = "INGRESADO";
                ctx.Ventas.Add(venta);
                ctx.SaveChanges();
                int id = (from o in ctx.Ventas
                          where o.Fecha_venta == fecha
                          select o.Id_ventas).SingleOrDefault();

                foreach (var item in cart)
                {
                    carro.Id_venta = id;
                    carro.Id_producto = item.p.Id_producto;
                    carro.Precio_p = item.p.Precio_producto;
                    carro.Cantidad = item.Quantity;
                    ctx.Carrito.Add(carro);
                    ctx.SaveChanges();
                }
                metodo_pago.Id_ventas = id;
                metodo_pago.Tipo_mpago = metodo.Tipo_mpago;
                ctx.Metodo_pago.Add(metodo_pago);
                ctx.SaveChanges();
        
                despacho.Id_ventas = metodo_pago.Id_ventas;
                despacho.Num_seguimiento = 0;
                despacho.Estado_despacho = d.Estado_despacho;
                despacho.Fecha_envio = d.Fecha_envio;
                despacho.Fecha_entrega = d.Fecha_entrega;
                despacho.Calle = d.Calle;
                despacho.Numero = d.Numero;
                despacho.Comuna = d.Comuna;
                despacho.Ciudad = d.Ciudad;
                despacho.Cod_postal = d.Cod_postal;
                despacho.Num_departamento = d.Num_departamento;
                ctx.Despacho.Add(despacho);
                ctx.SaveChanges();
              
            
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Ingresar-VentasDAL " + ex.Message);
                return false;
            }
        }
        public bool Ingresardebito(int id_user, List<Item> cart, Despacho d, Metodo_pago metodo, string Total_venta)
        {

            string fecha = DateTime.Now.ToString("MM-dd-yyyy/HH:mm:ss:fff");
            string hora = DateTime.Now.ToString("MM-dd-yyyy");
            

            try
            {
        
                venta.id_user = id_user;
                venta.Total_venta = Int32.Parse(Total_venta);
                venta.Fecha_venta = fecha;
                venta.Hora_venta = hora;
                venta.Estado = "INGRESADO";


                ctx.Ventas.Add(venta);
                ctx.SaveChanges();
        

                int id = (from o in ctx.Ventas
                          where o.Fecha_venta == fecha
                          select o.Id_ventas).SingleOrDefault();

                debito.Id_ventas = id;
                ctx.Debito.Add(debito);
                ctx.SaveChanges();
                foreach (var item in cart)
                {
                    carro.Id_venta = id;
                    carro.Id_producto = item.p.Id_producto;
                    carro.Precio_p = item.p.Precio_producto;
                    carro.Cantidad = item.Quantity;
                    ctx.Carrito.Add(carro);
                    ctx.SaveChanges();
                }
                metodo_pago.Id_ventas = id;
                metodo_pago.Tipo_mpago = metodo.Tipo_mpago;
                ctx.Metodo_pago.Add(metodo_pago);
                ctx.SaveChanges();

                despacho.Id_ventas = metodo_pago.Id_ventas;
                despacho.Num_seguimiento = 0;
                despacho.Estado_despacho = d.Estado_despacho;
                despacho.Fecha_envio = d.Fecha_envio;
                despacho.Fecha_entrega = d.Fecha_entrega;
                despacho.Calle = d.Calle;
                despacho.Numero = d.Numero;
                despacho.Comuna = d.Comuna;
                despacho.Ciudad = d.Ciudad;
                despacho.Cod_postal = d.Cod_postal;
                despacho.Num_departamento = d.Num_departamento;
                ctx.Despacho.Add(despacho);
                ctx.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<Ventas> ListarVentas_usuario(int id_user)
        {
            var lista =
            ctx.Ventas.Where(c => c.id_user == id_user).OrderByDescending(c => c.Fecha_venta).ToList();
            return lista;
        }

        public List<Producto> Listarpindex()
        {

            var lista = (from o in ctx.Producto
                         select new
                         {
                             o.Id_producto,
                             o.Imagen_producto,
                             o.Nombre_producto,
                             o.Precio_producto
                         });
            var lista2 = (from o in lista.AsEnumerable()
                          select new Producto
                          {
                              Id_producto = o.Id_producto,
                              Imagen_producto = o.Imagen_producto,
                              Nombre_producto = o.Nombre_producto,
                              Precio_producto = o.Precio_producto
                          });
            return lista2.ToList();
        }

        public List<VentaDetalle> ListarVentaPedido(int id_user, int Id_venta)
        {
            var lista = (from u in ctx.Usuario
                         from p in ctx.Producto
                         from v in ctx.Ventas
                         from m in ctx.Metodo_pago
                         from d in ctx.Despacho
                         from c in ctx.Carrito
                         from b in ctx.Banco_menaje
                          
            where 
                         u.id_user == v.id_user &&
                         v.Id_ventas == m.Id_ventas &&
                         m.Id_ventas == d.Id_ventas &&
                         d.Id_ventas == c.Id_venta &&
                         p.Id_producto == c.Id_producto &&
                         u.id_user == id_user &&
                         v.Id_ventas == Id_venta
           select new VentaDetalle{usuario = u, producto = p, ventas = v, metodo_pago = m, despacho = d, carrito = c, banco_menaje = b}).ToList();
           return lista;
        }

        public List<Ventas> ListarVentas()
        {
            var lista = ctx.Ventas.Select(c => c).OrderByDescending(c => c.Id_ventas).ToList();
            return lista;
        }

        public bool Modificar(int Id_ventas, string Estado_venta, string Estado_despacho, string Fecha_envio, string Fecha_entrega)
        {
            try
            {
                if (Estado_venta != "SELECCIONE")
                {
                    var ventas = ctx.Ventas.Where(c => c.Id_ventas == Id_ventas).SingleOrDefault();

                    ventas.Estado = Estado_venta;
                    ctx.SaveChanges();
                }
                var despacho = ctx.Despacho.Where(c => c.Id_ventas == Id_ventas).SingleOrDefault();

                if (Estado_despacho != "SELECCIONE")
                {

                    despacho.Estado_despacho = Estado_despacho;
                    ctx.SaveChanges();

                }

                if (Fecha_entrega != "")
                {
                    despacho.Fecha_entrega = Fecha_entrega;
                    ctx.SaveChanges();
                }
                if (Fecha_envio != "")
                {
                    despacho.Fecha_envio = Fecha_envio;
                    ctx.SaveChanges();

                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }






























    }


}
