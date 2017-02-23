using Menajes_Maipu.Models.ModeloBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Menajes_Maipu.Models.DAL.Administrador
{
    public class BancoDAL
    {
        MenajesbdEntities6 ctx = new MenajesbdEntities6();
     
        
        
        public List<Banco_menaje> Listar()
        {
       
            var lista = (from o in ctx.Banco_menaje
                         select o).ToList();
            return lista;
        }

        public List<Banco_menaje> Listar_por_id(string Num_cuenta)
        {

            var lista = (from o in ctx.Banco_menaje
                         where o.Num_cuenta == Num_cuenta
                         select o).ToList();

            return lista;
        }

        public bool Eliminar(Banco_menaje b)
        {

            try
            {
                var banco = ctx.Banco_menaje.Where(bc => bc.Num_cuenta == b.Num_cuenta).Single();
                ctx.Banco_menaje.Remove(banco);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool Ingresar(Banco_menaje b)
        {

            try
            {
                ctx.Banco_menaje.Add(b);
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