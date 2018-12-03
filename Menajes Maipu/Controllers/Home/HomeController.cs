using Menajes_Maipu.Models.DAL.Administrador;
using Menajes_Maipu.Models.DAL.Home;
using Menajes_Maipu.Models.ModeloBD;
using System;
using System.Linq;
using System.Web.Mvc;
using CaptchaMvc.HtmlHelpers;
using PagedList;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Data.Entity;

namespace Menajes_Maipu.Controllers
{
    public class HomeController : Controller
    {
        private MenajesbdEntities6 db = new MenajesbdEntities6();
        private Contacto contacto = new Contacto();
        private VentasDAL DALventa = new VentasDAL();
        private CategoriaDAL DALcategoria = new CategoriaDAL();
        private SubcategoriaDAL DALsubcategoria = new SubcategoriaDAL();
        private ProductosDAL DALproducto = new ProductosDAL();
        private ContactoDAL DALcontacto = new ContactoDAL();
        private  UsuarioDAL DALusuario = new UsuarioDAL();
        private Metodo_pago MetodoPago = new Metodo_pago();
        Despacho Despacho = new Despacho();

        public ActionResult VerificaEstado(string estado)
        {
            try
            {
                if (estado == "1" || estado == "0")
                {
                    if (estado == "1")
                    {
                        ViewBag.estado = "1";
                    }
                    else
                    {
                        ViewBag.estado = "0";
                    }
                    return View();

                }
                return View();
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }
        }

        public bool ComparaDigitos(string id, string total)
        {
            if (!IsDigitsOnly(total) || !IsDigitsOnly(id))
            {
          
                return false;
            }
            return true;
        }
        //Inicio Comprar
        [Authorize(Roles = "admin,cliente")]

        public ActionResult Comprar(string id, string total)
        {
            if (ComparaDigitos(id,total))
            {
                return RedirectToAction("Index", "Home");
            }
            int precio_t = 0;
            if (Session["cart"] != null)
            {
                if (id == null)
                {
                    return RedirectToAction("Login", "Usuario");
                }     
                foreach (var item in (List<Item>)Session["cart"])
                {
                    string precio = item.P.Precio_producto.ToString();
                    precio_t = precio_t + (item.p.Precio_producto.Value * item.Quantity);
                }
                if (precio_t != Int32.Parse(total))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                { 
                ViewBag.Total = total;
                int ID = Int32.Parse(id);
                ViewBag.usuario = DALusuario.getUsuarioALLDATA(ID);
                return View();
                }
            }
            else
            {
                return View("Cart");
            }
        }
        //Fin Comprar


        // cotizacion

