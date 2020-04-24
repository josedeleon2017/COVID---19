using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text.RegularExpressions;
using COVID_19.Models;
using COVID_19.Helpers;

namespace COVID_19.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {

                LinkedList<PatientModel> patientList = new LinkedList<PatientModel>();

                if (Storage.Instance.first_load)
                {
                    string Path_users = Server.MapPath("~/App_Data/");

                    string FilePath_users = Path_users + "data.csv";
                    using (var fileStream = new FileStream(FilePath_users, FileMode.Open))
                    {
                        using (var streamReader = new StreamReader(fileStream))
                        {
                            streamReader.ReadLine();
                            while (!streamReader.EndOfStream)
                            {
                                var row = streamReader.ReadLine();

                                Regex regx = new Regex(";" + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                                string[] line = regx.Split(row);

                                var currentPatient = new PatientModel
                                {
                                    Nombre = line[0],
                                    Apellido = line[1],
                                    FechaDeNacimiento = Convert.ToDateTime(line[2]),
                                    CUI = line[3],
                                    Departamento = line[4],
                                    Municipio = line[5],
                                    Edad = PersonModel.CalcularEdad(Convert.ToDateTime(line[2])),

                                    Sintomas = line[6],
                                    Descripcion = line[7],
                                    Estatus = line[8].ToUpper(),
                                    Categoria = PatientModel.DescripcionPrioridad(PatientModel.AsignarPrioridad(PersonModel.CalcularEdad(Convert.ToDateTime(line[2])), line[8])),
                                    Prioridad = PatientModel.AsignarPrioridad(PersonModel.CalcularEdad(Convert.ToDateTime(line[2])), line[8]),
                                    Hospital = PatientModel.AsignarHospital(line[4]),
                                    FechaDeIngreso = Convert.ToDateTime(line[9])
                                };
                                patientList.AddLast(currentPatient);
                            }
                        }
                    }                  
                }

                ///<!--AGREGAR INGRESO A HEAPS-->
                ///Storage.Instance.Heap_GU_C.Add(patientList.Where(x => x.Hospital == "Guatemala").Where(x => x.Estatus == "CONFIRMADO"));
                ///Storage.Instance.Heap_GU_S.Add(patientList.Where(x => x.Hospital == "Guatemala").Where(x => x.Estatus == "SOSPECHOSO"));
                ///
                //////Storage.Instance.Heap_ES_C.Add(patientList.Where(x => x.Hospital == "Guatemala").Where(x => x.Estatus == "CONFIRMADO"));
                ///Storage.Instance.Heap_ES_S.Add(patientList.Where(x => x.Hospital == "Guatemala").Where(x => x.Estatus == "SOSPECHOSO"));
                ///
                //////Storage.Instance.Heap_QZ_C.Add(patientList.Where(x => x.Hospital == "Guatemala").Where(x => x.Estatus == "CONFIRMADO"));
                ///Storage.Instance.Heap_QZ_S.Add(patientList.Where(x => x.Hospital == "Guatemala").Where(x => x.Estatus == "SOSPECHOSO"));
                ///
                //////Storage.Instance.Heap_CQ_C.Add(patientList.Where(x => x.Hospital == "Guatemala").Where(x => x.Estatus == "CONFIRMADO"));
                ///Storage.Instance.Heap_CQ_S.Add(patientList.Where(x => x.Hospital == "Guatemala").Where(x => x.Estatus == "SOSPECHOSO"));
                ///
                //////Storage.Instance.Heap_PE_C.Add(patientList.Where(x => x.Hospital == "Guatemala").Where(x => x.Estatus == "CONFIRMADO"));
                ///Storage.Instance.Heap_PE_S.Add(patientList.Where(x => x.Hospital == "Guatemala").Where(x => x.Estatus == "SOSPECHOSO"));



                return View();
            }
            catch
            {
                return View("Error");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult NewEntry()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult TestCSV()
        {
            ViewBag.Message = "CSV";

            return View();
        }

        [HttpPost]
        public ActionResult TestCSV(HttpPostedFileBase postedfile)
        {
       
            string FilePath;
            if (postedfile != null)
            {
                string Path = Server.MapPath("~/App_Data/descripciones/");
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                FilePath = Path + System.IO.Path.GetFileName(postedfile.FileName);
                postedfile.SaveAs(FilePath);


                string Path_db = Server.MapPath("~/App_Data/");
                if (!Directory.Exists(Path_db))
                {
                    Directory.CreateDirectory(Path_db);
                }

                string FilePath_db = Path_db + "words.csv";


                string count = "";
                count = PatientModel.ObtenerCoincidencias(FilePath, FilePath_db);

               
            }
            return View();
        }
    }
}