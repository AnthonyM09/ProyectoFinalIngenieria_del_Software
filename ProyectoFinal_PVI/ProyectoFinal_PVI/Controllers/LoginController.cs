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
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Inicio()
        {
            return View("Inicio");
        }
        [HttpPost]
        public ActionResult IniciarSesion(LoginModel usuario)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return View("Inicio", usuario);
                }

                using (PVI_ProyectoFinalEntities1 db = new PVI_ProyectoFinalEntities1())
                {
                    LoginModel login = (from l in db.spLogin(usuario.email, usuario.clave)
                                        select new LoginModel
                                        {
                                            idPersona = l.idPersona,
                                            nombreCompleto = l.nombreCompleto,
                                            esEmpleado = l.esEmpleado

                                        }).FirstOrDefault();
                    if (login == null)
                    {

                        TempData["msg"] = "<script>alert('Email o contraseña incorrecto');</script>";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Session["nuevaSesion"] = login;                       
                        //Redireccionamos dependiento si es cliente o empleado
                        if(login.ClienteOEmpleado == "Cliente")
                        {
                            return RedirectToAction("Reservaciones", "Reservaciones");
                        }
                        else
                        {
                            return RedirectToAction("GestionarReservaciones", "Reservaciones");
                        }
                     

                    }
                }
            }
            catch
            {
                return View("Error");
            }
        }
        [HttpGet]
        public ActionResult Salir()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Login");
        }
    }
}