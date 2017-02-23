using Menajes_Maipu.Models.ModeloBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Menajes_Maipu.Models.Security
{
    public class MyRoleProvider : RoleProvider
    {
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
        // string username en realidad es el Email_usuario 
        public override string[] GetRolesForUser(string username)
        {
            using ( MenajesbdEntities6 db = new MenajesbdEntities6())
            {
                Usuario user = db.Usuario.FirstOrDefault(u => u.Email_usuario.Equals(username, StringComparison.CurrentCultureIgnoreCase));

                var roles = (from ur in user.UserInRol
                             from r in db.Rol
                             where ur.id_rol == r.id_rol
                             select r.Nombre);
                if (roles != null)
                    return roles.ToArray();
                else
                    return new string[] { }; ;
            }



        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (MenajesbdEntities6 db = new MenajesbdEntities6())
            {
                Usuario user = db.Usuario.FirstOrDefault(u => u.Nombre_usuario.Equals(username, StringComparison.CurrentCultureIgnoreCase) || u.Email_usuario.Equals(username, StringComparison.CurrentCultureIgnoreCase));

                var roles = from ur in user.UserInRol
                            from r in db.Rol
                            where ur.id_rol == r.id_rol
                            select r.Nombre;
                if (user != null)
                    return roles.Any(r => r.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));
                else
                    return false;
            }
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