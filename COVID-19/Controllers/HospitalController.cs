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
            int Sospechosos = Storage.Instance.statsSospechosos;
            int Confirmados = Storage.Instance.statsConfirmados;
            int Recuperados = Storage.Instance.statsRecuperados;
            double Porcentaje = Convert.ToDouble(Confirmados) / (Convert.ToDouble(Confirmados) + Convert.ToDouble(Recuperados)) * 100;

            int Sospechosos_simulation = Storage.Instance.statsSimulationSospechosos;
            int Confirmados_simulation = Storage.Instance.statsSimulationConfirmados;
            int Recuperados_simulation = Storage.Instance.statsSimulationRecuperados;
            double Porcentaje_simulation = Convert.ToDouble(Confirmados_simulation) / (Convert.ToDouble(Confirmados_simulation) + Convert.ToDouble(Recuperados_simulation)) * 100;

            ViewBag.Sospechosos = Sospechosos;
            ViewBag.Confirmados = Confirmados;
            ViewBag.Porcentaje = Convert.ToString(Math.Round(Porcentaje, 2)) + "%";
            ViewBag.Recuperados = Recuperados;

            ViewBag.Sospechosos_simulation = Sospechosos_simulation;
            ViewBag.Confirmados_simulation = Confirmados_simulation;
            ViewBag.Porcentaje_simulation = Convert.ToString(Math.Round(Porcentaje_simulation, 2)) + "%";
            ViewBag.Recuperados_simulation = Recuperados_simulation;

            ViewBag.HospitalGuatemala = Storage.Instance.Hashfinal.CountEmptys(PatientModel.GetHash("GU"));
            ViewBag.HospitalEscuintla = Storage.Instance.Hashfinal.CountEmptys(PatientModel.GetHash("ES"));
            ViewBag.HospitalQuetzaltenango = Storage.Instance.Hashfinal.CountEmptys(PatientModel.GetHash("QZ"));
            ViewBag.HospitalChiquimula = Storage.Instance.Hashfinal.CountEmptys(PatientModel.GetHash("CQ"));
            ViewBag.HospitalPeten = Storage.Instance.Hashfinal.CountEmptys(PatientModel.GetHash("PE"));
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
                if (PatientModel.GetHash(HashCode.Substring(0, 2))!=-1)
                {
                    ViewBag.Result = "[ " + HashCode + " ]";
                    int position = 0;
                    if (HashCode.Contains("10"))
                    {
                       position = 1 + Convert.ToInt32(HashCode.Substring(3, 2));
                    }
                    else
                    {
                        position = 1 + Convert.ToInt32(HashCode.Substring(3, 1));
                    }
                    PatientModel patientResult = Storage.Instance.Hashfinal.Find(PatientModel.GetHash(HashCode.Substring(0, 2)), position);
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
            ViewBag.CamasDisponibles = Storage.Instance.Hashfinal.Positions(PatientModel.GetHash("GU"));
            ViewBag.Estatus= "PERSONAS EN ESPERA ["+Storage.Instance.Heap_GU_C.Count+"]";
            if (Storage.Instance.Hashfinal.IsFull(PatientModel.GetHash("GU")))
            {
                ViewBag.CamasDisponibles = "[ -- ]";
            }
            ViewBag.Result = "Personas hospitalizadas ["+Storage.Instance.Hashfinal.Count(PatientModel.GetHash("GU"))+"] ";
            return View(Storage.Instance.Hashfinal.ToList(PatientModel.GetHash("GU")));
        }

        [HttpPost]
        public ActionResult Admin_Guatemala(FormCollection collection)
        {
            try
            {
                //AGREGAR INSERCION DEL HEAP
                if (Storage.Instance.Heap_GU_C.Count == 0)
                {
                    ViewBag.Estatus = "PERSONAS EN ESPERA [" + Storage.Instance.Heap_GU_C.Count + "]";
                    ViewBag.Result = "Cola de confirmados vacia";
                    ViewBag.CamasDisponibles = Storage.Instance.Hashfinal.Positions(PatientModel.GetHash("GU"));
                    return View(Storage.Instance.Hashfinal.ToList(PatientModel.GetHash("GU")));
                }
                if (!Storage.Instance.Hashfinal.IsFull(PatientModel.GetHash("GU")))
                {
                    PatientModel patient = Storage.Instance.Heap_GU_C.RemoveRoot();
                    PatientModel.HashAdd(patient);
                    ViewBag.Result = "Paciente ingresado exitosamente";
                    ViewBag.CamasDisponibles = Storage.Instance.Hashfinal.Positions(PatientModel.GetHash("GU"));
                    if (Storage.Instance.Hashfinal.IsFull(PatientModel.GetHash("GU")))
                    {
                        ViewBag.CamasDisponibles = "[ -- ]";
                    }
                    ViewBag.Estatus = "PERSONAS EN ESPERA [" + Storage.Instance.Heap_GU_C.Count + "]";
                    return View(Storage.Instance.Hashfinal.ToList(PatientModel.GetHash("GU")));
                }
                else
                {
                    ViewBag.Result = "Camas llenas, el paciente permance en la cola";
                    ViewBag.CamasDisponibles = "[ -- ]";
                    ViewBag.Estatus = "PERSONAS EN ESPERA [" + Storage.Instance.Heap_GU_C.Count + "]";
                    return View(Storage.Instance.Hashfinal.ToList(PatientModel.GetHash("GU")));
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Remove_Guatemala(string id)
        {
            PatientModel.HashRemove("GU", id);
            Storage.Instance.statsSimulationRecuperados++;
            return RedirectToAction("Admin_Guatemala");
        }


        public ActionResult Admin_Escuintla()
        {
            ViewBag.CamasDisponibles = Storage.Instance.Hashfinal.Positions(PatientModel.GetHash("ES"));
            ViewBag.Estatus = "PERSONAS EN ESPERA [" + Storage.Instance.Heap_ES_C.Count + "]";
            if (Storage.Instance.Hashfinal.IsFull(PatientModel.GetHash("ES")))
            {
                ViewBag.CamasDisponibles = "[ -- ]";
            }
            ViewBag.Result = "Personas hospitalizadas [" + Storage.Instance.Hashfinal.Count(PatientModel.GetHash("ES")) + "] ";
            return View(Storage.Instance.Hashfinal.ToList(PatientModel.GetHash("ES")));
        }

        [HttpPost]
        public ActionResult Admin_Escuintla(FormCollection collection)
        {
            try
            {
                //AGREGAR INSERCION DEL HEAP
                if (Storage.Instance.Heap_ES_C.Count == 0)
                {
                    ViewBag.Estatus = "PERSONAS EN ESPERA [" + Storage.Instance.Heap_ES_C.Count + "]";
                    ViewBag.Result = "Cola de confirmados vacia";
                    ViewBag.CamasDisponibles = Storage.Instance.Hashfinal.Positions(PatientModel.GetHash("ES"));
                    return View(Storage.Instance.Hashfinal.ToList(PatientModel.GetHash("ES")));
                }
                if (!Storage.Instance.Hashfinal.IsFull(PatientModel.GetHash("ES")))
                {
                    PatientModel patient = Storage.Instance.Heap_ES_C.RemoveRoot();
                    PatientModel.HashAdd(patient);
                    ViewBag.Result = "Paciente ingresado exitosamente";
                    ViewBag.CamasDisponibles = Storage.Instance.Hashfinal.Positions(PatientModel.GetHash("ES"));
                    if (Storage.Instance.Hashfinal.IsFull(PatientModel.GetHash("ES")))
                    {
                        ViewBag.CamasDisponibles = "[ -- ]";
                    }
                    ViewBag.Estatus = "PERSONAS EN ESPERA [" + Storage.Instance.Heap_ES_C.Count + "]";
                    return View(Storage.Instance.Hashfinal.ToList(PatientModel.GetHash("ES")));
                }
                else
                {
                    ViewBag.Result = "Camas llenas, el paciente permance en la cola";
                    ViewBag.CamasDisponibles = "[ -- ]";
                    ViewBag.Estatus = "PERSONAS EN ESPERA [" + Storage.Instance.Heap_ES_C.Count + "]";
                    return View(Storage.Instance.Hashfinal.ToList(PatientModel.GetHash("ES")));
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Remove_Escuintla(string id)
        {
            PatientModel.HashRemove("ES", id);
            Storage.Instance.statsSimulationRecuperados++;
            return RedirectToAction("Admin_Escuintla");
        }

        public ActionResult Admin_Quetzaltenango()
        {
            ViewBag.CamasDisponibles = Storage.Instance.Hashfinal.Positions(PatientModel.GetHash("QZ"));
            ViewBag.Estatus = "PERSONAS EN ESPERA [" + Storage.Instance.Heap_QZ_C.Count + "]";
            if (Storage.Instance.Hashfinal.IsFull(PatientModel.GetHash("QZ")))
            {
                ViewBag.CamasDisponibles = "[ -- ]";
            }
            ViewBag.Result = "Personas hospitalizadas [" + Storage.Instance.Hashfinal.Count(PatientModel.GetHash("QZ")) + "] ";
            return View(Storage.Instance.Hashfinal.ToList(PatientModel.GetHash("QZ")));
        }

        [HttpPost]
        public ActionResult Admin_Quetzaltenango(FormCollection collection)
        {
            try
            {
                //AGREGAR INSERCION DEL HEAP
                if (Storage.Instance.Heap_QZ_C.Count == 0)
                {
                    ViewBag.Estatus = "PERSONAS EN ESPERA [" + Storage.Instance.Heap_QZ_C.Count + "]";
                    ViewBag.Result = "Cola de confirmados vacia";
                    ViewBag.CamasDisponibles = Storage.Instance.Hashfinal.Positions(PatientModel.GetHash("QZ"));
                    return View(Storage.Instance.Hashfinal.ToList(PatientModel.GetHash("QZ")));
                }
                if (!Storage.Instance.Hashfinal.IsFull(PatientModel.GetHash("QZ")))
                {
                    PatientModel patient = Storage.Instance.Heap_QZ_C.RemoveRoot();
                    PatientModel.HashAdd(patient);
                    ViewBag.Result = "Paciente ingresado exitosamente";
                    ViewBag.CamasDisponibles = Storage.Instance.Hashfinal.Positions(PatientModel.GetHash("QZ"));
                    if (Storage.Instance.Hashfinal.IsFull(PatientModel.GetHash("QZ")))
                    {
                        ViewBag.CamasDisponibles = "[ -- ]";
                    }
                    ViewBag.Estatus = "PERSONAS EN ESPERA [" + Storage.Instance.Heap_QZ_C.Count + "]";
                    return View(Storage.Instance.Hashfinal.ToList(PatientModel.GetHash("QZ")));
                }
                else
                {
                    ViewBag.Result = "Camas llenas, el paciente permance en la cola";
                    ViewBag.CamasDisponibles = "[ -- ]";
                    ViewBag.Estatus = "PERSONAS EN ESPERA [" + Storage.Instance.Heap_QZ_C.Count + "]";
                    return View(Storage.Instance.Hashfinal.ToList(PatientModel.GetHash("QZ")));
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Remove_Quetzaltenango(string id)
        {
            PatientModel.HashRemove("QZ", id);
            Storage.Instance.statsSimulationRecuperados++;
            return RedirectToAction("Admin_Quetzaltenango");
        }

        public ActionResult Admin_Chiquimula()
        {
            ViewBag.CamasDisponibles = Storage.Instance.Hashfinal.Positions(PatientModel.GetHash("CQ"));
            ViewBag.Estatus = "PERSONAS EN ESPERA [" + Storage.Instance.Heap_CQ_C.Count + "]";
            if (Storage.Instance.Hashfinal.IsFull(PatientModel.GetHash("CQ")))
            {
                ViewBag.CamasDisponibles = "[ -- ]";
            }
            ViewBag.Result = "Personas hospitalizadas [" + Storage.Instance.Hashfinal.Count(PatientModel.GetHash("CQ")) + "] ";
            return View(Storage.Instance.Hashfinal.ToList(PatientModel.GetHash("CQ")));
        }

        [HttpPost]
        public ActionResult Admin_Chiquimula(FormCollection collection)
        {
            try
            {
                //AGREGAR INSERCION DEL HEAP
                if (Storage.Instance.Heap_CQ_C.Count == 0)
                {
                    ViewBag.Estatus = "PERSONAS EN ESPERA [" + Storage.Instance.Heap_CQ_C.Count + "]";
                    ViewBag.Result = "Cola de confirmados vacia";
                    ViewBag.CamasDisponibles = Storage.Instance.Hashfinal.Positions(PatientModel.GetHash("CQ"));
                    return View(Storage.Instance.Hashfinal.ToList(PatientModel.GetHash("CQ")));
                }
                if (!Storage.Instance.Hashfinal.IsFull(PatientModel.GetHash("CQ")))
                {
                    PatientModel patient = Storage.Instance.Heap_CQ_C.RemoveRoot();
                    PatientModel.HashAdd(patient);
                    ViewBag.Result = "Paciente ingresado exitosamente";
                    ViewBag.CamasDisponibles = Storage.Instance.Hashfinal.Positions(PatientModel.GetHash("CQ"));
                    if (Storage.Instance.Hashfinal.IsFull(PatientModel.GetHash("CQ")))
                    {
                        ViewBag.CamasDisponibles = "[ -- ]";
                    }
                    ViewBag.Estatus = "PERSONAS EN ESPERA [" + Storage.Instance.Heap_CQ_C.Count + "]";
                    return View(Storage.Instance.Hashfinal.ToList(PatientModel.GetHash("CQ")));
                }
                else
                {
                    ViewBag.Result = "Camas llenas, el paciente permance en la cola";
                    ViewBag.CamasDisponibles = "[ -- ]";
                    ViewBag.Estatus = "PERSONAS EN ESPERA [" + Storage.Instance.Heap_CQ_C.Count + "]";
                    return View(Storage.Instance.Hashfinal.ToList(PatientModel.GetHash("CQ")));
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Remove_Chiquimula(string id)
        {
            PatientModel.HashRemove("CQ", id);
            Storage.Instance.statsSimulationRecuperados++;
            return RedirectToAction("Admin_Chiquimula");
        }

        public ActionResult Admin_Peten()
        {
            ViewBag.CamasDisponibles = Storage.Instance.Hashfinal.Positions(PatientModel.GetHash("PE"));
            ViewBag.Estatus = "PERSONAS EN ESPERA [" + Storage.Instance.Heap_PE_C.Count + "]";
            if (Storage.Instance.Hashfinal.IsFull(PatientModel.GetHash("PE")))
            {
                ViewBag.CamasDisponibles = "[ -- ]";
            }
            ViewBag.Result = "Personas hospitalizadas [" + Storage.Instance.Hashfinal.Count(PatientModel.GetHash("PE")) + "] ";
            return View(Storage.Instance.Hashfinal.ToList(PatientModel.GetHash("PE")));
        }

        [HttpPost]
        public ActionResult Admin_Peten(FormCollection collection)
        {
            try
            {
                //AGREGAR INSERCION DEL HEAP
                if (Storage.Instance.Heap_PE_C.Count == 0)
                {
                    ViewBag.Estatus = "PERSONAS EN ESPERA [" + Storage.Instance.Heap_PE_C.Count + "]";
                    ViewBag.Result = "Cola de confirmados vacia";
                    ViewBag.CamasDisponibles = Storage.Instance.Hashfinal.Positions(PatientModel.GetHash("PE"));
                    return View(Storage.Instance.Hashfinal.ToList(PatientModel.GetHash("PE")));
                }
                if (!Storage.Instance.Hashfinal.IsFull(PatientModel.GetHash("PE")))
                {
                    PatientModel patient = Storage.Instance.Heap_PE_C.RemoveRoot();
                    PatientModel.HashAdd(patient);
                    ViewBag.Result = "Paciente ingresado exitosamente";
                    ViewBag.CamasDisponibles = Storage.Instance.Hashfinal.Positions(PatientModel.GetHash("PE"));
                    if (Storage.Instance.Hashfinal.IsFull(PatientModel.GetHash("PE")))
                    {
                        ViewBag.CamasDisponibles = "[ -- ]";
                    }
                    ViewBag.Estatus = "PERSONAS EN ESPERA [" + Storage.Instance.Heap_PE_C.Count + "]";
                    return View(Storage.Instance.Hashfinal.ToList(PatientModel.GetHash("PE")));
                }
                else
                {
                    ViewBag.Result = "Camas llenas, el paciente permance en la cola";
                    ViewBag.CamasDisponibles = "[ -- ]";
                    ViewBag.Estatus = "PERSONAS EN ESPERA [" + Storage.Instance.Heap_PE_C.Count + "]";
                    return View(Storage.Instance.Hashfinal.ToList(PatientModel.GetHash("PE")));
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Remove_Peten(string id)
        {
            PatientModel.HashRemove("PE", id);
            Storage.Instance.statsSimulationRecuperados++;
            return RedirectToAction("Admin_Peten");
        }

        public ActionResult TestCovid()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TestCovid(FormCollection collection)
        {
            try
            {
                double resultado;
                PatientModel patient = new PatientModel();
                if (collection.AllKeys.Contains("GU"))
                {
                    if (Storage.Instance.Heap_GU_S.Count == 0)
                    {
                        ViewBag.estatus = "SIN PRUEBAS PENDIENTES POR REALIZAR";
                        return View(patient);
                    }
                    resultado = PatientModel.PruebaCovid();
                    if (resultado >= 60)
                    {
                        ViewBag.result = "POSITIVO   |  "+resultado + "% > 60%";
                        ViewBag.descripcion = PatientModel.DescripcionResultado(resultado);
                        Storage.Instance.statsSimulationConfirmados++;
                        patient = Storage.Instance.Heap_GU_S.RemoveRoot();
                        patient.Estatus = "CONFIRMADO";
                        patient.Prioridad = PatientModel.AsignarPrioridad(patient.Edad, patient.Estatus);
                        PatientModel.Heap_Add(patient);
                    }
                    else
                    {
                        ViewBag.result = "NEGATIVO   |  " + resultado + "% < 60%";
                        ViewBag.descripcion = PatientModel.DescripcionResultado(resultado);
                        Storage.Instance.statsSimulationSospechosos--;
                        patient = Storage.Instance.Heap_GU_S.RemoveRoot();
                    }
                    ViewBag.estatus = "PRUEBAS PENDIENTES HOSPITAL DE GUATEMALA ["+Storage.Instance.Heap_GU_S.Count+"]";
                }
                ///
                if (collection.AllKeys.Contains("ES"))
                {
                    if (Storage.Instance.Heap_ES_S.Count == 0)
                    {
                        ViewBag.estatus = "SIN PRUEBAS PENDIENTES POR REALIZAR";
                        return View(patient);
                    }
                    resultado = PatientModel.PruebaCovid();
                    if (resultado >= 60)
                    {
                        ViewBag.result = "POSITIVO   |  " + resultado + "% > 60%";
                        ViewBag.descripcion = PatientModel.DescripcionResultado(resultado);
                        Storage.Instance.statsSimulationConfirmados++;
                        patient = Storage.Instance.Heap_ES_S.RemoveRoot();
                        patient.Estatus = "CONFIRMADO";
                        patient.Prioridad = PatientModel.AsignarPrioridad(patient.Edad, patient.Estatus);
                        PatientModel.Heap_Add(patient);
                    }
                    else
                    {
                        ViewBag.result = "NEGATIVO   |  " + resultado + "% < 60%";
                        ViewBag.descripcion = PatientModel.DescripcionResultado(resultado);
                        Storage.Instance.statsSimulationSospechosos--;
                        patient = Storage.Instance.Heap_ES_S.RemoveRoot();
                    }
                    ViewBag.estatus = "PRUEBAS PENDIENTES HOSPITAL DE ESCUINTLA [" + Storage.Instance.Heap_ES_S.Count + "]";
                }
                ///
                if (collection.AllKeys.Contains("QZ"))
                {
                    if (Storage.Instance.Heap_QZ_S.Count == 0)
                    {
                        ViewBag.estatus = "SIN PRUEBAS PENDIENTES POR REALIZAR";
                        return View(patient);
                    }
                    resultado = PatientModel.PruebaCovid();
                    if (resultado >= 60)
                    {
                        ViewBag.result = "POSITIVO   |  " + resultado + "% > 60%";
                        ViewBag.descripcion = PatientModel.DescripcionResultado(resultado);
                        Storage.Instance.statsSimulationConfirmados++;
                        patient = Storage.Instance.Heap_QZ_S.RemoveRoot();
                        patient.Estatus = "CONFIRMADO";
                        patient.Prioridad = PatientModel.AsignarPrioridad(patient.Edad, patient.Estatus);
                        PatientModel.Heap_Add(patient);
                    }
                    else
                    {
                        ViewBag.result = "NEGATIVO   |  " + resultado + "% < 60%";
                        ViewBag.descripcion = PatientModel.DescripcionResultado(resultado);
                        Storage.Instance.statsSimulationSospechosos--;
                        patient = Storage.Instance.Heap_QZ_S.RemoveRoot();
                    }
                    ViewBag.estatus = "PRUEBAS PENDIENTES HOSPITAL DE QUETZALTENANGO [" + Storage.Instance.Heap_QZ_S.Count + "]";
                }
                ///
                if (collection.AllKeys.Contains("CQ"))
                {
                    if (Storage.Instance.Heap_CQ_S.Count == 0)
                    {
                        ViewBag.estatus = "SIN PRUEBAS PENDIENTES POR REALIZAR";
                        return View(patient);
                    }
                    resultado = PatientModel.PruebaCovid();
                    if (resultado >= 60)
                    {
                        ViewBag.result = "POSITIVO   |  " + resultado + "% > 60%";
                        ViewBag.descripcion = PatientModel.DescripcionResultado(resultado);
                        Storage.Instance.statsSimulationConfirmados++;
                        patient = Storage.Instance.Heap_CQ_S.RemoveRoot();
                        patient.Estatus = "CONFIRMADO";
                        patient.Prioridad = PatientModel.AsignarPrioridad(patient.Edad, patient.Estatus);
                        PatientModel.Heap_Add(patient);
                    }
                    else
                    {
                        ViewBag.result = "NEGATIVO   |  " + resultado + "% < 60%";
                        ViewBag.descripcion = PatientModel.DescripcionResultado(resultado);
                        Storage.Instance.statsSimulationSospechosos--;
                        patient = Storage.Instance.Heap_CQ_S.RemoveRoot();
                    }
                    ViewBag.estatus = "PRUEBAS PENDIENTES HOSPITAL DE CHIQUIMULA [" + Storage.Instance.Heap_CQ_S.Count + "]";
                }
                ///
                if (collection.AllKeys.Contains("PE"))
                {
                    if (Storage.Instance.Heap_PE_S.Count == 0)
                    {
                        ViewBag.estatus = "SIN PRUEBAS PENDIENTES POR REALIZAR";
                        return View(patient);
                    }
                    resultado = PatientModel.PruebaCovid();
                    if (resultado >= 60)
                    {
                        ViewBag.result = "POSITIVO   |  " + resultado + "% > 60%";
                        ViewBag.descripcion = PatientModel.DescripcionResultado(resultado);
                        Storage.Instance.statsSimulationConfirmados++;
                        patient = Storage.Instance.Heap_PE_S.RemoveRoot();
                        patient.Estatus = "CONFIRMADO";
                        patient.Prioridad = PatientModel.AsignarPrioridad(patient.Edad, patient.Estatus);
                        PatientModel.Heap_Add(patient);
                    }
                    else
                    {
                        ViewBag.result = "NEGATIVO   |  " + resultado + "% < 60%";
                        ViewBag.descripcion = PatientModel.DescripcionResultado(resultado);
                        Storage.Instance.statsSimulationSospechosos--;
                        patient = Storage.Instance.Heap_PE_S.RemoveRoot();
                    }
                    ViewBag.estatus = "PRUEBAS PENDIENTES HOSPITAL DE PETÉN [" + Storage.Instance.Heap_PE_S.Count + "]";
                }
                return View(patient);
            }
            catch
            {
                return View();
            }
        }
    }
}
