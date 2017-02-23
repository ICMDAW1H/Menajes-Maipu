using Menajes_Maipu.Models.ModeloBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menajes_Maipu.Models.DAL.Administrador
{
    class DebitoDAL
    {
        MenajesbdEntities6 ctx = new MenajesbdEntities6();
        public bool Modificar(Debito pro)
        {
           
                var obj = (from o in ctx.Debito
                           where o.Id_ventas == pro.Id_ventas
                           select o).Single();
                obj.Imagen = pro.Imagen;
                obj.Cod_deposito = pro.Cod_deposito;
                obj.Num_cuenta = pro.Num_cuenta;
            obj.Fecha_deposito = pro.Fecha_deposito;
            obj.N_img = pro.N_img;

                ctx.SaveChanges();

            var obj2 = ctx.Ventas.Where(v => v.Id_ventas == pro.Id_ventas).SingleOrDefault();
            obj2.Estado = "VERIFICANDO";
            ctx.SaveChanges();
                return true;



        }


        public List<Debito> Listar()
        {
            var lista = ctx.Debito.Select(c => c).ToList();
            return lista;
        }










    }
}
