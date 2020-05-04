using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COVID_19.Helpers;
using COVID_19.Models;

namespace COVID_19.Controllers
{
    public class HospitalController : Controller
    {
        // GET: Hospital
        public ActionResult Index()
        {
            return View();
        }

        // GET: Hospital/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Hospital/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Hospital/Create
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

        // GET: Hospital/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Hospital/Edit/5
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

        // GET: Hospital/Delete/5
        public ActionResult Delete(string id)
        {
            return View();
        }

        // POST: Hospital/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
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

        public ActionResult Simulation()
        {
            int Sospechosos = Storage.Instance.PatientTree.ToInOrden().Where(x => x.Estatus == "SOSPECHOSO").ToList().Count();
            int Confirmados = Storage.Instance.PatientTree.ToInOrden().Where(x => x.Estatus == "CONFIRMADO").ToList().Count();
            int Recuperados = Storage.Instance.PatientTree.ToInOrden().Where(x => x.Estatus == "RECUPERADO").ToList().Count();
            double Porcentaje = Convert.ToDouble(Confirmados) / (Convert.ToDouble(Confirmados) + Convert.ToDouble(Recuperados)) * 100;

            Storage.Instance.PatientHashTable.Add("GU", null);
            Storage.Instance.PatientHashTable.Add("ES", null);
            Storage.Instance.PatientHashTable.Add("QZ", null);
            Storage.Instance.PatientHashTable.Add("CQ", null);
            Storage.Instance.PatientHashTable.Add("PE", null);

            ViewBag.Sospechosos = Sospechosos;
            ViewBag.Confirmados = Confirmados;
            ViewBag.Porcentaje = Convert.ToString(Math.Round(Porcentaje, 2)) + "%";
            ViewBag.Recuperados = Recuperados;

            ViewBag.HospitalGuatemala = Storage.Instance.PatientHashTable.CountEmptys("GU");
            ViewBag.HospitalEscuintla = Storage.Instance.PatientHashTable.CountEmptys("ES");
            ViewBag.HospitalQuetzaltenango = Storage.Instance.PatientHashTable.CountEmptys("QZ");
            ViewBag.HospitalChiquimula = Storage.Instance.PatientHashTable.CountEmptys("CQ");
            ViewBag.HospitalPeten = Storage.Instance.PatientHashTable.CountEmptys("PE");
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(FormCollection collection)
        {
            try
            {
                string HashCode = collection["Hash"];
                if (Storage.Instance.PatientHashTable.Contains(HashCode.Substring(0, 2)))
                {
                    ViewBag.Result = "[ " + HashCode + " ]";
                    PatientModel patientResult = Storage.Instance.PatientHashTable.Find(HashCode);
                    if (patientResult == null)
                    {
                        ViewBag.Result = "[ CAMA VACÍA ]";
                        return View();
                    }
                    return View(patientResult);
                }
                else
                {
                    ViewBag.Result = "[ NO SE ECONTRO EL HASH ]";
                    return View();
                }           
            }
            catch
            {
                return View("Error");
            }
        }

        public ActionResult Admin_Guatemala()
        {
            ViewBag.CamasDisponibles = Storage.Instance.PatientHashTable.Positions("GU");
            if (Storage.Instance.PatientHashTable.isFull("GU"))
            {
                ViewBag.CamasDisponibles = "[ -- ]";
            }
            return View(Storage.Instance.PatientHashTable.ToList("GU"));
        }

        [HttpPost]
        public ActionResult Admin_Guatemala(FormCollection collection)
        {
            try
            {
                //AGREGAR INSERCION DEL HEAP
                if (Storage.Instance.Heap_GU_C.Count == 0)
                {
                    ViewBag.Result = "Cola de confirmados vacia";
                    ViewBag.CamasDisponibles = Storage.Instance.PatientHashTable.Positions("GU");
                    return View(Storage.Instance.PatientHashTable.ToList("GU"));
                }
                if (!Storage.Instance.PatientHashTable.isFull("GU"))
                {
                    PatientModel patient = Storage.Instance.Heap_GU_C.RemoveRoot();
                    PatientModel.HashTable_Add("GU", patient);
                    ViewBag.Result = "Paciente ingresado exitosamente";
                    ViewBag.CamasDisponibles = Storage.Instance.PatientHashTable.Positions("GU");
                    if (Storage.Instance.PatientHashTable.isFull("GU"))
                    {
                        ViewBag.CamasDisponibles = "[ -- ]";
                    }
                    return View(Storage.Instance.PatientHashTable.ToList("GU"));
                }
                else
                {
                    ViewBag.Result = "Camas llenas, el paciente permance en la cola";
                    ViewBag.CamasDisponibles = "[ -- ]";
                    return View(Storage.Instance.PatientHashTable.ToList("GU"));
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Remove_Guatemala(string id)
        {
            PatientModel.HashTable_Delete("GU", id);
            return RedirectToAction("Admin_Guatemala");
        }


        public ActionResult Admin_Escuintla()
        {
            ViewBag.CamasDisponibles = Storage.Instance.PatientHashTable.Positions("ES");
            if (Storage.Instance.PatientHashTable.isFull("ES"))
            {
                ViewBag.CamasDisponibles = "[ -- ]";
            }
            return View(Storage.Instance.PatientHashTable.ToList("ES"));
        }

        [HttpPost]
        public ActionResult Admin_Escuintla(FormCollection collection)
        {
            try
            {
                //AGREGAR INSERCION DEL HEAP
                if (Storage.Instance.Heap_ES_C.Count == 0)
                {
                    ViewBag.Result = "Cola de confirmados vacia";
                    ViewBag.CamasDisponibles = Storage.Instance.PatientHashTable.Positions("ES");
                    return View(Storage.Instance.PatientHashTable.ToList("ES"));
                }
                if (!Storage.Instance.PatientHashTable.isFull("ES"))
                {
                    PatientModel patient = Storage.Instance.Heap_ES_C.RemoveRoot();
                    PatientModel.HashTable_Add("ES", patient);
                    ViewBag.Result = "Paciente ingresado exitosamente";
                    ViewBag.CamasDisponibles = Storage.Instance.PatientHashTable.Positions("ES");
                    if (Storage.Instance.PatientHashTable.isFull("ES"))
                    {
                        ViewBag.CamasDisponibles = "[ -- ]";
                    }
                    return View(Storage.Instance.PatientHashTable.ToList("ES"));
                }
                else
                {
                    ViewBag.Result = "Camas llenas, el paciente permance en la cola";
                    ViewBag.CamasDisponibles = "[ -- ]";
                    return View(Storage.Instance.PatientHashTable.ToList("ES"));
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Remove_Escuintla(string id)
        {
            PatientModel.HashTable_Delete("ES", id);
            return RedirectToAction("Admin_Escuintla");
        }

        public ActionResult Admin_Quetzaltenango()
        {
            ViewBag.CamasDisponibles = Storage.Instance.PatientHashTable.Positions("QZ");
            if (Storage.Instance.PatientHashTable.isFull("QZ"))
            {
                ViewBag.CamasDisponibles = "[ -- ]";
            }
            return View(Storage.Instance.PatientHashTable.ToList("QZ"));
        }

        [HttpPost]
        public ActionResult Admin_Quetzaltenango(FormCollection collection)
        {
            try
            {
                //AGREGAR INSERCION DEL HEAP
                if (Storage.Instance.Heap_QZ_C.Count == 0)
                {
                    ViewBag.Result = "Cola de confirmados vacia";
                    ViewBag.CamasDisponibles = Storage.Instance.PatientHashTable.Positions("QZ");
                    return View(Storage.Instance.PatientHashTable.ToList("QZ"));
                }
                if (!Storage.Instance.PatientHashTable.isFull("QZ"))
                {
                    PatientModel patient = Storage.Instance.Heap_QZ_C.RemoveRoot();
                    PatientModel.HashTable_Add("QZ", patient);
                    ViewBag.Result = "Paciente ingresado exitosamente";
                    ViewBag.CamasDisponibles = Storage.Instance.PatientHashTable.Positions("QZ");
                    if (Storage.Instance.PatientHashTable.isFull("QZ"))
                    {
                        ViewBag.CamasDisponibles = "[ -- ]";
                    }
                    return View(Storage.Instance.PatientHashTable.ToList("QZ"));
                }
                else
                {
                    ViewBag.Result = "Camas llenas, el paciente permance en la cola";
                    ViewBag.CamasDisponibles = "[ -- ]";
                    return View(Storage.Instance.PatientHashTable.ToList("QZ"));
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Remove_Quetzaltenango(string id)
        {
            PatientModel.HashTable_Delete("QZ", id);
            return RedirectToAction("Admin_Quetzaltenango");
        }

        public ActionResult Admin_Chiquimula()
        {
            ViewBag.CamasDisponibles = Storage.Instance.PatientHashTable.Positions("CQ");
            if (Storage.Instance.PatientHashTable.isFull("CQ"))
            {
                ViewBag.CamasDisponibles = "[ -- ]";
            }
            return View(Storage.Instance.PatientHashTable.ToList("CQ"));
        }

        [HttpPost]
        public ActionResult Admin_Chiquimula(FormCollection collection)
        {
            try
            {
                //AGREGAR INSERCION DEL HEAP
                if (Storage.Instance.Heap_CQ_C.Count == 0)
                {
                    ViewBag.Result = "Cola de confirmados vacia";
                    ViewBag.CamasDisponibles = Storage.Instance.PatientHashTable.Positions("CQ");
                    return View(Storage.Instance.PatientHashTable.ToList("CQ"));
                }
                if (!Storage.Instance.PatientHashTable.isFull("CQ"))
                {
                    PatientModel patient = Storage.Instance.Heap_CQ_C.RemoveRoot();
                    PatientModel.HashTable_Add("CQ", patient);
                    ViewBag.Result = "Paciente ingresado exitosamente";
                    ViewBag.CamasDisponibles = Storage.Instance.PatientHashTable.Positions("CQ");
                    if (Storage.Instance.PatientHashTable.isFull("CQ"))
                    {
                        ViewBag.CamasDisponibles = "[ -- ]";
                    }
                    return View(Storage.Instance.PatientHashTable.ToList("CQ"));
                }
                else
                {
                    ViewBag.Result = "Camas llenas, el paciente permance en la cola";
                    ViewBag.CamasDisponibles = "[ -- ]";
                    return View(Storage.Instance.PatientHashTable.ToList("CQ"));
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Remove_Chiquimula(string id)
        {
            PatientModel.HashTable_Delete("CQ", id);
            return RedirectToAction("Admin_Chiquimula");
        }

        public ActionResult Admin_Peten()
        {
            ViewBag.CamasDisponibles = Storage.Instance.PatientHashTable.Positions("PE");
            if (Storage.Instance.PatientHashTable.isFull("PE"))
            {
                ViewBag.CamasDisponibles = "[ -- ]";
            }
            return View(Storage.Instance.PatientHashTable.ToList("PE"));
        }

        [HttpPost]
        public ActionResult Admin_Peten(FormCollection collection)
        {
            try
            {
                //AGREGAR INSERCION DEL HEAP
                if (Storage.Instance.Heap_PE_C.Count == 0)
                {
                    ViewBag.Result = "Cola de confirmados vacia";
                    ViewBag.CamasDisponibles = Storage.Instance.PatientHashTable.Positions("PE");
                    return View(Storage.Instance.PatientHashTable.ToList("PE"));
                }
                if (!Storage.Instance.PatientHashTable.isFull("PE"))
                {
                    PatientModel patient = Storage.Instance.Heap_PE_C.RemoveRoot();
                    PatientModel.HashTable_Add("PE", patient);
                    ViewBag.Result = "Paciente ingresado exitosamente";
                    ViewBag.CamasDisponibles = Storage.Instance.PatientHashTable.Positions("PE");
                    if (Storage.Instance.PatientHashTable.isFull("PE"))
                    {
                        ViewBag.CamasDisponibles = "[ -- ]";
                    }
                    return View(Storage.Instance.PatientHashTable.ToList("PE"));
                }
                else
                {
                    ViewBag.Result = "Camas llenas, el paciente permance en la cola";
                    ViewBag.CamasDisponibles = "[ -- ]";
                    return View(Storage.Instance.PatientHashTable.ToList("PE"));
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Remove_Peten(string id)
        {
            PatientModel.HashTable_Delete("PE", id);
            return RedirectToAction("Admin_Peten");
        }
    }
}
