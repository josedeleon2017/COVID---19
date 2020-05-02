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
                               /// PatientModel.Heap_Add(currentPatient);
                            }
                        }
                    }                  
                }

                PersonModel person1 = new PersonModel { Nombre = "Jose", Apellido="De Leon", CUI="2996768110101", Departamento="Guatemala", Edad="19 años", FechaDeNacimiento=Convert.ToDateTime("09/09/2000"), Municipio="Guatemala"};
                PersonModel person2 = new PersonModel { Nombre = "Vinicio" };
                PersonModel person3 = new PersonModel { Nombre = "De Leon" };
                PersonModel person4 = new PersonModel { Nombre = "Jimenez" };
                PersonModel person5 = new PersonModel { Nombre = "Javier" };

                PersonModel.HashTable_Add("GU", person1);
                PersonModel.HashTable_Add("GU", person2);
                PersonModel.HashTable_Add("GU", person3);
                PersonModel.HashTable_Add("GU", person4);
                PersonModel.HashTable_Add("GU", person5);

                int firstIndicator = PersonModel.HashTable_CountEmptys("GU");
                string positions1 = PersonModel.HashTable_Positions("GU");

                PersonModel personResult = PersonModel.HashTable_Find("GU-3");


                int SecondIndicator = PersonModel.HashTable_CountEmptys("GU");
                string positions2 = PersonModel.HashTable_Positions("GU");

                List<PersonModel> listTest = Storage.Instance.HospitalHashTable["GU"].ToList();

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