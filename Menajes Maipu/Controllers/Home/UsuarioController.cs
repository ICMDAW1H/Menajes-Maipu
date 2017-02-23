
using Menajes_Maipu.Models.DAL.Administrador;
using Menajes_Maipu.Models.ModeloBD;
using Menajes_Maipu.Models.Security;
using Microsoft.Reporting.WebForms;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Menajes_Maipu.Controllers.Home

{
    public class UsuarioController : Controller
    {
        MenajesbdEntities6 ctx = new MenajesbdEntities6();
        UsuarioDAL DAL = new UsuarioDAL();
        VentasDAL dalven = new VentasDAL();

        /// <summary>
        /// Metodos Json para validar campos
        /// </summary>
        /// <returns>Valida que no se repitan datos de usuarios</returns>

        public static readonly log4net.ILog log = LogHelper.GetLogger();


        public bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
        public FileContentResult GetImage(int Id_ventas)
        {
            Debito pro = ctx.Debito.FirstOrDefault(c => c.Id_ventas == Id_ventas);
            if (pro != null)
            {

                return File(pro.Imagen, pro.N_img);
            }
            else
            {
                return null;
            }
        }
        /// /////////////
        /// Json que validan campos
        // Busca si el NOMBRE del usuario existe en la base de datos.
        public JsonResult IsUserNameAvailable(string Nombre_usuario)
        {
            return Json(!ctx.Usuario.Any(user => user.Nombre_usuario == Nombre_usuario), JsonRequestBehavior.AllowGet);
        }
        //public JsonResult ExisteConElMismoNombre(string Nombre_usuario, int id_user)
        //{

        //    return Json(!ctx.Usuario.Any(user => user.Nombre_usuario == Nombre_usuario && user.id_user == id_user), JsonRequestBehavior.AllowGet);
        //}
        // Busca si el EMAIL del usuario existe en la base de datos.
        public JsonResult IsUserEmailAvailable(string Email_usuario)
        {
            return Json(!ctx.Usuario.Any(user => user.Email_usuario == Email_usuario), JsonRequestBehavior.AllowGet);
        }
        // Busca si el NOMBRE del producto existe en la base de datos.
        public JsonResult IsProductNameAvailable(string Nombre_producto)
        {
            return Json(!ctx.Producto.Any(p => p.Nombre_producto == Nombre_producto), JsonRequestBehavior.AllowGet);
        }
        // Busca si el NOMBRE de la categoria existe en la base de datos.
        public JsonResult IsCatNameAvailable(string Nombre_categoria)
        {
            return Json(!ctx.Categoria.Any(c => c.Nombre_categoria == Nombre_categoria), JsonRequestBehavior.AllowGet);
        }
        // Busca si el Nombre de la subcategoria existe en la base de datos
        public JsonResult IsSubcatNameAvailable(string Nombre_subcategoria)
        {
            return Json(!ctx.Subcategoria.Any(c => c.Nombre_subcategoria == Nombre_subcategoria), JsonRequestBehavior.AllowGet);
        }
        /// /////////////
        /// 

        /// <summary>
        /// Metodos que deberian estar en Dashboard
        /// </summary>
        /// <returns>Crear, Editar y listar usuarios de la BD</returns>

        public PartialViewResult DebitoDetalle(string ID)
        {
            int id = Int32.Parse(ID);
            List<Debito> model = ctx.Debito.Where(x => x.Id_ventas == id).ToList();
            //Session["idPDF"] = "CLIALL";

            return PartialView("_Debito", model);
        }
        [HttpPost]
        public ActionResult DebitoDetalle(FormCollection f, HttpPostedFileBase imagen)
        {
            try
            {
                DebitoDAL deb = new DebitoDAL();
                Debito debito = new Debito();
                if (imagen != null)
                {
                    debito.N_img = imagen.ContentType;
                    debito.Imagen = new byte[imagen.ContentLength];
                    imagen.InputStream.Read(debito.Imagen, 0, imagen.ContentLength);
                }

                int deposito = Int32.Parse(f["Cod_deposito"]);
                debito.Id_ventas = Int32.Parse(f["Id_ventas"]);
                debito.Cod_deposito = deposito;
                debito.Num_cuenta = f["Num_cuenta"];
                debito.Fecha_deposito = f["Fecha_deposito"];
                //var id_user = ctx.Ventas.Where(c => c.Id_ventas == debito.Id_ventas).Select(c => c.id_user).SingleOrDefault();
                //int Id_user =id_user;


                bool validar = deb.Modificar(debito);
                if (validar == true)
                {
                    ViewBag.Estado = "Datos ingresados exitosamente.";
                    return RedirectToAction("Vestado", "Home", new { estado = "1" });
                }
                else
                {
                    ViewBag.Estado = "Los datos ingresados contienen errores.";
                    return RedirectToAction("Vestado", "Home", new { estado = "0" });
                }
            }
            catch (Exception)
            {

                ViewBag.Estado = "Los datos ingresados contienen errores.";
                return PartialView("Error", "Home");
            }

        }








        //    List<Debito> model = ctx.Debito.Where(x => x.Id_ventas == id).ToList();
        //    //Session["idPDF"] = "CLIALL";




        public ActionResult EntradaDetalle(int id)
        {
            ViewBag.listarid = ctx.Mensaje_consulta.Where(c => c.Id_contacto == id).ToList();
            return View();
        }

        public ActionResult BuzonEntrada(string ID)
        {
            if (Session["id"] == null || ID == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            if (!IsDigitsOnly(ID))
            {
                return RedirectToAction("Login", "Usuario");
            }
            int id = Int32.Parse(ID);
           
            string idsession = Session["id"].ToString();
            string id_user = id.ToString();
            if (idsession == id_user)
            {

                ViewBag.id = id;
                UsuarioDAL DAL = new UsuarioDAL();
                ViewBag.Lista = DAL.Listar_por_id(id);
                return View();
            }
            else
            {
              return RedirectToAction("Login", "Usuario");
            }

        }


        public ActionResult ConsultaCreate(string ID)
        {
            if (Session["id"] == null || ID == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            int id = Int32.Parse(ID);
            if (!IsDigitsOnly(ID))
            {
                return RedirectToAction("Login", "Usuario");
            }
          
            string idsession = Session["id"].ToString();
            string id_user = id.ToString();
            if (idsession == id_user)
            {
                ViewBag.id = id;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }
        [HttpPost]
        public ActionResult ConsultaCreate(FormCollection f)
        {
            try
            {
            string msj = f["Mensaje"];
            Mensaje_consulta mc = new Mensaje_consulta();
            string mifecha = DateTime.Now.ToShortDateString();
            mc.Fecha = mifecha;
            mc.id_user = Int32.Parse(f["id_user"]);
            mc.Motivo_consulta = f["Motivo_consulta"];
            msj = Regex.Replace(msj, "<script.*?</script>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            mc.Mensaje = msj;
            mc.Estado_consuta = "PROCESANDO";
            mc.Respuesta_consulta = "ESPERANDO RESPUESTA";
            ctx.Mensaje_consulta.Add(mc);
            ctx.SaveChanges();
            ViewBag.id = mc.id_user;
          
            int id = Int32.Parse(f["id_user"]);
            ViewBag.Lista = DAL.Listar_por_id(id);
            ViewBag.Estado = "Exito";
            return View("BuzonEntrada", Session["id_user"]);
            }
            catch (Exception)
            {
                return View("Index", "Home");
            }
        }




        //Inicio Crear Usuario en Dashboard
        [Authorize(Roles = "admin")]
        public ActionResult CrearUsuario()
        {
            UsuarioDAL DAL = new UsuarioDAL();
            ViewBag.Rol = DAL.ListarRol();
            return View();
        }
        //Fin Crear Usuario en Dashboard

        //Inicio Crear Usuario en Dashboard POST
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CrearUsuario(Usuario user)
        {

            if (ModelState.IsValid == false)
            {
                ViewBag.Rol = DAL.ListarRol();
                return View();
            }
            else
            {
                DAL.Ingresar(user);
                return RedirectToAction("Index", "Dashboard");

            }


        }
        //Fin Crear Usuario en Dashboard POST

        //Inicio Editar Cliente en Dashboard
        public ActionResult EditCliente(string ID)
        {
            if (Session["id"] == null || ID == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            if (!IsDigitsOnly(ID))
            {
                return RedirectToAction("Login", "Usuario");
            }
            int id = Int32.Parse(ID);
            
            string idsession = Session["id"].ToString();
            string id_user = id.ToString();
            if (idsession == id_user)
            {

                ViewBag.Usuario = DAL.getUsuarioALLDATA(id);
                ViewBag.Todorol = DAL.ListarRol();
                ViewBag.Rol = DAL.nombreRol(id);
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Usuario");
            }
           
        }
        //Fin Editar Cliente en Dashboard

        //Inicio Editar Cliente Post en Dashboard
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditCliente(Usuario user)
        {
            user.Contrasena = ctx.Usuario.Where(p => p.id_user == user.id_user).Select(c => c.Contrasena).SingleOrDefault();
            if (ModelState.IsValid)
            {
                DAL.Modificar(user);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Usuario = DAL.getUsuarioALLDATA(user.id_user);
                ViewBag.Todorol = DAL.ListarRol();
                ViewBag.Rol = DAL.nombreRol(user.id_user);
                ViewBag.Estado = "Error";
                return View(user);
            }
        }
        //Fin Editar Cliente Post en Dashboard

        //Inicio Editar Password en Dashboard
        public ActionResult EditPassword(string ID)
        {
            if (Session["id"] == null || ID == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            if (!IsDigitsOnly(ID))
            {
                return RedirectToAction("Login", "Usuario");
            }
            int id = Int32.Parse(ID);
           
            string idsession = Session["id"].ToString();
            string id_user = id.ToString();
            if (idsession == id_user)
            {
                ViewBag.Usuario = DAL.getUsuarioALLDATA(id);
                ViewBag.Todorol = DAL.ListarRol();
                ViewBag.Rol = DAL.nombreRol(id);
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }
        //Fin Editar Password en Dashboard

        //Inicio Editar Cliente Post en Dashboard
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditPassword(Usuario user, FormCollection us)
        {
            string contrasena = us["Contrasena1"];
            string contrasena2 = us["Contrasena2"];
            if (contrasena == contrasena2)
            {
                Usuario usuario = ctx.Usuario.FirstOrDefault(u => u.Contrasena.Equals(contrasena));
                if (usuario != null) {
                    DAL.ModificarPassword(user);
                    Session["user"] = null;
                    Session["id"] = null;
                    Session["cpw"] = "true";
                    return RedirectToAction("Login", "Usuario");
                }
                else
                {
                    ViewBag.Estado = "Error";
                    return View(user);
                }
            }
            else
            {
                ViewBag.Estado = "Error";
                return View(user);
            }
        }
        //Fin Editar Cliente Post en Dashboard

        //Inicio Listar Usuarios en Dashboard
        [Authorize(Roles = "admin")]
        public ActionResult Lista(int? page)
        {

            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            UsuarioDAL dal = new UsuarioDAL();
            var usuarios = dal.Listar();

            var pageNumber = page ?? 1;
            ViewBag.Lista = usuarios.ToPagedList(pageNumber, 6);

            return View();
        }
        //Fin Listar Usuarios en Dashboard


        /// <summary>
        /// Metodos para login
        /// </summary>
        /// <returns>Logearse, Registrar y cerrar una cuenta de usuario</returns>


        //Inicio Ingreso de usuario
        public ActionResult Login()
        {
            if (Session["id"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        //Fin Ingreso de usuario

        //Inicio Ingreso de usuario POST
        [HttpPost]
        public ActionResult Login(Usuario model, string returnUrl)
        {
            MenajesbdEntities6 ctx = new MenajesbdEntities6();
            UsuarioDAL DAL = new UsuarioDAL();
            if (new MembershipProviter().VerificaUsuario(model.Email_usuario, model.Contrasena))
                FormsAuthentication.SetAuthCookie(model.Email_usuario, true);
                if (!String.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    string Nombre = DAL.getUsuario(model.Email_usuario);
                    int Id = DAL.getUsuarioIDporEmail(model.Email_usuario);
                    string rol = DAL.nombreRol(Id);

                    Session["user"] = Nombre;
                    Session["id"] = Id;
                    log.Info("Iniciando sesion " + model.Email_usuario + " password : " + model.Contrasena + " Rol :" + rol);

                  if (rol == "cliente") { 
                  return  RedirectToAction("Index", "Home");
                  }
                  if (rol == "admin") { 
                    return RedirectToAction("Index", "Dashboard");
                  }
                }
                return RedirectToAction("Index", "Home");
        } 
        //Fin Ingreso de usuario POST 

        //Inicio Registrar usuario Desde vista Login
        public ActionResult RegistrarUsuario()
        {

            return View();


        }
        //Fin Registrar usuario Desde vista Login

        //Inicio Registrar usuario POST Desde vista Login
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RegistrarUsuario(Usuario user)
        {

            try
            {
                int numero = Int32.Parse(user.Telefono_usuario); //23432432.
            }
            catch (Exception)
            {
                ModelState.AddModelError("Telefono_usuario", "Error! Este campo posee caracteres inválidos!");
                return View(user);
               
            }

            MenajesbdEntities6 ctx = new MenajesbdEntities6();
            UsuarioDAL DAL = new UsuarioDAL();
            if (ctx.Usuario.Any(pro => pro.Email_usuario == user.Email_usuario))
            {
                ModelState.AddModelError("Email_usuario", "Error! Este correo ya fue ingresado!");
                ViewBag.Error = "Error";
                return View(user);
            }

            if (ModelState.IsValid)
            {
                DAL.Ingresar(user);

                string Nombre = DAL.getUsuario(user.Email_usuario);
                int Id = DAL.getUsuarioIDporEmail(user.Email_usuario);
                string rol = DAL.nombreRol(Id);

                Session["user"] = Nombre;
                Session["id"] = Id;

                if (rol == "cliente")
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.Exito = "Exito";
                return View("Login", "Usuario");
            }
            else
            {
                ViewBag.Error = "Error";
                return View(user);
            }


        }
        //Fin Registrar usuario POST Desde vista Login

        //Inicio Logout usuario POST desde vistas Dashboard
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff(string id)
        {
            Request.Cookies.Remove(id);
            FormsAuthentication.SignOut();
            Session.Clear();


            return RedirectToAction("Login", "Usuario");

        }
        //Fin Logout usuario POST desde vistas Dashboard
      
        public ActionResult Pedidos(string ID,string estado)
        {
            try
            { 
            if (!IsDigitsOnly(ID))
            {
                return RedirectToAction("Login", "Usuario");
            }
            if (!IsDigitsOnly(estado))
            {
                return RedirectToAction("Login", "Usuario");
            }
            int id = Int32.Parse(ID);

            if (Session["id"] == null || id.ToString() == null || estado.ToString() == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            string idsession = Session["id"].ToString();
            string id_user = id.ToString();
            if (idsession == id_user)
            {
                ViewBag.Lista = dalven.ListarVentas_usuario(id);

                if (estado.ToString()=="1")
                {
                    ViewBag.Estado = "Exito";
                }

                return View();
            }

            return RedirectToAction("Login", "Usuario");
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }

        }
        [HttpPost]
        public ActionResult PedidosDetalle(FormCollection f)
        {
            try
            {
                ViewBag.Id_ventas = Int32.Parse(f["id_venta"]);
                string idsession = Session["id"].ToString();
                int id_usuario = Int32.Parse(f["id_user"]);
                if (idsession == f["id_user"])
                {
                    ViewBag.Detalle = ctx.VentaDetalle(id_usuario, Int32.Parse(f["id_venta"])).ToList();
                    return View();
                }
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Usuario");
            }
                     
        }

        public ActionResult PedidosDetalle()
        {
            return RedirectToAction("Login", "Usuario");
        }




        public ActionResult DetalleVentaDebito(string id_user, string id_venta)
        {
            if (id_user == null || id_venta == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            else
            { 

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "DetalleVentaDebito.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            List<VentaDetalle_Result> cm = new List<VentaDetalle_Result>();

            using (MenajesbdEntities6 dc = new MenajesbdEntities6())
            {
                cm = dc.VentaDetalle(int.Parse(id_user), int.Parse(id_venta)).ToList(); 
                 


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





    }
}