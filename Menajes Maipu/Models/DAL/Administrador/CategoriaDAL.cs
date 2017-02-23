using Menajes_Maipu.Models.ModeloBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menajes_Maipu.Models.DAL.Administrador
{
    class CategoriaDAL
    {
        //Listar
        public List<Categoria> Listar()
        {
            MenajesbdEntities6 ctx = new MenajesbdEntities6();
            var lista = (from o in ctx.Categoria
                         select o).ToList();
            return lista;
        }
        public List<Categoria> Listarcindex()
        {
            MenajesbdEntities6 ctx = new MenajesbdEntities6();
            var lista = (from o in ctx.Categoria
                         select new
                         {
                             o.Id_categoria,
                             o.Nombre_categoria
                         });
            var lista2 = (from o in lista.AsEnumerable()
                          select new Categoria
                          {
                              Id_categoria = o.Id_categoria,
                              Nombre_categoria = o.Nombre_categoria 
                          });
            return lista2.ToList();
        }






        //public List<Producto> getBusquedap(string buscarproducto)
        //{
        //    MenajesbdEntities6 ctx = new MenajesbdEntities6();
        //    var lista = (from o in ctx.Producto
        //                 where o.Nombre_producto.Contains(buscarproducto)
        //                 select new
        //                 {
        //                     o.Id_producto,
        //                     o.Imagen_producto,
        //                     o.Nombre_producto,
        //                     o.Precio_producto
        //                 });
        //    var lista2 = (from o in lista.AsEnumerable()
        //                  select new Producto
        //                  {
        //                      Id_producto = o.Id_producto,
        //                      Imagen_producto = o.Imagen_producto,
        //                      Nombre_producto = o.Nombre_producto,
        //                      Precio_producto = o.Precio_producto
        //                  });
        //    return lista2.ToList();



        //}








        public List<Categoria> getlista(int Id_categoria)
        {
            MenajesbdEntities6 ctx = new MenajesbdEntities6();
            var lista = (from o in ctx.Categoria
                         where o.Id_categoria == Id_categoria
                         select o).ToList();
            return lista;
        }
    

        //Ingresar
        public bool Ingresar(Categoria cat)
        {
            try
            {
                MenajesbdEntities6 ctx = new MenajesbdEntities6();
                ctx.Categoria.Add(cat);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Modificar
        public bool Modificar(int Id_categoria, string Nombre_categoria)
        {
            try
            {
                MenajesbdEntities6 ctx = new MenajesbdEntities6();
                var obj = (from o in ctx.Categoria
                           where o.Id_categoria == Id_categoria
                           select o).Single();

                obj.Nombre_categoria = Nombre_categoria;

                ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }


        }


        //Eliminar
        public bool Eliminar(int Id_categoria)
        {
            try
            {
                MenajesbdEntities6 ctx = new MenajesbdEntities6();
                var obj = (from o in ctx.Categoria
                           where o.Id_categoria == Id_categoria
                           select o).Single();

                ctx.Categoria.Remove(obj);
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
