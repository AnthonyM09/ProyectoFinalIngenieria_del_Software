using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoFinal_PVI.Models;
using ProyectoFinal_PVI.Data;
using ProyectoFinal_PVI.Filters;

namespace ProyectoFinal_PVI.Controllers
{
    public class HabitacionesController : Controller
    {
        // GET: Habitaciones
        public ActionResult Index()
        {
            return RedirectToAction("Habitaciones", "Habitaciones");
        }
        [HttpGet]
        [ValidarSesionFilter]
        public ActionResult Habitaciones()//Acción que consulta todas las habitaciones en la BD
        {
            try
            {
                var sesion = (ProyectoFinal_PVI.Models.LoginModel)Session["nuevaSesion"];
                if(sesion != null)
                {
                    if (sesion.ClienteOEmpleado == "Empleado")
                    {
                        List<HabitacionModel> lista = null;
                        using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                        {
                            lista = (from l in db.spConsultarHabitaciones()
                                     select new HabitacionModel
                                     {
                                         idHabitacion = l.idHabitacion,
                                         nombre = l.nombre,
                                         numeroHabitacion = l.numeroHabitacion,
                                         capacidadMaxima = l.capacidadMaxima,
                                         estadoH = l.estado
                                     }).ToList();
                        }
                        return View("Lista", lista);
                    }
                    return RedirectToAction("Reservaciones", "Reservaciones");
                }           
                return RedirectToAction("Index", "Login"); 
            }
            catch
            {
                return View("Error");
            }
        }
        [HttpGet]
        [ValidarSesionFilter]
        public ActionResult Crear()//Acción que realizará un nuevo registro en la BD
        {
            try
            {
                var sesion = (ProyectoFinal_PVI.Models.LoginModel)Session["nuevaSesion"];
                if (sesion != null)
                {
                    if (sesion.ClienteOEmpleado == "Empleado")
                    {
                        List<HabitacionModel> ddnHoteles = null;
                        using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                        {
                            ddnHoteles = (from h in db.spConsultarHoteles()
                                          select new HabitacionModel
                                          {
                                              idHotel = h.idHotel,
                                              nombre = h.nombre,

                                          }).ToList();

                        }
                        ViewBag.listahotel = ddnHoteles;
                        return View("Formulario", new HabitacionModel());//Le mandos una instancia vacia para darle dinamismo a la vista
                    }
                    return RedirectToAction("Reservaciones", "Reservaciones");
                }
                return RedirectToAction("Index", "Login");
            }
            catch
            {
                return View("Error");
            }
        }
        [HttpPost]
        [ValidarSesionFilter]
        public ActionResult Crear(HabitacionModel modelo)
        {
            try
            {
                List<HabitacionModel> ddnHoteles = null;
                if (ModelState.IsValid == false)
                { 
                    using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                    {
                        ddnHoteles = (from h in db.spConsultarHoteles()
                                      select new HabitacionModel
                                      {
                                          idHotel = h.idHotel,
                                          nombre = h.nombre
                                      }).ToList();
                    }
                    ViewBag.listahotel = ddnHoteles;
                    return View("Formulario", modelo);
                }//Termina la validación del modelo

                var numeroHabitacion = "";//Variable local que me recibe lo que me trae el procedimiento almacenado
                using(PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                {
                    numeroHabitacion = (from h in db.spConsultarHabitacionesPorIdHotel(modelo.idHotel, modelo.numeroHabitacion)
                                        select h).FirstOrDefault();//Procedimiento almancenado que me devuelve el número de habitación para no repertirlo
                    if (numeroHabitacion != modelo.numeroHabitacion)
                    {
                        db.spCrearHabitacion(modelo.idHotel, modelo.numeroHabitacion, modelo.capacidadMaxima, modelo.descripcion, modelo.estadoH = "A");
                        ViewBag.mensaje = "Se ha creado una nueva habitación en la base de datos";
                        return View("Resultado", modelo);
                    }//Fin del if (numeroHabitacion != modelo.numeroHabitacion)

                    ddnHoteles = (from h in db.spConsultarHoteles()
                                  select new HabitacionModel
                                  {
                                      idHotel = h.idHotel,
                                      nombre = h.nombre
                                  }).ToList();
                }
                ViewBag.listahotel = ddnHoteles;
                TempData["msg"] = "<script>alert('El número de habitación ya existe en la base de datos');</script>";
                return View("Formulario", modelo);
            }
            catch
            {
                return View("Error");
            }
        }
        [HttpGet]
        [ValidarSesionFilter]
        public ActionResult Editar(int id)//Acción que me devuelve la consulta por id de la habitación selecionada por el usuario
        {
            try
            {

                var sesion = (ProyectoFinal_PVI.Models.LoginModel)Session["nuevaSesion"];
                List<HabitacionModel> ddnHoteles = null;
                HabitacionModel habitacionesDB = null;
                ReservacionModel reservacion = null;
                if(sesion != null) 
                {
                    if (sesion.ClienteOEmpleado == "Cliente")
                    {
                        return RedirectToAction("Reservaciones", "Reservaciones");
                    }
                    //Validación que me verifica el estado de la habitación a editar              
                using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                {
                    reservacion = (from r in db.spConsultarReservacionPorNumeroHabitacion(id)
                                   select new ReservacionModel 
                                   {                                      
                                       fechaSalida = r.fechaSalida,
                                       estado = r.estado
                                   }).FirstOrDefault();

                    habitacionesDB = (from h in db.spConsultarHabitacionPorId(id)
                                      select new HabitacionModel
                                      {
                                          idHotel = h.idHotel,
                                          idHabitacion = h.idHabitacion,
                                          nombre = h.nombre,
                                          numeroHabitacion = h.numeroHabitacion,
                                          capacidadMaxima = h.capacidadMaxima,
                                          descripcion = h.descripcion,
                                          estadoH = h.estado
                                      }).FirstOrDefault();

                    if (habitacionesDB.estadoH == "I" || reservacion?.fechaSalida >= DateTime.Now)
                    {
                        return RedirectToAction("Habitaciones");
                    }//Hacemos la validación para no permitir la modificación de las habitaciones que estan reservadas. 
                    ddnHoteles = (from h in db.spConsultarHoteles()
                                  select new HabitacionModel
                                  {
                                      idHotel = h.idHotel,
                                      nombre = h.nombre
                                  }).ToList();
                    
                    habitacionesDB = (from h in db.spConsultarHabitacionPorId(id)
                                      select new HabitacionModel
                                      {
                                          idHotel = h.idHotel,
                                          idHabitacion = h.idHabitacion,
                                          nombre = h.nombre,
                                          numeroHabitacion = h.numeroHabitacion,
                                          capacidadMaxima = h.capacidadMaxima,
                                          descripcion = h.descripcion,
                                          estadoH = h.estado
                                      }).FirstOrDefault();
                }
                ViewBag.listahotel = ddnHoteles;
                return View("Formulario", habitacionesDB);//Retornamos la vista Editar para realizar los cambios necesarios
                }
                return RedirectToAction("Index", "Login");
            }
            catch
            {
                return View("Error");
            }
        }
        [HttpPost]
        [ValidarSesionFilter]
        public ActionResult Editar(HabitacionModel modelo)
        {
            try
            {
                var sesion = (ProyectoFinal_PVI.Models.LoginModel)Session["nuevaSesion"];
                if (ModelState.IsValid == false)//Verificamos que el modelo cumpla con las anotaciones de datos
                {
                    return View("Formulario", modelo);
                }
                //var numeroHabitacion = "";//Variable local que me recibe lo que me trae el procedimiento almacenado
                using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                {
                    /*numeroHabitacion = (from n in db.pa_ConsultarNumerodeHabitaciones(modelo.numeroHabitacion)
                                        select n).FirstOrDefault();     
                    
                        if (numeroHabitacion == modelo.numeroHabitacion )//Realizamos la actualización de los datos
                        {                      
                        TempData["msg"] = "<script>alert('El número de habitación ya existe en la base de datos');</script>";
                        return View("Formulario", modelo);
                        }*/

                    db.spEditarHabitacion(modelo.idHabitacion, modelo.idHotel, modelo.numeroHabitacion, modelo.capacidadMaxima, modelo.descripcion, modelo.estadoH = "A");                
                }
                ViewBag.Mensaje = "Se han modificado los datos de la habitación en la base de datos";
                return View("Resultado", modelo);
            }
            catch
            {
                return View("Error");
            }
        }
        [HttpGet]
        [ValidarSesionFilter]
        public ActionResult InactivarHabitacion(int id)
        {
            try
            {
                using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                {
                    db.spInactivarHabitacion(id);
                }               
                return RedirectToAction("Habitaciones");
            }
            catch
            {
                return View("Error");
            }
        }
    }
}