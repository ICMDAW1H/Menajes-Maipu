using Menajes_Maipu.Models.ModeloBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
namespace Menajes_Maipu.Models.DAL
{
    public class Seguridad : RoleProvider
    {



        public override string[] GetRolesForUser(string Nombre_usuario)
        {
            using (MenajesbdEntities6 ctx = new MenajesbdEntities6())
            {
                var objUser = ctx.Usuario.FirstOrDefault(x => x.Nombre_usuario == Nombre_usuario);
                if (objUser == null)
                {
                    return null;
                }
                else
                {
                    string[] ret = ctx.Rol.Select(x => x.Nombre).ToArray();
                    return ret;
                }
            }
        }



        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}