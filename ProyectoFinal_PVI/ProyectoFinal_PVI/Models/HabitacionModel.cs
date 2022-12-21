using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal_PVI.Models
{
    public class HabitacionModel
    {
        public int idHabitacion { set; get; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Número de habitación")]
        [StringLength(10, ErrorMessage = "La longitud de este campo debe de estar entre 1 y 10 caracteres", MinimumLength = 1)]
        public string numeroHabitacion { set; get;}// Este valor no puede estar duplicado para un mismo hotel
                                                    // (no puede haber dos números de habitaciones iguales para el mismo hotel).
       
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Capacidad máxima")]
        [Range(1, 8, ErrorMessage = "Este campo debe ser mayor a cero, y menor o igual a 8")]
        public int capacidadMaxima { set; get; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Descripción")]
        [StringLength(500, ErrorMessage = "La longitud de este campo debe de estar entre 1 y 500 caracterers", MinimumLength = 1)]
        public string descripcion { set; get; }

        [Display(Name = "Estado")]
        [StringLength(1)]
        public string estadoH { set; get; }
        public string estadoHabitacion
        {
            get
            {
                if (this.estadoH == "I")
                {
                    return "Inactiva";
                }
                if(this.estadoH == "A")
                {
                    return "Activa";
                }
                return "";
            }
        }
        //Otros atributos
        [Required(ErrorMessage = "Campo requerido")]
        public int idHotel { set; get; }

        public string nombre { set; get; }

    }
}