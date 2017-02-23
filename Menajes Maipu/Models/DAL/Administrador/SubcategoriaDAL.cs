using Menajes_Maipu.Models.ModeloBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menajes_Maipu.Models.DAL.Administrador
{
    class SubcategoriaDAL
    {

        //Listar
        public List<Subcategoria> Listar()
        {
            MenajesbdEntities6 ctx = new MenajesbdEntities6();
            var lista = (from o in ctx.Subcategoria
                         select o).ToList();
            return lista;
        }

        public List<Subcategoria> ListarFiltrado()
        {
            MenajesbdEntities6 ctx = new MenajesbdEntities6();
            var lista = (from o in ctx.Subcategoria 
                         select         o).ToList();
            return lista;
        }



        public List<Subcategoria> Listarscindex()
        {
            MenajesbdEntities6 ctx = new MenajesbdEntities6();
            var lista = (from o in ctx.Subcategoria
                         select new
                         {
                             o.Id_categoria,
                             o.Nombre_subcategoria,
                             o.Id_subcategoria
                         });
            var lista2 = (from o in lista.AsEnumerable()
                          select new Subcategoria
                          {
                              Id_categoria = o.Id_categoria,
                             Nombre_subcategoria = o.Nombre_subcategoria,
                             Id_subcategoria = o.Id_subcategoria
                          });
            return lista2.ToList();
        }





        //Ingresar
        public bool Ingresar(Subcategoria subcat)
        {
            try
            {
                MenajesbdEntities6 ctx = new MenajesbdEntities6();
                ctx.Subcategoria.Add(subcat);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Subcategoria> getlista(int Id_subcategoria)
        {
            MenajesbdEntities6 ctx = new MenajesbdEntities6();
            var lista = (from o in ctx.Subcategoria
                         where o.Id_subcategoria == Id_subcategoria
                         select o).ToList();
            return lista;
        }


        //Modificar
        public bool Modificar(int Id_subcategoria, string Nombre_subcategoria, int Id_categoria )
        {
            try
            {
                MenajesbdEntities6 ctx = new MenajesbdEntities6();
                var obj = (from o in ctx.Subcategoria
                           where o.Id_subcategoria == Id_subcategoria
                           select o).Single();

                obj.Nombre_subcategoria = Nombre_subcategoria;
                obj.Id_categoria = Id_categoria;

                ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }


        }


        //Eliminar
        public bool Eliminar(int Id_subcategoria)
        {
            try
            {
                MenajesbdEntities6 ctx = new MenajesbdEntities6();
                var obj = (from o in ctx.Subcategoria
                           where o.Id_subcategoria == Id_subcategoria
                           select o).Single();

                ctx.Subcategoria.Remove(obj);
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
