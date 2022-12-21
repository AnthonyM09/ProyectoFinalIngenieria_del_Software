using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal_PVI.Models
{
    public class PersonaModel 
    {
        public int idPersona { set; get; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Nombre completo")]
        [StringLength(250, ErrorMessage = "La longitud de este campo debe de estar entre 1 y 250 caracteres", MinimumLength = 1)]
        public string nombreCompleto { set; get; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Email")]
        [StringLength(150, ErrorMessage = "La longitud de este campo debe de estar entre 1 y 150 caracteres", MinimumLength = 1)]
        public string email { set; get; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Contraseña")]
        [StringLength(15, ErrorMessage = "La longitud máxima de este este campo es de 15 caracteres", MinimumLength = 1)]
        public string clave { set; get; }

        [Required(ErrorMessage = "Campo requerido")]
        public bool esEmpleado { set; get; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Contraseña")]
        [StringLength(1, ErrorMessage = "La longitud máxima de este este campo debe ser de 1 caracter", MinimumLength = 1)]
        public string estadoP { set; get; }

        //Método que realiza la clasificación de cliente o empleado
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
    }
}