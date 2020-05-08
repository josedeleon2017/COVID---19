using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COVID_19.Models;
using System.IO;
using COVID_19.Helpers;


namespace COVID_19.Controllers
{
    public class PatientController : Controller
    {
        // GET: Patient
        public ActionResult Index()
        {
            return View();
        }

        // GET: Patient/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Patient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Patient/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Patient/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Patient/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Patient/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult NewEntry()
        {
            return View();
        }


        [HttpPost]
        public ActionResult NewEntry(FormCollection collection, HttpPostedFileBase postedfile)
        {
            try
            {                
                if(collection["Nombre"] == "" || 
                   collection["Apellido"] == "" ||
                   collection["FechaDeNacimiento"] == "" ||
                   collection["CUI"] == "" ||
                   collection["Departamento"] == "" ||
                   collection["Municipio"] == "") 
                {
                    ViewBag.Result = "Debe llenar todos los campos requeridos";
                    return View();
                }
                if (!collection.AllKeys.Contains("Departamento"))
                {
                    ViewBag.Result = "Debe seleccionar el departamento de residencia";
                    return View();
                }
                string currentDescription = "";
                if(collection["Descripcion"] == "" && postedfile == null)
                {
                    ViewBag.Result = "Debe agregar almenos una descripción del posible contagio";
                    return View();
                }
                if (collection["Descripcion"] != "" && postedfile != null)
                {
                    ViewBag.Result = "Debe agregar una descripción del posible contagio como máximo";
                    return View();
                }
                string FilePath = "";
                string FilePath_db = "";
                if (postedfile != null)
                {
                    string Path = Server.MapPath("~/Data/");
                    string Path_db = Server.MapPath("~/App_Data/");
                    FilePath_db = Path_db + "words.csv";
                    if (!Directory.Exists(Path))
                    {
                        Directory.CreateDirectory(Path);
                    }
                    FilePath = Path + System.IO.Path.GetFileName(postedfile.FileName);
                    postedfile.SaveAs(FilePath);
                    currentDescription = PatientModel.ObtenerCoincidencias(FilePath, FilePath_db);
                }
                else
                {
                    currentDescription = collection["Descripcion"];
                }
                PersonModel CuiPerson = new PersonModel { CUI = collection["CUI"], };
                if (Storage.Instance.PersonTree.Find(CuiPerson) != null)
                {
                    ViewBag.Result = "El CUI ya se encuentra registrado en el sistema";
                    return View();
                }
                if (Convert.ToDateTime(collection["FechaDeNacimiento"]) > DateTime.Now)
                {
                    ViewBag.Result = "La fecha de nacimiento no puede ser mayor a la fecha actual";
                    return View();
                }
                string currentAge = PersonModel.CalcularEdad(Convert.ToDateTime(collection["FechaDeNacimiento"]));
                if (currentAge == null)
                {
                    return View("Error");
                }
                var currentPerson = new PersonModel
                {
                    Nombre = collection["Nombre"],
                    Apellido = collection["Apellido"],
                    FechaDeNacimiento = Convert.ToDateTime(collection["FechaDeNacimiento"]),
                    CUI = collection["CUI"],
                    Departamento = collection["Departamento"],
                    Municipio = collection["Municipio"],
                    Edad = currentAge,
                };
                int currentPriority = PatientModel.AsignarPrioridad(currentAge, "Sospechoso");
                if (currentPriority == -1)
                {
                    return View("Error");
                };
                string currentCategory = PatientModel.DescripcionPrioridad(currentPriority);
                string currentHospital = PatientModel.AsignarHospital(collection["Departamento"]);
                if (currentHospital == "")
                {
                    return View("Error");
                }
                string currentSymtoms = PatientModel.DescripcionSintomas(collection["Sintoma1"], collection["Sintoma2"], collection["Sintoma3"], collection["Sintoma4"], collection["Sintoma5"]);
                if (currentSymtoms == "")
                {
                    ViewBag.Result = "Debe seleccionar almenos un sintoma";
                    return View();
                }
                var currentPatient = new PatientModel
                {
                    Nombre = collection["Nombre"],
                    Apellido = collection["Apellido"],
                    FechaDeNacimiento = Convert.ToDateTime(collection["FechaDeNacimiento"]),
                    CUI = collection["CUI"],
                    Departamento = collection["Departamento"],
                    Municipio = collection["Municipio"],
                    Edad = currentAge,
                    Sintomas = currentSymtoms,
                    Descripcion = currentDescription,
                    Estatus = "Sospechoso".ToUpper(),
                    Categoria = currentCategory,
                    Prioridad = currentPriority,
                    Hospital = currentHospital,
                    FechaDeIngreso = DateTime.Now.Date,
                };
                PersonModel.Tree_Add(currentPerson);
                PersonModel.CustomTree_Add(currentPerson);
                PatientModel.Tree_Add(currentPatient);
                PatientModel.Heap_Add(currentPatient);
                ViewBag.Result = "Persona registrada exitosamente";
                return View();
            }
            catch
            {
                return View();
            }
        }


        

    }
}
