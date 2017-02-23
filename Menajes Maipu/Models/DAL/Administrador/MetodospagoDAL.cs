using Menajes_Maipu.Models.ModeloBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menajes_Maipu.Models.DAL.Administrador
{
    class MetodospagoDAL
    {




        MenajesbdEntities6 ctx = new MenajesbdEntities6();
        //Listar
        public List<Metodo_pago> Listar()
        {

            var lista = (from o in ctx.Metodo_pago
                         select o).ToList();
            return lista;
        }


        public bool ingresar(Metodo_pago m)
        {
            try
            {
                ctx.Metodo_pago.Add(m);
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
