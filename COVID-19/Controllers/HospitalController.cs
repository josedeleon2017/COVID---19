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
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Hospital/Delete/5
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

        public ActionResult Simulation()
        {
            int Sospechosos = Storage.Instance.PatientTree.ToInOrden().Where(x => x.Estatus == "SOSPECHOSO").ToList().Count();
            int Confirmados = Storage.Instance.PatientTree.ToInOrden().Where(x => x.Estatus == "CONFIRMADO").ToList().Count();
            int Recuperados = Storage.Instance.PatientTree.ToInOrden().Where(x => x.Estatus == "RECUPERADO").ToList().Count();
            double Porcentaje = Convert.ToDouble(Confirmados) / (Convert.ToDouble(Confirmados) + Convert.ToDouble(Recuperados)) * 100;

            ViewBag.Sospechosos = Sospechosos;
            ViewBag.Confirmados = Confirmados;
            ViewBag.Porcentaje = Convert.ToString(Math.Round(Porcentaje, 2)) + "%";
            ViewBag.Recuperados = Recuperados;

            ViewBag.HospitalGuatemala = PersonModel.HashTable_CountEmptys("GU");
            ViewBag.HospitalEscuintla = PersonModel.HashTable_CountEmptys("ES");
            ViewBag.HospitalQuetzaltenango = PersonModel.HashTable_CountEmptys("QZ");
            ViewBag.HospitalChiquimula = PersonModel.HashTable_CountEmptys("CQ");
            ViewBag.HospitalPeten = PersonModel.HashTable_CountEmptys("PE");
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
                if(Storage.Instance.HospitalHashTable.ContainsKey(HashCode.Substring(0, 2)))
                {
                    ViewBag.Result = "[ " + HashCode + " ]";
                    PersonModel personResult = PersonModel.HashTable_Find(HashCode);
                    if (personResult == null)
                    {
                        ViewBag.Result = "[ CAMA VACÍA ]";
                        return View();
                    }
                    return View(personResult);
                }
                else
                {
                    ViewBag.Result = "[ HASH INVALIDO ]";
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
            ViewBag.CamasDisponibles = PersonModel.HashTable_Positions("GU");
            return View(Storage.Instance.PersonTree.ToInOrden());
        }

        [HttpPost]
        public ActionResult Admin_Guatemala(FormCollection collection)
        {
            try
            {

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
