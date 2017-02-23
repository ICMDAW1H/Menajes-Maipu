using Menajes_Maipu.Models.ModeloBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menajes_Maipu.Models.DAL.Administrador
{
    class DespachoDAL
    {
        MenajesbdEntities6 ctx = new MenajesbdEntities6();
        //Listar
        public List<Despacho> Listar()
        {
    
            var lista = (from o in ctx.Despacho
                         select o).ToList();
            return lista;
        }


        public bool ingresar(Despacho d)
        {
            try
            {
                ctx.Despacho.Add(d);
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
