using Menajes_Maipu.Models.ModeloBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menajes_Maipu.Models.DAL.Administrador
{
   public class UsuarioMotivo
    {
        public Mensaje_consulta mensaje { get; set; }
        public virtual ICollection<UsuarioMotivo> Usuarioen { get; set; }
        public Usuario usuario { get; set; }

    

    }
}
//public class Item
//{
//    public Producto p = new Producto();
//    public Item()
//    { }

//    public Item(Producto p, int quantity)
//    {
//        this.P = p;
//        this.Quantity = quantity;
//    }

//    public Producto P
//    {
//        get
//        {
//            return p;
//        }

//        set
//        {
//            p = value;
//        }
//    }

//    public int Quantity
//    {
//        get
//        {
//            return quantity;
//        }

//        set
//        {
//            quantity = value;
//        }
//    }