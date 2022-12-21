using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ProyectoFinal_PVI.Data;
namespace ProyectoFinal_PVI.Models
{ 
    public class LoginModel
    {

        public int idPersona { set; get; }
        public string nombreCompleto { set; get; }

        public bool esEmpleado { set; get; }
        public string ClienteOEmpleado
        {
            get
            {
                if (this.esEmpleado == false)
                {
                    return "Cliente";
                }
                return "Empleado";
            }
        }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string email { set; get; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Contraseña")]
        public string clave { set; get; }

    }
}