using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Menajes_Maipu.Models.DAL.Administrador;
using Menajes_Maipu.Models.ModeloBD;
using Menajes_Maipu.Models.DAL.Home;
using PagedList;
using Microsoft.Reporting.WebForms;
using System.IO;
using Menajes_Maipu.Models.Security;

namespace Menajes_Maipu.Controllers
{
    [Authorize(Roles = "admin")]
    public class DashboardController : Controller
    {
        public static readonly log4net.ILog log = LogHelper.GetLogger();
        MenajesbdEntities6 ctx = new MenajesbdEntities6();
        CategoriaDAL DALcategoria = new CategoriaDAL();
        SubcategoriaDAL DALsubcategoria = new SubcategoriaDAL();
        ProductosDAL DALproducto = new ProductosDAL();
        UsuarioDAL DALusuario = new UsuarioDAL();
        VentasDAL DALventa = new VentasDAL();
        DebitoDAL DALdebito = new DebitoDAL();
     
        List<Producto> Lproducto = new List<Producto>();
        List<Usuario> Lusuario = new List<Usuario>();
        ReportDataSource reportdata = new ReportDataSource();
        ContactoDAL DALcontacto = new ContactoDAL();
        Subcategoria subcategoria = new Subcategoria();
        string info,mimeType,encoding,fileNameExtension,reportType,path;
        Warning[] warnings;
        string[] streams;
        byte[] renderedBytes;

        public ReportDataSource Reportdata { get; private set; }