        public ActionResult Cotizar(string id, string total)
        {
            CotizacionDAL cotizacion = new CotizacionDAL();
            if (ComparaDigitos(id, total))
            {
                return RedirectToAction("Index", "Home");
            }
            int precio_t = 0;
            if (Session["cart"] != null)
            {
                if (id == null)
                {
                    return RedirectToAction("Login", "Usuario");
                }
                List<Item> cart = (List<Item>)Session["cart"];
                foreach (var item in cart)
                {
                    string precio = item.P.Precio_producto.ToString();
                    precio_t = precio_t + (item.p.Precio_producto.Value * item.Quantity);
                }
                if (precio_t != Int32.Parse(total))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Total = total;
                    int ID = Int32.Parse(id);
                    ViewBag.usuario = DALusuario.getUsuarioALLDATA(ID);
                    int id_cotizacion = cotizacion.Ingresar(ID, cart, total);
                    if (id_cotizacion != 0)
                    {
                        return RedirectToAction("ReporteCotizacion", "Home", new {id_user = ID, id_venta = id_cotizacion  });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                
                }
            }
            else
            {
                return View("Cart");
            }
        }
        // reporte cotizacion
        public ActionResult ReporteCotizacion(string id_user, string id_venta)
        {
            if (id_user == null || id_venta == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            else
            {

                LocalReport lr = new LocalReport();
                string path = Path.Combine(Server.MapPath("~/Reportes"), "DetalleCotizacion.rdlc");
                if (System.IO.File.Exists(path))
                {
                    lr.ReportPath = path;
                }
                else
                {
                    return View("Index");
                }
                List<Cotizacion_Detalle_Result> cm = new List<Cotizacion_Detalle_Result>();

                using (MenajesbdEntities6 dc = new MenajesbdEntities6())
                {
                    cm = dc.Cotizacion_Detalle(int.Parse(id_user), int.Parse(id_venta)).ToList();



                }
                ReportDataSource rd = new ReportDataSource("DataSet1", cm);
                string id = "PDF";
                lr.DataSources.Add(rd);
                string reportType = id;
                string mimeType;
                string encoding;
                string fileNameExtension;



                string deviceInfo =

                "<DeviceInfo>" +
                "  <OutputFormat>" + id + "</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>0.5in</MarginTop>" +
                "  <MarginLeft>0.5in</MarginLeft>" +
                "  <MarginRight>0.5in</MarginRight>" +
                "  <MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);


                return File(renderedBytes, mimeType);
            }
        }


        // fin reporte cotizacion 



        //Inicio Comprar POST
        [HttpPost]
        public ActionResult Comprar(FormCollection f)
        {


            ViewBag.Total = (f["Total_venta"]);
            int id = Int32.Parse(f["id_user"]);
            if (f["Tipo"] == null)
            {
                ViewBag.usuario = DALusuario.getUsuarioALLDATA(id);
                return View("Cart", new { id = 0 });
            }
            
           
            List<Item> cart = (List<Item>)Session["cart"];

      
            // VENTA EN TIENDA //
            //////////////////// 
            if (f["Tipo"] == "Tienda")
            {
                Despacho.Estado_despacho = "INGRESADO";
                Despacho.Fecha_entrega = "PENDIENTE";
                Despacho.Fecha_envio = "PENDIENTE";
                Despacho.Calle = "tienda";
       
                Despacho.Numero = 0;
                Despacho.Comuna = "tienda";
                Despacho.Ciudad = "tienda";
                Despacho.Cod_postal = "0";
                Despacho.Num_departamento = 0;
                MetodoPago.Tipo_mpago = f["Tipo"];
                ViewBag.id = Session["id"];
                ViewBag.usuario = DALusuario.getUsuarioALLDATA(id);
                DALventa.Ingresar(id, cart, Despacho, MetodoPago, f["Total_venta"]);
                ViewBag.Tipo = "IngresadoTienda";
                ViewBag.Ingresado = "Compra realizada con éxito";
                ViewBag.Estado = "Exito";
                Session["cart"] = null;
                return View();
            }





            if (f["Tipo"] == "Debito")
            {
                ViewBag.Tipo = "Debito";
                ViewBag.id = Session["id"];
                ViewBag.usuario = DALusuario.getUsuarioALLDATA(id);
                ViewBag.Ingresado = "laweaquesea";

                if (f["Debito"] == "Debito")
                {

                    Despacho.Estado_despacho = "INGRESADO";
                    Despacho.Fecha_entrega = "PENDIENTE";
                    Despacho.Fecha_envio = "PENDIENTE";
                    Despacho.Calle = f["Calle"];
                    Despacho.Numero = Int32.Parse(f["Numero"]);
                    Despacho.Comuna = f["Comuna"];
                    Despacho.Ciudad = f["Ciudad"];
                    Despacho.Cod_postal = f["Cod_postal"];
                    string depto =f["Num_departamento"];
                    if (depto == "")
                    {
                        Despacho.Num_departamento = 0;
                    }
                    else
                    {
                        Despacho.Num_departamento = Int32.Parse(depto);
                
                    }
                    MetodoPago.Tipo_mpago = f["Tipo"];

                    DALventa.Ingresardebito(id, cart, Despacho, MetodoPago, f["Total_venta"]);
                    Session["cart"] = null;
                    return RedirectToAction("Pedidos","Usuario", new { id, estado=1});
                }
                ViewBag.Total = (f["Total_venta"]);
                return View();


            }

            return View();
        }
        //Fin Comprar POST





        //Aumentar cantidad en el carrito si producto existe
        private int isExisting(int id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].p.Id_producto == id)
                    return i;
            }

