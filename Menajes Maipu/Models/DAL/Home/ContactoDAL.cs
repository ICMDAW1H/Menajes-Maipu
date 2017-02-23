using Menajes_Maipu.Models.ModeloBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menajes_Maipu.Models.DAL.Home
{
    class ContactoDAL
    {
     
        MenajesbdEntities6 ctx = new MenajesbdEntities6();
        public bool Ingresar(Contacto con)
        {
         
    
                ctx.Contacto.Add(con);
                ctx.SaveChanges();
                return true;
            
    
        }
        public List<Contacto> getlista(int Id_contacto)
        {
           
            var lista = (from o in ctx.Contacto
                         where o.Id_contacto == Id_contacto
                         select o).ToList();
            return lista;
        }

        public List<Contacto> Listar()
        {
    
            var lista = (from o in ctx.Contacto
                         select o).ToList();
            return lista;
        }


        public bool ModificarMensaje_consulta(int Id_contacto, string respuesta)
        {
            try
            {

                var obj = (from o in ctx.Mensaje_consulta
                           where o.Id_contacto == Id_contacto
                           select o).Single();

                obj.Estado_consuta = "Respondido";
                obj.Respuesta_consulta = respuesta;

                ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }


        }


        public bool Modificar(int Id_contacto)
        {
            try
            {

                var obj = (from o in ctx.Contacto
                           where o.Id_contacto == Id_contacto
                           select o).Single();

                obj.Estado = "Respondido";

                ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }


        }




        //Eliminar
        public bool Eliminar(int id)
        {
            try
            {   
                var obj = (from o in ctx.Contacto
                           where o.Id_contacto == id
                           select o).Single();
                ctx.Contacto.Remove(obj);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }










    }



}
