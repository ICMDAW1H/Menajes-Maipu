using Menajes_Maipu.Models.ModeloBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menajes_Maipu.Models.DAL.Administrador
{
    class ProductosDAL
    {
        MenajesbdEntities6 ctx = new MenajesbdEntities6();
        //Listar
        public List<Producto> Listar()
        {
            var lista = (from o in ctx.Producto
                         orderby o.Id_producto descending
                         select o).ToList();
            return lista;
        }
        public List<Producto> Listarpindex()
        {
         
            var lista = (from o in ctx.Producto orderby o.Id_producto descending
                         select new   {o.Id_producto,
                                                            o.Imagen_producto,
                                                            o.Nombre_producto, 
                                                            o.Precio_producto});
            var lista2 = (from o in lista.AsEnumerable()
                          select new Producto
                          {
                            Id_producto =  o.Id_producto,
                            Imagen_producto = o.Imagen_producto,
                            Nombre_producto = o.Nombre_producto,
                            Precio_producto = o.Precio_producto
                          });
            return lista2.ToList();
        } 
        public List<Producto> getBusquedap(string buscarproducto)
        {        
                var lista = (from o in ctx.Producto
                             orderby o.Id_producto descending
                             where o.Nombre_producto.Contains(buscarproducto)
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
        public List<Producto> getlista(int Id_producto)
        {
          
            var lista = (from o in ctx.Producto
                         where o.Id_producto == Id_producto
                         select o).ToList();
            return lista;
        }
        public List<Producto> getBusquedasc(int Id_subcategoria)
        {
  
            var lista = (from o in ctx.Producto where o.Id_subcategoria == Id_subcategoria
                         orderby o.Id_producto descending
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
        public bool Ingresar(Producto pro)
        {
            try
            {
                pro.Tipo = 0;
                ctx.Producto.Add(pro);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

  
        
        //Modificar

        //public bool Modificar(int Id_producto, int Id_subcategoria, string Nombre_producto,byte[] Imagen_producto, string Nombre_imagen, 
        //                        string Descripcion_producto, Int32 Stock_producto, Int32 Precio_producto)
        //{
        //    try
        //    {
        //        MenajesbdEntities6 ctx = new MenajesbdEntities6();
        //        var obj = (from o in ctx.Producto
        //                   where o.Id_producto == Id_producto
        //                   select o).Single();

        //        obj.Id_subcategoria = Id_subcategoria;
        //        obj.Nombre_producto = Nombre_producto;
        //        obj.Imagen_producto = Imagen_producto;
        //        obj.Nombre_imagen = Nombre_imagen;
        //        obj.Descripcion_producto = Descripcion_producto;
        //        obj.Stock_producto = Stock_producto;
        //        obj.Precio_producto = Precio_producto;

        //        ctx.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }


        //}

        public bool Modificar(Producto pro)
        {
            try
            {

                var obj = (from o in ctx.Producto
                           where o.Id_producto == pro.Id_producto
                           select o).Single();
                obj.Id_subcategoria = pro.Id_subcategoria;
                obj.Nombre_producto = pro.Nombre_producto;
                obj.Imagen_producto = pro.Imagen_producto;
                obj.Nombre_imagen = pro.Nombre_imagen;
                obj.Descripcion_producto = pro.Descripcion_producto;
                obj.Stock_producto = pro.Stock_producto;
                obj.Precio_producto = pro.Precio_producto;
                obj.Tipo = pro.Tipo;
                ctx.SaveChanges();
                return true;
            } catch (Exception){
                return false;
 
            }
                           }


      





















        //Eliminar
        public bool Eliminar(int Id_producto)
        {
            try
            {
                MenajesbdEntities6 ctx = new MenajesbdEntities6();
                var obj = (from o in ctx.Producto
                           where o.Id_producto == Id_producto
                           select o).Single();

                ctx.Producto.Remove(obj);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }


        //public bool modificarexiste(int id)
        //{

        //        var obj = (from o in ctx.Producto
        //                   where o.Id_producto == id
        //                   select o.Descripcion_producto).Single();
        //        if (obj != null)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }


        //}


        public bool ModificarTipo(int id, int tipo)
        {

          

            Producto pro = ctx.Producto.Where(c => c.Tipo == tipo).FirstOrDefault();
            if (pro != null)
                {
                    pro.Tipo = 0;
                    ctx.SaveChanges();
                }
  

                var obj2 = (from o in ctx.Producto
                            where o.Id_producto == id
                            select o).Single();
                obj2.Tipo = tipo;
                ctx.SaveChanges();
                return true;
                
            


        }



    }
}
