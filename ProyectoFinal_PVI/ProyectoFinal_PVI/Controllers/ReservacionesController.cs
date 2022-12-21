using ProyectoFinal_PVI.Data;
using ProyectoFinal_PVI.Filters;
using ProyectoFinal_PVI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ProyectoFinal_PVI.Controllers
{
    public class ReservacionesController : Controller
    {
        // GET: proyectoFinalPVI
        [HttpGet]
        public ActionResult Index()
        {
            return View("");
        }
        [HttpGet]
        [ValidarSesionFilter]
        public ActionResult Reservaciones()
        {
            try
            {
                //Realizo el casteo de las variables de sesión para su uso.
                var sesion = (ProyectoFinal_PVI.Models.LoginModel)Session["nuevaSesion"];
                if (sesion != null)//Valido la sesión actual 
                {
                    List<ReservacionModel> listaReservaciones = null;
                    using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())//Creamos la conexión a la base de datos
                    {
                        listaReservaciones = (from r in db.spConsultarReservacionesPorId(sesion.idPersona)
                                              select new ReservacionModel
                                              {
                                                  idReservacion = r.idReservacion,
                                                  nombre = r.nombre,
                                                  fechaEntrada = r.fechaEntrada,
                                                  fechaSalida = r.fechaSalida,
                                                  costoTotal = r.costoTotal,
                                                  estado = r.estado
                                              }).ToList();
                    }
                    return View("MisReservaciones", listaReservaciones); //Llenamos la primera lista que va a ver el usuario
                }
                return RedirectToAction("Salir", "Login"); //Si la sesión es nula redirecionamos a la acción salir
              
            }
            catch
            {
                return View("Error");
            }
        }
        [HttpGet]
        [ValidarSesionFilter]
        public ActionResult DetalleReservacion(int id)
        {
            try
            {
                var sesion = (ProyectoFinal_PVI.Models.LoginModel)Session["nuevaSesion"];
                if(sesion != null)
                {
                    ReservacionModel nombreCompleto = null;
                    using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                    {
                        nombreCompleto = (from d in db.spConsultarDetalleReservacionPorId(id)
                                          select new ReservacionModel
                                          {
                                              nombreCompleto = d.nombreCompleto
                                          }).FirstOrDefault();
                    }
                    if (sesion.ClienteOEmpleado == "Empleado")//Realizamos la validacion si es cliente o empleado
                    {
                        ReservacionModel detalleBD = null;
                        using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                        {
                            detalleBD = (from d in db.spConsultarDetalleReservacionPorIdB(id)
                                         select new ReservacionModel
                                         {
                                             idReservacion = d.idReservacion,
                                             nombre = d.nombre,
                                             numeroHabitacion = d.numeroHabitacion,
                                             nombreCompleto = d.nombreCompleto,
                                             fechaEntrada = d.fechaEntrada,
                                             fechaSalida = d.fechaSalida,
                                             numeroAdultos = d.numeroAdultos,
                                             numeroNihnos = d.numeroNinhos,
                                             costoTotal = d.costoTotal,
                                             estado = d.estado

                                         }).FirstOrDefault();
                        }
                        List<BitacoraModel> bitacoraDB = null;
                        using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                        {
                            bitacoraDB = (from b in db.spConsultarAccionesRealizadasPorId(id)
                                          select new BitacoraModel
                                          {
                                              fechaDeLaAccion = b.fechaDeLaAccion,
                                              accionRealizada = b.accionRealizada,
                                              nombre = b.nombreCompleto

                                          }).ToList();
                        }
                        ViewBag.bitacora = bitacoraDB;
                        return View(detalleBD);
                    }//Fin if empleado
                    else
                    {
                        if (sesion.nombreCompleto == nombreCompleto.nombreCompleto)//Realizamos la verificación de el usuario para que pueda acceder solo a los datos
                                                                                   //que le pertenecen según la BD          
                        {
                            ReservacionModel detalleBD = null;
                            using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                            {
                                detalleBD = (from d in db.spConsultarDetalleReservacionPorIdB(id)
                                             select new ReservacionModel
                                             {
                                                 idReservacion = d.idReservacion,
                                                 nombre = d.nombre,
                                                 numeroHabitacion = d.numeroHabitacion,
                                                 nombreCompleto = d.nombreCompleto,
                                                 fechaEntrada = d.fechaEntrada,
                                                 fechaSalida = d.fechaSalida,
                                                 numeroAdultos = d.numeroAdultos,
                                                 numeroNihnos = d.numeroNinhos,
                                                 costoTotal = d.costoTotal,
                                                 estado = d.estado


                                             }).FirstOrDefault();
                            }
                            List<BitacoraModel> bitacoraDB = null;
                            using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                            {
                                bitacoraDB = (from b in db.spConsultarAccionesRealizadasPorId(id)
                                              select new BitacoraModel
                                              {
                                                  fechaDeLaAccion = b.fechaDeLaAccion,
                                                  accionRealizada = b.accionRealizada,
                                                  nombre = b.nombreCompleto

                                              }).ToList();
                            }
                            ViewBag.bitacora = bitacoraDB;
                            return View(detalleBD);
                        }//Fin if cliente
                        return RedirectToAction("Reservaciones", "Reservaciones"); //Hacemos un redirecionamiento hacia la pantalla de mis reservaciones si el cliente intenta acceder a otra información
                    }
                }
                return RedirectToAction("Index", "Login");                              
            }
            catch
            {
                return View("Error");
            }
        }
        [HttpGet]
        [ValidarSesionFilter]//Validamos la sesión
        public ActionResult GestionarReservaciones()//Acción solo habilitada para los empleados
        {
            try
            {
                var sesion = (ProyectoFinal_PVI.Models.LoginModel)Session["nuevaSesion"];
                if(sesion != null)
                {
                    if (sesion.ClienteOEmpleado == "Empleado")
                    {
                        List<ReservacionModel> ddnClientes = null;
                        List<ReservacionModel> listaBD = null;
                        using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                        {
                            ddnClientes = (from c in db.spConsultarPersonas()
                                           select new ReservacionModel
                                           {
                                               idPersona = c.idPersona,
                                               nombreCompleto = c.nombreCompleto
                                           }).ToList();

                            listaBD = (from r in db.spConsultarReservaciones()
                                       select new ReservacionModel
                                       {
                                           idReservacion = r.idReservacion,
                                           nombreCompleto = r.nombreCompleto,
                                           nombre = r.nombre,
                                           fechaEntrada = r.fechaEntrada,
                                           fechaSalida = r.fechaSalida,
                                           costoTotal = r.costoTotal,
                                           estado = r.estado
                                       }).ToList();
                        }
                        ViewBag.query = listaBD;//Pasamos la lista como un viewbag para la vista y poder crear los filtros para la busqueda.
                        ViewBag.listaclientes = ddnClientes;
                        return View("DatosGlobales");

                    }
                    return RedirectToAction("Reservaciones");
                }
                return RedirectToAction("Index", "Login");
                      
            }
            catch
            {
                return View("Error");
            }
        }
        [HttpPost]
        public ActionResult Buscar(ReservacionModel filtro)//Acción solo habilitada para los empleados
        {
            try
            {
                string fechadefault = "1/1/0001";
                List<ReservacionModel> ddnClientes = null;
                List<ReservacionModel> listaBD = null;
                if (filtro.fechaEntrada != Convert.ToDateTime(fechadefault) && filtro.fechaSalida != Convert.ToDateTime(fechadefault))
                {
                    if(filtro.fechaEntrada <= filtro.fechaSalida)
                    {
                        using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())//Creamos la conexión a la base de datos
                        {
                            ddnClientes = (from c in db.spConsultarPersonas()
                                           select new ReservacionModel
                                           {
                                               idPersona = c.idPersona,
                                               nombreCompleto = c.nombreCompleto
                                           }).ToList();

                            listaBD = (from r in db.spConsultarReservacionesFiltro(filtro.idPersona, filtro.fechaEntrada, filtro.fechaSalida)
                                       select new ReservacionModel
                                       {
                                           idReservacion = r.idReservacion,
                                           nombreCompleto = r.nombreCompleto,
                                           nombre = r.nombre,
                                           fechaEntrada = r.fechaEntrada,
                                           fechaSalida = r.fechaSalida,
                                           costoTotal = r.costoTotal,
                                           estado = r.estado
                                       }).ToList();


                        }
                        ViewBag.listaclientes = ddnClientes;
                        ViewBag.query = listaBD;
                        return View("DatosGlobales", filtro);
                    }//Fin del if
                    using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())//Creamos la conexión a la base de datos
                    {
                        ddnClientes = (from c in db.spConsultarPersonas()
                                       select new ReservacionModel
                                       {
                                           idPersona = c.idPersona,
                                           nombreCompleto = c.nombreCompleto
                                       }).ToList();
                        listaBD = (from r in db.spConsultarReservaciones()
                                   select new ReservacionModel
                                   {
                                       idReservacion = r.idReservacion,
                                       nombreCompleto = r.nombreCompleto,
                                       nombre = r.nombre,
                                       fechaEntrada = r.fechaEntrada,
                                       fechaSalida = r.fechaSalida,
                                       costoTotal = r.costoTotal,
                                       estado = r.estado
                                   }).ToList();

                    }
                    ViewBag.query = listaBD;
                    ViewBag.listaclientes = ddnClientes;
                    ViewBag.mensaje = "La fecha de salida debe ser mayor o igual a la fecha de entrada.";
                    return View("DatosGlobales");
                }//Fin del segundo if
                using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())//Creamos la conexión a la base de datos
                {
                    ddnClientes = (from c in db.spConsultarPersonas()
                                   select new ReservacionModel
                                   {
                                       idPersona = c.idPersona,
                                       nombreCompleto = c.nombreCompleto
                                   }).ToList();
                    listaBD = (from r in db.spConsultarReservaciones()
                               select new ReservacionModel
                               {
                                   idReservacion = r.idReservacion,
                                   nombreCompleto = r.nombreCompleto,
                                   nombre = r.nombre,
                                   fechaEntrada = r.fechaEntrada,
                                   fechaSalida = r.fechaSalida,
                                   costoTotal = r.costoTotal,
                                   estado = r.estado
                               }).ToList();

                }
                ViewBag.query = listaBD;
                ViewBag.listaclientes = ddnClientes;
                return View("DatosGlobales");
            }
            catch
            {
                return View("Error");
            }
        }
        [HttpGet]
        [ValidarSesionFilter]
        public ActionResult Crear()//Creamos la reservación
        {
            try
            {
                var sesion = (ProyectoFinal_PVI.Models.LoginModel)Session["nuevaSesion"];
                List<ReservacionModel> ddnClientes = null;
                List<ReservacionModel> ddnHoteles = null;
                if(sesion != null)
                {
                    if (sesion.ClienteOEmpleado == "Empleado")//Si es empleado 
                    {
                        using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                        {
                            ddnHoteles = (from h in db.spConsultarHoteles()
                                          select new ReservacionModel
                                          {
                                              idHotel = h.idHotel,
                                              nombre = h.nombre,
                                          }).ToList();


                            ddnClientes = (from c in db.spConsultarPersonas()
                                           select new ReservacionModel
                                           {
                                               idPersona = c.idPersona,
                                               nombreCompleto = c.nombreCompleto
                                           }).ToList();


                        }
                        ViewBag.listahotel = ddnHoteles;
                        ViewBag.listaclientes = ddnClientes;
                        return View("Crear", new ReservacionModel());//Le mandos una instancia vacia para darle dinamismo a la vista
                    }
                    if (sesion.ClienteOEmpleado == "Cliente")//Si es cliente
                    {
                        using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                        {
                            ddnHoteles = (from h in db.spConsultarHoteles()
                                          select new ReservacionModel
                                          {
                                              idHotel = h.idHotel,
                                              nombre = h.nombre,
                                          }).ToList();

                            ddnClientes = (from c in db.spConsultarPersonas()
                                           select new ReservacionModel
                                           {
                                               idPersona = c.idPersona,
                                               nombreCompleto = c.nombreCompleto
                                           }).ToList();
                        }
                        ViewBag.listahotel = ddnHoteles;
                        ViewBag.listaclientes = ddnClientes;
                        return View("Crear", new ReservacionModel()
                        { idPersona = sesion.idPersona });//Le mandos una instancia vacia para darle dinamismo a la vista
                    }
                    return RedirectToAction("Reservaciones");
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
        public ActionResult Crear(ReservacionModel reservacion)
        {
            try
            {
                var sesion = (ProyectoFinal_PVI.Models.LoginModel)Session["nuevaSesion"];
                List<ReservacionModel> ddnClientes = null;
                List<ReservacionModel> ddnHoteles = null;
                
                if (ModelState.IsValid == false)
                {
                    using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                    {
                        ddnHoteles = (from h in db.spConsultarHoteles()
                                      select new ReservacionModel
                                      {
                                          idHotel = h.idHotel,
                                          nombre = h.nombre,
                                      }).ToList();


                        ddnClientes = (from c in db.spConsultarPersonas()
                                       select new ReservacionModel
                                       {
                                           idPersona = c.idPersona,
                                           nombreCompleto = c.nombreCompleto
                                       }).ToList();
                    }
                    ViewBag.listahotel = ddnHoteles;
                    ViewBag.listaclientes = ddnClientes;
                    return View("Crear", reservacion);
                    
                }//Validamos que el modelo cumpla las anotaciones de datos correspondientes

                if (reservacion.fechaEntrada >= DateTime.Now && reservacion.fechaSalida >= DateTime.Now && reservacion.fechaEntrada <= reservacion.fechaSalida)
                {
                    var numeroHabitacion = "";//Variable local que me recibe lo que me trae el procedimiento almacenado
                    using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                    {
                        numeroHabitacion = (from n in db.spConsultarHabitacionSinReservacion(reservacion.idHotel, reservacion.numeroAdultos + reservacion.numeroNihnos)
                                            select n).FirstOrDefault();
                        if (numeroHabitacion == "" || numeroHabitacion == null)
                        {
                            ddnHoteles = (from h in db.spConsultarHoteles()
                                          select new ReservacionModel
                                          {
                                              idHotel = h.idHotel,
                                              nombre = h.nombre,
                                          }).ToList();


                            ddnClientes = (from c in db.spConsultarPersonas()
                                           select new ReservacionModel
                                           {
                                               idPersona = c.idPersona,
                                               nombreCompleto = c.nombreCompleto
                                           }).ToList();
                            ViewBag.listahotel = ddnHoteles;
                            ViewBag.listaclientes = ddnClientes;
                            TempData["msg"] = "<script>alert('El hotel actual no tiene habitaciones con suficiente espacio para realizar la reservación');</script>";
                            return View("Crear", reservacion);
                        }
                        //Código para realizar la reservación                     
                        ReservacionModel idHabitacion = (from h in db.spConsultarIdDeHabitacion(numeroHabitacion)
                                            select new ReservacionModel
                                            {
                                               idHabitacion = h.idHabitacion 
                                            }).FirstOrDefault();

                        ReservacionModel precios = (from p in db.spConsultarPreciosPorIdHotel(reservacion.idHotel)
                                       select new ReservacionModel
                                       { 
                                           costoPorCadaAdulto = p.costoPorCadaAdulto,
                                           costoPorCadaNihno = p.costoPorCadaNinho

                                       }).FirstOrDefault();

                        reservacion.idHabitacion = idHabitacion.idHabitacion;
                        reservacion.costoPorCadaAdulto = precios.costoPorCadaAdulto;
                        reservacion.costoPorCadaNihno = precios.costoPorCadaNihno;                
                        reservacion.fechaCreacion = DateTime.Now;
                        reservacion.estado = "A";


                        db.spCrearReservacion(reservacion.idPersona, reservacion.idHabitacion, 
                                                reservacion.fechaEntrada, reservacion.fechaSalida, 
                                                reservacion.numeroAdultos, reservacion.numeroNihnos, 
                                                reservacion.totalDiasReservacion, reservacion.costoPorCadaAdulto, reservacion.costoPorCadaNihno,
                                                reservacion.calcularCostoTotal, reservacion.fechaCreacion, reservacion.estado);

                        //Insertar datos en bitacora                      
                        var idReservacion = (from r in db.spConsultarUltimaReservacionRealizada()
                                             select new ReservacionModel 
                                             {
                                                 idReservacion = r.idReservacion
                                             }).FirstOrDefault();
                        db.spCrearRegistroBitacora(idReservacion.idReservacion, sesion.idPersona, reservacion.accionRealizada = "CREADA", reservacion.fechaDeLaAccion = DateTime.Now);

                    }
                    ViewBag.mensaje = "Reservación creada correctamente";
                    return View("Resultado", new ReservacionModel 
                    {
                       numeroHabitacion = numeroHabitacion,
                       fechaEntrada = reservacion.fechaEntrada,
                       fechaSalida = reservacion.fechaSalida,
                       costoTotal = reservacion.costoTotal,
                       accionRealizada = reservacion.accionRealizada,
                  
                    });
                }
                //Fin validación de fechas
                else
                {

                    using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                    {
                        ddnHoteles = (from h in db.spConsultarHoteles()
                                      select new ReservacionModel
                                      {
                                          idHotel = h.idHotel,
                                          nombre = h.nombre
                                      }).ToList();


                        ddnClientes = (from c in db.spConsultarPersonas()
                                       select new ReservacionModel
                                       {
                                           idPersona = c.idPersona,
                                           nombreCompleto = c.nombreCompleto
                                       }).ToList();
                    }
                    ViewBag.listahotel = ddnHoteles;
                    ViewBag.listaclientes = ddnClientes;
                    TempData["msg"] = "<script>alert('La fecha de entrada o de salida no son correctas');</script>";
                    return View("Crear", reservacion);
                }
                
                
            }
            catch
            {
                return View("Error");
            }
        }

        [HttpGet]
        [ValidarSesionFilter]
        public ActionResult Editar(int id)
        {
            try
            {
                var sesion = (ProyectoFinal_PVI.Models.LoginModel)Session["nuevaSesion"];
                if(sesion != null)
                {
                    ReservacionModel reservacionRealizada = null;
                    using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                    {
                        reservacionRealizada = (from d in db.spConsultarDetalleReservacionPorId(id)
                                                select new ReservacionModel
                                                {
                                                    nombreCompleto = d.nombreCompleto,
                                                    estado = d.estado,
                                                    fechaEntrada = d.fechaEntrada,
                                                    fechaSalida = d.fechaSalida


                                                }).FirstOrDefault();
                    }
                    if (sesion.ClienteOEmpleado == "Empleado")//Realizamos la validacion si es cliente o empleado
                    {
                        if (reservacionRealizada.estadoActual == "Finalizada" || reservacionRealizada.estadoActual == "Cancelada")
                        {
                            return RedirectToAction("GestionarReservaciones");
                        }
                        ReservacionModel reservacionBD = null;
                        using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                        {
                            reservacionBD = (from r in db.spConsultarDetalleReservacionPorId(id)
                                             select new ReservacionModel
                                             {
                                                 idHotel = r.idHotel,
                                                 idHabitacion = r.idHabitacion,
                                                 idReservacion = r.idReservacion,
                                                 nombre = r.nombre,
                                                 numeroHabitacion = r.numeroHabitacion,
                                                 nombreCompleto = r.nombreCompleto,
                                                 fechaEntrada = r.fechaEntrada,
                                                 fechaSalida = r.fechaSalida,
                                                 costoPorCadaAdulto = r.costoPorCadaAdulto,
                                                 costoPorCadaNihno = r.costoPorCadaNinho,
                                                 numeroAdultos = r.numeroAdultos,
                                                 numeroNihnos = r.numeroNinhos

                                             }).FirstOrDefault();
                        }
                        return View("Editar", reservacionBD);//Retornamos la vista Editar para realizar los cambios necesarios
                    }//Fin if empleado
                    else
                    {
                        if (reservacionRealizada.estadoActual == "Finalizada" || reservacionRealizada.estadoActual == "En proceso" || reservacionRealizada.estadoActual == "Cancelada")
                        {
                            return RedirectToAction("Reservaciones");
                        }
                        if (sesion.nombreCompleto == reservacionRealizada.nombreCompleto)//Realizamos la verificación de el usuario para que pueda acceder solo a los datos
                                                                                         //que le pertenecen según la BD          
                        {
                            ReservacionModel detalleBD = null;
                            using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                            {
                                detalleBD = (from d in db.spConsultarDetalleReservacionPorId(id)
                                             select new ReservacionModel
                                             {
                                                 idReservacion = d.idReservacion,
                                                 idHotel = d.idHotel,
                                                 nombre = d.nombre,
                                                 idHabitacion = d.idHabitacion,
                                                 numeroHabitacion = d.numeroHabitacion,
                                                 nombreCompleto = d.nombreCompleto,
                                                 fechaEntrada = d.fechaEntrada,
                                                 fechaSalida = d.fechaSalida,
                                                 costoPorCadaAdulto = d.costoPorCadaAdulto,
                                                 costoPorCadaNihno = d.costoPorCadaNinho,
                                                 numeroAdultos = d.numeroAdultos,
                                                 numeroNihnos = d.numeroNinhos,
                                                 estado = d.estado


                                             }).FirstOrDefault();
                            }
                            return View("Editar", detalleBD);
                        }//Fin if cliente
                        return RedirectToAction("Reservaciones", "Reservaciones");
                    }
                             
                }
                return RedirectToAction("Index", "Login");
            }//Hacemos un redirecionamiento hacia la pantalla de mis reservaciones si el cliente intenta acceder a otra información
            catch
            {
                return View("Error");
            }
        }
        [HttpPost]
        [ValidarSesionFilter]
        public ActionResult Editar(ReservacionModel modelo)
        {
            try
            {
                if(ModelState.IsValid == false)
                {
                    return View("Editar", modelo);
                }//Validamos que el modelo este correcto
                ReservacionModel reservacionRealizada = null;
                var sesion = (ProyectoFinal_PVI.Models.LoginModel)Session["nuevaSesion"];
                //Reseolver este if
                if (modelo.fechaEntrada < DateTime.Now || modelo.fechaSalida < DateTime.Now || modelo.fechaEntrada > modelo.fechaSalida)
                {
                    TempData["msg"] = "<script>alert('La fecha de entrada o de salida no son correctas');</script>";
                    return View("Editar", modelo);
                }
                else
                {
                    using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                    {
                        reservacionRealizada = (from r in db.spConsultarHabitacionPorId(modelo.idHabitacion)
                                                select new ReservacionModel
                                                {
                                                    capacidadMaxima = r.capacidadMaxima
                                                }).FirstOrDefault();

                        if (reservacionRealizada.capacidadMaxima < modelo.numeroAdultos + modelo.numeroNihnos)
                        {
                            TempData["msg"] = "<script>alert('La cantidad de personas superan el máximo de esta habitación');</script>";
                            return View("Editar", modelo);
                        }
                        else
                        {
                            ReservacionModel precios = (from p in db.spConsultarPreciosPorIdHotel(modelo.idHotel)//Recalculamos los precios
                                                        select new ReservacionModel
                                                        {
                                                            costoPorCadaAdulto = p.costoPorCadaAdulto,
                                                            costoPorCadaNihno = p.costoPorCadaNinho

                                                        }).FirstOrDefault();                         
                            modelo.costoPorCadaAdulto = precios.costoPorCadaAdulto;
                            modelo.costoPorCadaNihno = precios.costoPorCadaNihno;

                            //Realizamos la edición de los datos correctamente
                            db.spEditarReservacion(modelo.idReservacion, modelo.fechaEntrada, modelo.fechaSalida, modelo.numeroAdultos, modelo.numeroNihnos,
                                                 modelo.totalDiasReservacion, modelo.costoPorCadaAdulto, modelo.costoPorCadaNihno, modelo.calcularCostoTotal, modelo.fechaModificacion = DateTime.Now);
                            //Se crea el registro de la bitacora
                            db.spCrearRegistroBitacora(modelo.idReservacion, sesion.idPersona, 
                                                        modelo.accionRealizada = "CORREGIDA", modelo.fechaDeLaAccion = DateTime.Now);
                        }
                    }
                    ViewBag.Mensaje = "Se han modificado los datos de la siguiente reservación:";
                    return View("Resultado", new ReservacionModel 
                    {
                        numeroHabitacion = modelo.numeroHabitacion,
                        fechaEntrada = modelo.fechaEntrada,
                        fechaSalida = modelo.fechaSalida,
                        accionRealizada = modelo.accionRealizada
                  
                    });
                    
                }//Fin pimer if
               
                


            }
            catch
            {
                return View("Error");
            }
        }
        [HttpGet]
        public ActionResult Cancelar(int id)
        {
            try
            {
                var sesion = (ProyectoFinal_PVI.Models.LoginModel)Session["nuevaSesion"];
                ReservacionModel reservacionRealizada = null;

                using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                {
                    reservacionRealizada = (from r in db.spConsultarDetalleReservacionPorId(id)
                                            select new ReservacionModel
                                            {
                                                idReservacion = r.idReservacion,
                                                fechaEntrada = r.fechaEntrada,
                                                fechaSalida = r.fechaSalida,
                                                estado = r.estado
                                            }).FirstOrDefault();
                    
                    if(sesion.ClienteOEmpleado == "Empleado") 
                    {
                        if (reservacionRealizada.estado == "I" || reservacionRealizada.estadoActual == "Finalizada" || reservacionRealizada.estadoActual == "En proceso")
                        {
                            return RedirectToAction("GestionarReservaciones");
                        }
                        db.spCancelarReservacion(id, reservacionRealizada.fechaModificacion = DateTime.Now);//Cancelamos la reservacion
                        db.spCrearRegistroBitacora(id, sesion.idPersona, reservacionRealizada.accionRealizada = "CANCELADA", reservacionRealizada.fechaDeLaAccion = DateTime.Now);

                        return RedirectToAction("GestionarReservaciones");
                    }
                    else
                    {
                        if(reservacionRealizada.estado == "I" || reservacionRealizada.estadoActual == "Finalizada" || reservacionRealizada.estadoActual == "En proceso")
                        {
                            return RedirectToAction("Reservaciones");
                        }
                        db.spCancelarReservacion(id, reservacionRealizada.fechaModificacion = DateTime.Now);//Cancelamos la reservacion
                        db.spCrearRegistroBitacora(id, sesion.idPersona, reservacionRealizada.accionRealizada = "CANCELADA", reservacionRealizada.fechaDeLaAccion = DateTime.Now);

                        return RedirectToAction("Reservaciones");

                    }
                }              
            }
            catch
            {
                return View("Error");
            }
        }
       
   
    }
}