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

                                string currentAge = PersonModel.CalcularEdad(Convert.ToDateTime(line[2]));
                                if (currentAge == null)
                                {
                                    return View("Error");
                                }
                                var currentPerson = new PersonModel
                                {
                                    Nombre = line[0],
                                    Apellido = line[1],
                                    FechaDeNacimiento = Convert.ToDateTime(line[2]),
                                    CUI = line[3],
                                    Departamento = line[4],
                                    Municipio = line[5],
                                    Edad = currentAge,
                                };
                                int currentPriority = PatientModel.AsignarPrioridad(currentAge, line[8]);
                                if (currentPriority == -1)
                                {
                                    return View("Error");
                                };
                                string currentCategory = PatientModel.DescripcionPrioridad(currentPriority);
                                string currentHospital = PatientModel.AsignarHospital(line[4]);
                                if (currentHospital == "")
                                {
                                    return View("Error");
                                }
                                var currentPatient = new PatientModel
                                {
                                    Nombre = line[0],
                                    Apellido = line[1],
                                    FechaDeNacimiento = Convert.ToDateTime(line[2]),
                                    CUI = line[3],
                                    Departamento = line[4],
                                    Municipio = line[5],
                                    Edad = currentAge,
                                    Sintomas = line[6],
                                    Descripcion= line[7],
                                    Estatus= line[8].ToUpper(),
                                    Categoria= currentCategory,
                                    Prioridad= currentPriority,
                                    Hospital= currentHospital,
                                    FechaDeIngreso= Convert.ToDateTime(line[9]),
                                };
                                PersonModel.Tree_Add(currentPerson);
                                PersonModel.CustomTree_Add(currentPerson);
                                PatientModel.Tree_Add(currentPatient);
                                PatientModel.Heap_Add(currentPatient);
                            }
                        }
                    }                  
                }
                /*
                PatientModel person1 = new PatientModel { CUI="51" };
                PatientModel person2 = new PatientModel { CUI = "91" };
                PatientModel person3 = new PatientModel { CUI = "7" };
                PatientModel person4 = new PatientModel { CUI = "9" };
                PatientModel person5 = new PatientModel { CUI = "99" };

                PatientModel patient1 = PatientModel.Tree_Search(person1);
                PatientModel patient2 = PatientModel.Tree_Search(person2);
                PatientModel patient3 = PatientModel.Tree_Search(person3);
                PatientModel patient4 = PatientModel.Tree_Search(person4);
                PatientModel patient5 = PatientModel.Tree_Search(person5);

                Storage.Instance.PatientHashTable.Add("GU", patient1);
                Storage.Instance.PatientHashTable.Add("GU", patient2);
                Storage.Instance.PatientHashTable.Add("GU", patient3);
                Storage.Instance.PatientHashTable.Add("GU", patient4);
                Storage.Instance.PatientHashTable.Add("GU", patient5);

                int firstIndicator = Storage.Instance.PatientHashTable.CountEmptys("GU");
                */
                int indicator = Storage.Instance.PatientTree.ToInOrden().Where(x => x.Hospital == "Quetzaltenango").Where(x => x.Estatus == "CONFIRMADO").Count();

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