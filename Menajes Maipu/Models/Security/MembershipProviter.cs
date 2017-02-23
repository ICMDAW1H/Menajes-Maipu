using Menajes_Maipu.Models.ModeloBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Menajes_Maipu.Models.Security
{
    public class MembershipProviter
    {
        MenajesbdEntities6 ctx = new MenajesbdEntities6();

        public bool VerificaUsuario(String Email, String Password)
        {

            var lista = (from o in ctx.Usuario
                         where o.Email_usuario == Email && o.Contrasena == Password
                         select o);

            if (lista.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }
    


    }
}