        //Vistas Parciales y Reportes 
        public string DeviceInfo(string id)
        {
            return "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";
        }
        public ActionResult Slider()
        {
            ViewBag.Sactivo = ctx.Producto.Where(p => p.Tipo == 1).ToList();
            ViewBag.Sitem2 = ctx.Producto.Where(p => p.Tipo == 2).ToList();
            ViewBag.Sitem3 = ctx.Producto.Where(p => p.Tipo == 3).ToList();
            ViewBag.Sitem4 = ctx.Producto.Where(p => p.Tipo == 4).ToList();


            ViewBag.Producto = DALproducto.Listar();
            return View();
        }
        [HttpPost]
        public ActionResult Slider(FormCollection f)
        {
            ViewBag.Sactivo = ctx.Producto.Where(p => p.Tipo == 1).ToList();
            ViewBag.Sitem2 = ctx.Producto.Where(p => p.Tipo == 2).ToList();
            ViewBag.Sitem3 = ctx.Producto.Where(p => p.Tipo == 3).ToList();
            ViewBag.Sitem4 = ctx.Producto.Where(p => p.Tipo == 4).ToList();
            if (f["Principal"] != null)
            {
                int id = Int32.Parse(f["principal"]);
                int tipo = 1;

                DALproducto.ModificarTipo(id, tipo);
            }
            if (f["Segundo"] != null)
            {
                int id = Int32.Parse(f["Segundo"]);
                int tipo = 2;
                DALproducto.ModificarTipo(id, tipo);
            }
            if (f["Tercero"] != null)
            {
                int id = Int32.Parse(f["Tercero"]);
                int tipo = 3;
                DALproducto.ModificarTipo(id, tipo);
            }
            if (f["Cuarto"] != null)
            {
                int id = Int32.Parse(f["Cuarto"]);
                int tipo = 4;
                DALproducto.ModificarTipo(id, tipo);
            }

            ViewBag.Producto = DALproducto.Listar();
            return RedirectToAction("Slider");
        }
        public PartialViewResult P10MENOSTOCK()
        {
            Lproducto= ctx.Producto.OrderBy(x => x.Stock_producto).Take(10).ToList();
            Session["idPDF"] = "10MENOSTOCK";

            return PartialView("_Productos", Lproducto);
        }
        public ActionResult ReportP10MENOSTOCK(string id)
        {
            LocalReport localReport = new LocalReport();
            reportType = id;
            info= DeviceInfo(id);
            path = Path.Combine(Server.MapPath("~/Reportes"), "ProductoReport.rdlc");
            if (System.IO.File.Exists(path))
            {
               localReport.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
             Lproducto =ctx.Producto.OrderBy(x => x.Stock_producto).Take(10).ToList();
             reportdata= new ReportDataSource("Productodata", Lproducto);
            localReport.DataSources.Add(reportdata);
            renderedBytes = localReport.Render(
                reportType,
                info,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedBytes, mimeType);
        }
        public PartialViewResult CLIALL()
        {
          Lusuario = ctx.Usuario.Where(c => c.Tipo == 3).OrderByDescending(x => x.Fecha_registro).ToList();
            Session["idPDF"] = "CLIALL";

            return PartialView("_Usuarios", Lusuario);
        }
        public ActionResult ReportCLIALL(string id)
        {
            LocalReport localReport = new LocalReport();
            info = DeviceInfo(id);
            reportType = id;
            string path = Path.Combine(Server.MapPath("~/Reportes"), "ClienteReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                localReport.ReportPath = path;
            }
            else
            {
                return View("Index");
            } 
            Lusuario = ctx.Usuario.Where(c => c.Tipo == 3).OrderBy(c => c.Nombre_usuario).ToList();
            reportdata = new ReportDataSource("ClienteData", Lusuario);
            localReport.DataSources.Add(reportdata);
            renderedBytes = localReport.Render(
                reportType,
                info,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedBytes, mimeType);
        }
        public PartialViewResult TRABALL()
        {
            Lusuario = ctx.Usuario.Where(c => c.Tipo == 4).OrderBy(c => c.Fecha_registro).ToList();
            Session["idPDF"] = "TRABALL";
            return PartialView("_Usuarios", Lusuario);
        }
        public ActionResult ReportTRABALL(string id)
        {
            LocalReport localReport = new LocalReport();
            info = DeviceInfo(id);
            string reportType = id;
            string path = Path.Combine(Server.MapPath("~/Reportes"), "UsuarioReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                localReport.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            Lusuario = ctx.Usuario.Where(c => c.Tipo == 4).OrderBy(c => c.Fecha_registro).ToList();
            reportdata = new ReportDataSource("UsuariosData", Lusuario);
            localReport.DataSources.Add(reportdata);
            renderedBytes = localReport.Render(
                reportType,
                info,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedBytes, mimeType);
        }
        public PartialViewResult P10MASTOCK()
        {
           Lproducto = ctx.Producto.OrderByDescending(x => x.Stock_producto).Take(10).ToList();
            Session["idPDF"] = "10MASTOCK";

            return PartialView("_Productos", Lproducto);
        }
        public ActionResult ReportP10MASTOCK(string id)
        {
            LocalReport localReport = new LocalReport();
            info = DeviceInfo(id);
            string path = Path.Combine(Server.MapPath("~/Reportes"), "ProductoReport.rdlc");
            string reportType = id;
            if (System.IO.File.Exists(path))
            {
                localReport.ReportPath = path;
            }
            else
            {
                return View("Index");
            }            
            Lproducto = ctx.Producto.OrderByDescending(x => x.Stock_producto).Take(10).ToList();
            reportdata = new ReportDataSource("Productodata", Lproducto);
            localReport.DataSources.Add(reportdata);
            renderedBytes = localReport.Render(
                reportType,
                info,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedBytes, mimeType);
        }
        public PartialViewResult P10MAS()
        {
            Lproducto = ctx.Producto.OrderByDescending(x => x.Precio_producto).Take(10).ToList();
            Session["idPDF"] = "10MAS";

            return PartialView("_Productos", Lproducto);
        }
        public ActionResult ReportP10MAS(string id)
        {
            LocalReport localReport = new LocalReport();
            reportType = id;
            info = DeviceInfo(id);
            path = Path.Combine(Server.MapPath("~/Reportes"), "ProductoReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                localReport.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
                Lproducto= ctx.Producto.OrderByDescending(x => x.Precio_producto).Take(10).ToList();

           reportdata = new ReportDataSource("Productodata", Lproducto);

            localReport.DataSources.Add(reportdata);
         
            renderedBytes = localReport.Render(
                reportType,
                info,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            return File(renderedBytes, mimeType);
        }
        public PartialViewResult P10MENOS()
        {
           Lproducto= ctx.Producto.OrderBy(x => x.Precio_producto).Take(10).ToList();
            Session["idPDF"] = "10MENOS";
            return PartialView("_Productos", Lproducto);
        }
        public ActionResult ReportP10MENOS(string id) //log4net
        {
            try
            {
                LocalReport localReport = new LocalReport();
                info = DeviceInfo(id);
                reportType = id;
                path = Path.Combine(Server.MapPath("~/Reportes"), "ProductoReport.rdlc");
                if (System.IO.File.Exists(path))
                {
                    localReport.ReportPath = path;
                }
                else
                {
                   
                }
                Lproducto = ctx.Producto.OrderBy(x => x.Precio_producto).Take(10).ToList();
                reportdata = new ReportDataSource("Productodata", Lproducto);
                localReport.DataSources.Add(reportdata);
                renderedBytes = localReport.Render(
                    reportType,
                    info,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                return File(renderedBytes, mimeType);
            }
         
        
               catch (Exception ex)
            {
                log.Error("ReportP10MENOS " + ex.Message);
                return View("Index");
            }
        }
        //FIN Vistas Parciales y Reportes 



        // GET: Dashboard
        public ActionResult Index()
        {           

            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            ViewBag.contarcat = ctx.Categoria.ToList().Count();
            ViewBag.contarscat = ctx.Subcategoria.ToList().Count();
            ViewBag.contarpro = DALproducto.Listar().Count();
            ViewBag.contaruser = ctx.Usuario.Count();
            ViewBag.contarven = ctx.Ventas.Count();
            ViewBag.contarvenc = ctx.Ventas.Where(c => c.Estado == "CONCRETADO").Count();
            ViewBag.contarvena = ctx.Ventas.Where(c => c.Estado == "ANULADO").Count();

            return View();
        }

        public ActionResult EditPerfil(Int32 id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            ViewBag.Usuario = DALusuario.getUsuarioALLDATA(id);
            ViewBag.Todorol = DALusuario.ListarRol();
            ViewBag.Rol = DALusuario.nombreRol(id);
            return View();
        }

        [HttpPost]
        public ActionResult EditPerfil(Usuario user)
        {
            DALusuario.Modificar(user);          
            return RedirectToAction("Index");
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////INICIO PRODUCTO //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // GET: Productos/Create
        public ActionResult ProductosCreate()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            ViewBag.Todo = DALsubcategoria.Listar();
            return View();
        }
        [HttpPost]
        public ActionResult ProductosCreate(Producto p, HttpPostedFileBase imagen)
        {
            if (ModelState.IsValid == true)
            {
                if (imagen != null)
                {

                    p.Nombre_imagen = imagen.ContentType;
                    p.Imagen_producto = new byte[imagen.ContentLength];
                    imagen.InputStream.Read(p.Imagen_producto, 0, imagen.ContentLength);

                }

                if (ctx.Producto.Any(pro => pro.Nombre_producto == p.Nombre_producto))
                {
                    ModelState.AddModelError("Nombre_producto", "Error! Este nombre ya fue ingresado!");
                    ViewBag.Todo = DALsubcategoria.Listar();
                    return View(p);
                }
                else
                {
                    DALproducto.Ingresar(p);
                    return RedirectToAction("ProductosList");
                }

            }
            ViewBag.Todo = DALsubcategoria.Listar();
            return View(p);
        }
        //POST: Productos/Edit/5
        [HttpPost]
        public ActionResult ProductosEdit(Producto p, HttpPostedFileBase imagen, int id)
        {
            int i = 0;
            p.Id_producto = id;
            p.Tipo = i;
            if (imagen != null)
            {
                p.Nombre_imagen = imagen.ContentType;
                p.Imagen_producto = new byte[imagen.ContentLength];
                imagen.InputStream.Read(p.Imagen_producto, 0, imagen.ContentLength);
            }
            if (ViewData.ModelState.Values.Any(x => x.Errors.Count == 1))
            {

                DALproducto.Modificar(p);
                bool validar = DALproducto.Modificar(p);
                if (validar == true)
                {
                    return RedirectToAction("ProductosList");
                }
                else
                {
                    ViewBag.nombre = DALproducto.getlista(p.Id_producto);
                    ViewBag.Todo = DALsubcategoria.Listar();
                    return View(p);
                }
            }
            return View(p);
        }



        // GET: Productos/Edit/5
        public ActionResult ProductosEdit(int id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            } 
            ViewBag.nombre = DALproducto.getlista(id);
            ViewBag.Todo = DALsubcategoria.Listar();
            return View();
        }
        // GET: Productos/Delete/5
        public ActionResult ProductosDelete(int id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            ViewBag.nombre = DALproducto.getlista(id);
            ViewBag.Listar = DALproducto.getlista(id);
            ViewBag.Todo = DALsubcategoria.Listar();
            return View();
        }
        // POST: Productos/Delete/5
        [HttpPost]
        public ActionResult ProductosDelete(int id, FormCollection collection)
        {
            try
            {
               DALproducto.Eliminar(id);
                return RedirectToAction("ProductosList");
            }
            catch
            {
                return View();
            }
        }
        // GET: Productos
        public ActionResult ProductosList(int? page)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            var products = DALproducto.Listar();
            var pageNumber = page ?? 1;
            ViewBag.Productos = products.ToPagedList(pageNumber, 6);
            return View();
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////FIN PRODUCTO //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////








        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////INICIO CATEGORIA //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public ActionResult CategoriasList()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            ViewBag.Categoria = DALcategoria.Listar();
            return View();
        }
        // GET: Categoria/Create
        public ActionResult CategoriasCreate()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            return View();
        }
        // POST: Categoria/Create
        [HttpPost]
        public ActionResult CategoriasCreate(Categoria cat)
        {
            try
            {
                if (ctx.Categoria.Any(pro => pro.Nombre_categoria == cat.Nombre_categoria)) 
                {
                    ModelState.AddModelError("Nombre_categoria", "Error! Este nombre ya fue ingresado!");
                    ViewBag.Error = "Error!Este nombre ya fue ingresado!";
                    return View(cat);
                }
                DALcategoria.Ingresar(cat);
                return RedirectToAction("CategoriasList");
            }
            catch
            {
                return View();
            }
        }
        // GET: Categoria/Edit/5
        public ActionResult CategoriasEdit(int id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            ViewBag.nombre = DALcategoria.getlista(id);
            ViewBag.id = id;
            return View();
        }
        // POST: Categoria/Edit/5
        [HttpPost]
        public ActionResult CategoriasEdit(int id, string Nombre_categoria)
        {
            try
            {
                DALcategoria.Modificar(id, Nombre_categoria);
                return RedirectToAction("CategoriasList");
            }
            catch
            {
                return View();
            }
        }
        // GET: Categoria/Delete/5
        public ActionResult CategoriasDelete(int id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            ViewBag.nombre = DALcategoria.getlista(id);

            return View();
        }
        // POST: Categoria/Delete/5
        [HttpPost]
        public ActionResult CategoriasDelete(int id, FormCollection collection)
        {
            try
            {
                if (DALcategoria.Eliminar(id))
                {
                    return RedirectToAction("CategoriasList");
                }
                else
                {
                    ViewBag.nombre = DALcategoria.getlista(id);
                    ViewBag.Error = "Error";
                    return View();
                }
               
            }
            catch
            {
                return RedirectToAction("Login","Home");
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////FIN CATEGORIA //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////








        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////INICIO SUBCATEGORIA //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // GET: Subcategoria
        public ActionResult SubcategoriaList(int? page)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            var subcategorias = DALsubcategoria.Listar();
            var pageNumber = page ?? 1;
            ViewBag.Subcategoria = subcategorias.ToPagedList(pageNumber, 6);

            return View();
        }

        // GET: Subcategoria/Create
        public ActionResult SubcategoriaCreate()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            ViewBag.Todo = DALcategoria.Listar();
            return View();
        }
        // POST: Subcategoria/Create
        [HttpPost]
        public ActionResult SubcategoriaCreate(FormCollection subcat)
        {
            try
            {
                ViewBag.Todo = DALcategoria.Listar();
                subcategoria.Nombre_subcategoria = subcat["Nombre_subcategoria"];
                subcategoria.Id_categoria = int.Parse(subcat["Id_categoria"]);
                DALsubcategoria.Ingresar(subcategoria);
                return RedirectToAction("SubcategoriaList");
            }
            catch
            {
                return View();
            }
        }
        // GET: Subcategoria/Edit/5
        public ActionResult SubcategoriaEdit(int id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            ViewBag.Todo = DALcategoria.Listar();
            ViewBag.nombre = DALsubcategoria.getlista(id);
            return View();
        }
        // POST: Subcategoria/Edit/5
        [HttpPost]
        public ActionResult SubcategoriaEdit(int id, string Nombre_subcategoria, int Id_categoria)
        {
            try
            {
   
                DALsubcategoria.Modificar(id, Nombre_subcategoria, Id_categoria);
                return RedirectToAction("SubcategoriaList");
            }
            catch
            {
                return View();
            }

        }
        // GET: Subcategoria/Delete/5
        public ActionResult SubcategoriaDelete(int id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            ViewBag.nombre = DALsubcategoria.getlista(id);

            return View();
        }
        // POST: Subcategoria/Delete/5
        [HttpPost]
        public ActionResult SubcategoriaDelete(int id, FormCollection collection)
        {
            try
            {
                DALsubcategoria.Eliminar(id);
                return RedirectToAction("SubcategoriaList");
            }
            catch
            {
                return View();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////FIN SUBCATEGORIA //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////INICIO CONTACTO //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public ActionResult ContactoCliente(int? page)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }


            var viewModel =
                (from mensaje in ctx.Mensaje_consulta
                join usuario in ctx.Usuario on mensaje.id_user equals usuario.id_user

                select new UsuarioMotivo() { usuario = usuario, mensaje = mensaje});

            var contacto = viewModel.ToList();
            var pageNumber = page ?? 1;
            ViewBag.Contacto = contacto.ToPagedList(pageNumber, 6);
            return View();
        }




        public ActionResult ContactoClienteDetalle(int id, int id_user)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");

            }
            ViewBag.Usuario = DALusuario.getUsuarioALLDATA(id_user);
            ViewBag.Contacto = DALusuario.Listar_por_id_contacto(id);

            return View();
        }




        [HttpPost]
        public ActionResult ContactoClienteDetalle(FormCollection f)
        {


            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");

            }
            int id_user = Int32.Parse(f["id_user"]);
            int id = Int32.Parse(f["Id_contacto"]);
            string respuesta = f["Respuesta"];
            DALcontacto.ModificarMensaje_consulta(id, respuesta);
            ViewBag.Usuario = DALusuario.getUsuarioALLDATA(id_user);
            ViewBag.Contacto = DALusuario.Listar_por_id_contacto(id);
            return RedirectToAction("ContactoCliente");
            //}
        }