            return (-1);
        }
        //FIN- Aumentar cantidad en el carrito si producto existe

        //Eliminar item del carrito
        public ActionResult Borrar(int id)
        {

            int index = isExisting(id);
            if (index == -1)
            {

                return View("Cart");
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                cart.RemoveAt(index);
                Session["cart"] = cart;
                return View("Cart");
            }

        }
        //Eliminar item del carrito


        //Restar item del carrito
        public ActionResult Remove(int id)
        {

            int index = isExisting(id);
            if (index == -1)
            {

                return View("Cart");
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                cart[index].Quantity--;
                Session["cart"] = cart;
                return View("Cart");
            }

        }
        //Eliminar item del carrito
        //Añadir al carrito
        public ActionResult Cart(string Id)
        {
            try
            {        
                if (!IsDigitsOnly(Id))
                {
                    return RedirectToAction("Index");
                }
                if (Id == null)
                {
                    return RedirectToAction("Index");
                }

                int id = Int32.Parse(Id);
                if (!db.Producto.Any(pro => pro.Id_producto == id)) 
                {
                    return View();
                }
                if (id != 0 || id == 0)
                {
                    if (Session["cart"] == null)
                    {
                        List<Item> cart = new List<Item>();
                        cart.Add(new Item(db.Producto.Find(id), 1));
                        Session["cart"] = cart;
                    }
                    else
                    {

                        List<Item> cart = (List<Item>)Session["cart"];
                        int index = isExisting(id);
                        if (index == -1)
                        {
                            cart.Add(new Item(db.Producto.Find(id), 1));
                        }
                        else
                        {
                            cart[index].Quantity++;
                        }
                        Session["cart"] = cart;
                    }
                    ViewBag.link = "#" + id.ToString();

                    return View();
                    
                }
                else
                {
         
                    return RedirectToAction("Cart");

                }
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }
          
        }

        // Fin Añadir al carrito



        // Inicio Obtener imagen
        public FileContentResult GetImage(int Id_producto)
        {
            Producto pro = db.Producto.FirstOrDefault(c => c.Id_producto == Id_producto);
            if (pro != null)
            {

                return File(pro.Imagen_producto, pro.Nombre_imagen);
            }
            else
            {
                return null;
            }
        }
        // Fin Obtener Imagen






        /// <summary>
        /// Seccion Vistas principales
        /// </summary>
        /// <returns>Links productos, Acerca de la empresa y Acerca de la empresa</returns>
        /// 


        //Listar- Pagina de inicio (Productos)
        public ActionResult Index(int? page)
        {
            ViewBag.Categoria = DALcategoria.Listarcindex();

            ViewBag.Slide = GetSlideDal(db.Producto);
            var products = DALproducto.Listarpindex();

            var pageNumber = page ?? 1;
            ViewBag.Productos = products.ToPagedList(pageNumber, 6);


            return View();
        }

        private List<Producto> GetSlideDal(DbSet<Producto> producto) => (from p in producto
                                                                         where p.Tipo >= 1 && p.Tipo <= 4
                                                                          select p).ToList();

        //Fin- Pagina de inicio (Productos)

        //Inicio Acerca de la empresa
        public ActionResult About()
        {
            return View();
        }
        //Fin Acerca de la empresa

        //Inicio Contacto
        public ActionResult Contact()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {

                return View();
            }
           
        }
        //Fin Contacto


        //Inicio POST Contacto
        [HttpPost]
        public ActionResult Contact(Contacto con)
        {
            try
            {
                string n, a, e, m;
                n = con.Nombre_contacto;
                a = con.Asunto_contacto;
                e = con.Email_contacto;
                m = con.Mensaje_contacto;
                con.Fecha = DateTime.Now.ToShortDateString();
                con.Estado = "1";
                if (this.IsCaptchaValid("Captcha is not valid"))
                {
                    con.Estado = "1";
                }
                else
                {

                    con.Estado = null;
                }
                if (n != null && a != null && e != null && m != null)
                {


                    if (ModelState.IsValid && con.Estado != null)
                    {
                        DALcontacto.Ingresar(con);
                        ViewBag.Estado = "Captcha válido";
                        return View("Contact");

                    }
                    else
                    {
                        ViewBag.Estado = "Captcha Inválido";
                        return View(con);
                    }
                }
                else
                {
                    return RedirectToAction("Contact");
                }
            }
            catch (Exception)
            {

                return View();
            }
           



        }
          
            
        
        //Fin POST Contacto



        /// <summary>
        /// Caja de busqueda y seleccion de subcategoria
        /// </summary>
        /// <returns>Retorna un producto buscado y la subcategoria seleccionada</returns>
        /// 


        //Inicio Buscar productos
        public ActionResult Getbusqueda(string buscarproducto, int? page)
        {
            //ViewBag.Busqueda = dalpro.getBusquedap(buscarproducto);
            ViewBag.Categoria = DALcategoria.Listar();
            ViewBag.Subcategoria = DALsubcategoria.Listar();

            ViewBag.Id = buscarproducto;

            var pageNumber = page ?? 1;
            var busqueda = DALproducto.getBusquedap(buscarproducto);
            int cantidad = busqueda.Count();

            if (cantidad == 0)
            {
                ViewBag.busqueda = null;
                ViewBag.cantidad = "MK7";
            }

            ViewBag.Busqueda = busqueda.ToPagedList(pageNumber, 6);
            return View();

        }
        //Fin Buscar productos

        //Inicio Obtener subcategoria
        public ActionResult Getsubcategoria(string Id_subcategoria, int? page)
        {
            if (!IsDigitsOnly(Id_subcategoria))
            {
                return RedirectToAction("Index", "Home");
            }
            

            int Id_subcat = Int32.Parse(Id_subcategoria);
            if (!db.Subcategoria.Any(c => c.Id_subcategoria == Id_subcat))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Categoria = DALcategoria.Listar();
            ViewBag.Subcategoria = DALsubcategoria.Listar();
            ViewBag.Id = Id_subcat;
            var pageNumber = page ?? 1;
            var busqueda = DALproducto.getBusquedasc(Id_subcat);
            ViewBag.Busqueda = busqueda.ToPagedList(pageNumber, 6);
            return View();
        }
        //Fin Obtener subcategoria



        /// <summary>
        /// Seccion Errores
        /// </summary>
        /// <returns>Errores de links no existentes y captcha invalido</returns>
        /// 


        //Inicio Error 404
        public ActionResult Error()
        {
            return View();
        }
        //Fin Error 404

        //Inicio Error Captcha Vista Contacto
        public ActionResult ErrorCaptcha()
        {
            return View();
        }
        //Fin Error Captcha Vista Contact

       public bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        //Inicio Detalle de producto
        public ActionResult DetalleProducto(string Id)
        {
            try
            {
                if (!IsDigitsOnly(Id))
                {
                    return RedirectToAction("Index");
                }
                if (Id == null)
                {
                    return RedirectToAction("Index");
                }
                Int32 id = Int32.Parse(Id);
                if (db.Producto.Any(pro => pro.Id_producto == id))
                {
                    ViewBag.Categoria = DALcategoria.Listarcindex();
                    ViewBag.Subcategoria = DALsubcategoria.Listar();
                    ViewBag.Productos = DALproducto.getlista(Int32.Parse(Id));
                    return View("DetalleProducto");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

        }
        //Fin Detalle de producto





        //Inicio Contacto producto
        [HttpPost]
        public ActionResult ProductoContact(Contacto con, FormCollection cp)
        {
            try
            {
                string id_item, nombre_producto, nombreContacto, AsuntoContacto, EmailContacto, MensajeContacto;
                id_item = cp["Id_item"];
                nombre_producto = cp["Nombre_item"];
                nombreContacto = con.Nombre_contacto;
                AsuntoContacto = con.Asunto_contacto;
                EmailContacto = con.Email_contacto;
                MensajeContacto = con.Mensaje_contacto;

                con.Fecha = DateTime.Now.ToShortDateString();
                con.Estado = "1";
                if (this.IsCaptchaValid("Captcha is not valid"))
                {
                    con.Estado = "1";
                }
                else
                {

                    con.Estado = null;
                }

                if (nombreContacto != null && AsuntoContacto != null && EmailContacto != null && MensajeContacto != null)
                {


                    if (ModelState.IsValid && con.Estado != null)
                    {

                        con.Asunto_contacto = string.Concat(id_item, "-", nombre_producto);

                        bool ingresa = DALcontacto.Ingresar(con);
                        if (ingresa)
                        {
                            ViewBag.Categoria = DALcategoria.Listarcindex();
                            ViewBag.Subcategoria = DALsubcategoria.Listar();
                            ViewBag.Productos = DALproducto.getlista(Convert.ToInt32(cp["Id_item"]));
                            ViewBag.Estado = "Captcha válido";
                            return View("DetalleProducto", con);
                        }
                        else
                        {
                            ViewBag.Categoria = DALcategoria.Listarcindex();
                            ViewBag.Subcategoria = DALsubcategoria.Listar();
                            ViewBag.Productos = DALproducto.getlista(Convert.ToInt32(cp["Id_item"]));
                            ViewBag.Estado = "Captcha Inválido";
                            return View("DetalleProducto", con);
                        }
                    }
                    else
                    {
                        ViewBag.Categoria = DALcategoria.Listarcindex();
                        ViewBag.Subcategoria = DALsubcategoria.Listar();
                        ViewBag.Productos = DALproducto.getlista(Convert.ToInt32(cp["Id_item"]));
                        ViewBag.Estado = "Captcha Inválido";
                        return View("DetalleProducto", con);
                    }
                }
                else
                {
                    ViewBag.Categoria = DALcategoria.Listarcindex();
                    ViewBag.Subcategoria = DALsubcategoria.Listar();
                    ViewBag.Productos = DALproducto.getlista(Convert.ToInt32(cp["Id_item"]));
                    return RedirectToAction("DetalleProducto", new { Id = id_item });

                }


            }

            catch (Exception)
            {

                return View("Index");
            }


        }





    }
}


