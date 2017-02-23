using Menajes_Maipu.Models.ModeloBD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Menajes_Maipu.Models.DAL.Administrador
{
    public class UsuarioDAL
    {
   MenajesbdEntities6 ctx = new MenajesbdEntities6();
   public bool Ingresar(Usuario user)
   {
       try
       {
        

                ctx.Usuario.Add(user);
           ctx.SaveChanges();

           UserInRol UserInROL = new UserInRol();

           UserInROL.id_rol = (user.Tipo);
           UserInROL.id_user = user.id_user;

           ctx.UserInRol.Add(UserInROL);
           ctx.SaveChanges();


           return true;
       }
       catch (Exception)
       {
           return false;
       }
   }

        public List<Mensaje_consulta> Listar_por_id_contacto(int Id_contacto)
        {
            var lista = ctx.Mensaje_consulta.Where(m => m.Id_contacto == Id_contacto).ToList();
            return lista;
        }

        public List<Usuario> Listar()
        {
            MenajesbdEntities6 ctx = new MenajesbdEntities6();
            var lista = (from o in ctx.Usuario
                         select o).ToList();
            return lista;
        }

        public List<Rol> ListarRol()
        {
            MenajesbdEntities6 ctx = new MenajesbdEntities6();
            var lista = (from o in ctx.Rol
                         select o).ToList();
            return lista;
        }



        // recibe Nombre Usuario, devuelve ID
        public int getUsuarioID(string Nombre_usuario)
        {
            MenajesbdEntities6 ctx = new MenajesbdEntities6();
           int lista = (from o in ctx.Usuario
                            where o.Nombre_usuario == Nombre_usuario
                            select o.id_user).SingleOrDefault();
            return lista;
        }

        // recibe EMAIL, devuelve ID
        public int getUsuarioIDporEmail(string Email_usuario)
        {
            MenajesbdEntities6 ctx = new MenajesbdEntities6();
            int lista = (from o in ctx.Usuario
                         where o.Email_usuario == Email_usuario
                         select o.id_user).SingleOrDefault();
            return lista;
        }


        public string nombreRol(int id_user)
        {
            MenajesbdEntities6 ctx = new MenajesbdEntities6();
            string lista = (from ur in ctx.UserInRol
                            from r in ctx.Rol
                            where ur.id_rol== r.id_rol && ur.Usuario.id_user == id_user
                            select r.Nombre).SingleOrDefault();
            return lista;
        }



        // obtiene nombre de usuario buscando por correo

        public string getUsuario(string Email_usuario)
        {
            MenajesbdEntities6 ctx = new MenajesbdEntities6();
            string lista = (from o in ctx.Usuario
                         where o.Email_usuario == Email_usuario
                            select o.Nombre_usuario).SingleOrDefault();
  
            return lista;

            
        }

        // obtiene todos los campos de un usuario buscando id
        public List<Usuario> getUsuarioALLDATA(int id)
        {
            MenajesbdEntities6 ctx = new MenajesbdEntities6();
            var lista = (from o in ctx.Usuario
                            where o.id_user == id
                            select o).ToList();

            return lista;


        }
                        


        public bool Modificar(Usuario user)
        {
            Usuario u = ctx.Usuario.Single(c => c.id_user == user.id_user);

            u.Rut = user.Rut;
            u.Nombre_usuario = user.Nombre_usuario;
            u.Apellido_usuario = user.Apellido_usuario;
            u.Email_usuario = user.Email_usuario;
            u.Telefono_usuario = user.Telefono_usuario;
            u.Sexo = user.Sexo;
            u.Contrasena = user.Contrasena;
            u.Tipo = user.Tipo;

            UserInRol urol = ctx.UserInRol.Single(c => c.id_user == user.id_user);

            urol.id_rol = user.Tipo;

            ctx.SaveChanges();
            return true;

        }

        public bool ModificarPassword(Usuario user)
        {
            Usuario u = ctx.Usuario.Single(c => c.id_user == user.id_user);

            u.Contrasena = user.Contrasena;

            ctx.SaveChanges();
            return true;

        }


public List<Mensaje_consulta> Listar_por_id(int id)
        {
          var lista =  ctx.Mensaje_consulta.Where(m => m.id_user == id).OrderByDescending(x => x.Id_contacto).ToList();
            return lista;
        }



    }
}