        public ActionResult ContactoList(int? page)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            var contacto = DALcontacto.Listar();
            var pageNumber = page ?? 1;
            ViewBag.Contacto = contacto.ToPagedList(pageNumber, 6);
            return View();
        }

        public ActionResult ContactoDetalle(int id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            ViewBag.Contacto = DALcontacto.getlista(id);
            return View();
        }

        [HttpPost]
        public ActionResult ContactoDetalle(int id, string Estado)
        {
            string e = Estado; //  21/8/2016  modificar.
            try
            {
                DALcontacto.Modificar(id);
                return RedirectToAction("ContactoList");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ContactoDelete(int id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            DALcontacto.Eliminar(id);
            return RedirectToAction("ContactoList");

        }



        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////FIN CONTACTO //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////INICIO VENTAS //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public ActionResult VentasList(int? page)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }           
            var ventas = DALventa.ListarVentas();

            var pageNumber = page ?? 1;
            ViewBag.Lista = ventas.ToPagedList(pageNumber, 6);

            return View();
        }

        public ActionResult VentasEdit(string id)
        {
            if (Session["user"] == null && id==null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            ViewBag.Id_ventas = id;
            return View();
        }

        [HttpPost]
        public ActionResult VentasEdit(FormCollection f)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            int Id_ventas = Int32.Parse(f["Id_ventas"]);
            string Estado_venta= f["Estado_venta"];
            string Estado_despacho = f["Estado_despacho"];
            string Fecha_envio = f["Fecha_envio"];
            string Fecha_entrega = f["Fecha_entrega"];

            DALventa.Modificar(Id_ventas, Estado_venta, Estado_despacho, Fecha_envio, Fecha_entrega);

            ViewBag.Id_ventas = Id_ventas;
            return View();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////FIN VENTAS //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////INICIO DEBITO //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public ActionResult DebitoList(int? page)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            var debito = DALdebito.Listar();
            var pageNumber = page ?? 1;
            ViewBag.Lista = debito.ToPagedList(pageNumber, 6);
            return View();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////FIN DEBITO //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////INICIO BANCO MENAJE //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        BancoDAL banco = new BancoDAL();

   

        public ActionResult BancoList()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            ViewBag.Cuenta = banco.Listar().Count();
            ViewBag.BancosListar = banco.Listar();
            return View();
        }

        public ActionResult BancoCreate()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            return View();
        }

        [HttpPost]
        public ActionResult BancoCreate(Banco_menaje b)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            if (ModelState.IsValid)
            {
           
                if (banco.Ingresar(b))
                {
                    ViewBag.Cuenta = banco.Listar().Count();
                    ViewBag.BancosListar = banco.Listar();
                    return RedirectToAction("BancoList");
                }
                else
                {
                    return View(b);
                }
            }
            else
            {
                return View(b);
            }
        }

        public ActionResult BancoDelete(string id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            ViewBag.BancoListar = banco.Listar_por_id(id);
            return View();
        }

        [HttpPost]
        public ActionResult BancoDelete(Banco_menaje b)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            bool elimina = banco.Eliminar(b);

            if (elimina)
            {
                ViewBag.Cuenta = banco.Listar().Count();
                ViewBag.BancosListar = banco.Listar();
                return RedirectToAction("BancoList");
            }
            else
            {
                ViewBag.BancoListar = banco.Listar_por_id(b.Num_cuenta);
                return View(b);
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////FIN BANCO MENAJE //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    }

}
