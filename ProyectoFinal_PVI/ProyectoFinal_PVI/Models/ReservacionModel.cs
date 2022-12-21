using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal_PVI.Models
{
    public class ReservacionModel 
    {
        public int idReservacion { set; get; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Fecha de entrada")]
        public DateTime fechaEntrada { set; get; } //Se debe de validar que el dato que se registre en esta
                                                   //columna no sea menor a la fecha actual de la máquina en la que se está ejecutando la aplicación.
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Fecha de salida")]
        public DateTime fechaSalida { set; get; } //se debe de validar que el dato que se registre en esta
                                                  //columna no sea menor a la fecha de entrada de la reservación.
        [Required(ErrorMessage = "Campo requerido")]
        [Range(1, 9, ErrorMessage = "El número de adultos debe de ser mayor a 0")]
        [Display(Name = "Número de adultos")]
        public int numeroAdultos { set; get; } = 1; //Este dato debe ser mayor a cero,
                                               //y menor o igual a la capacidad máxima de la habitación reservada (si la cantidad de niños es cero).
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Número de niños")]
        public int numeroNihnos { set; get; } //Este dato debe ser mayor o igual que cero,
                                              //y menor o igual a capacidad máxima de la habitación reservada menos una unidad (si la cantidad de adultos es igual a 1)
        
        [Display(Name = "Total de días de reservación")]
        public int totalDiasReservacion
        { get {
                TimeSpan totalDiasReservados = (this.fechaSalida - this.fechaEntrada);
                int dias = totalDiasReservados.Days;
                return dias;
            } } //Este dato no se le debe de solicitar al usuario,
                                                      //ya que su valor se debe calcular a partir de la diferencia que exista entre las fechas de entrada y salida de la reservación.
                                                      //En caso de que las fechas de entrada y salida de la reservación sean modificadas, este valor debe de volverse a calcular a partir de los nuevos valores antes mencionados.
        [Display(Name = "Costo total")]
        public decimal costoTotal { set; get; }
        public decimal calcularCostoTotal//Método que me calcula el costo total de la reservación
        {
            get
            {
                return this.totalDiasReservacion * (this.numeroAdultos * this.costoPorCadaAdulto) + (this.numeroNihnos * this.costoPorCadaNihno);
            }      
        }
        [Display(Name = "Fecha de creación")]
        public DateTime fechaCreacion { set; get; } //Este dato no debe ser solicitado al usuario; ya que su valor se debe obtener a
                                                    //partir de la fecha y hora del equipo de cómputo en el que se está ejecutando el proyecto.
        [Display(Name = "Fecha de modificación")]
        public DateTime? fechaModificacion { set; get; } //Este dato corresponde a la fecha y hora en la que se realiza la última modificación de los datos de la reservación seleccionada por el usuario, en la base de datos. Este dato no es requerido para insertar los datos en base de datos,
                                                         //pero sí es REQUERIDO para hacer cualquier modificación en los datos de una reservación.
        
        [Display(Name = "Estado")]
        
        public string estado { set; get; }

        //Método que me permite saber el estado de cada reservación
        public string estadoActual 
        { get
            {
                if(this.estado == "I")
                {
                    return "Cancelada";
                }
                if(this.estado == "A" && this.fechaSalida < DateTime.Now)
                {
                    return "Finalizada";
                }
                if(this.estado == "A" && this.fechaEntrada <= DateTime.Now)
                {
                    return "En proceso";
                }
                if(this.estado == "A" && this.fechaEntrada > DateTime.Now && this.fechaSalida > DateTime.Now)
                {
                    return "En espera";
                }
                return "";
            } 
        }
        //Otros atributos
        [Display(Name = "Cliente")]
        public string nombreCompleto { set; get; }
        [Required(ErrorMessage = "Campo requerido")]
        public int idPersona { set; get; }
        public int idHabitacion { set; get; }
        [Display(Name ="Número de habitación")]
        public string numeroHabitacion { set; get; }
        [Display(Name = "Hotel")]
        public string nombre { set; get; }
        [Required(ErrorMessage = "Campo requerido")]
        public int idHotel { set; get; }
        public decimal costoPorCadaAdulto { set; get; } //Este dato no se debe de solicitar al usuario, ya que se obtiene de la tabla llamada Hotel.      
        public decimal costoPorCadaNihno { set; get; } //Este dato no se debe de solicitar al usuario, ya que se obtiene de la tabla llamada Hotel.

        public int capacidadMaxima { set; get; }

        public string nombrebitacora { set; get; }

        public string accionRealizada { set; get; }
        public DateTime fechaDeLaAccion { set; get; }
    }
}