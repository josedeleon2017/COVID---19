using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COVID_19.Helpers;
using COVID_19.Models;
using System.Diagnostics;

namespace COVID_19.Controllers
{
    public class PersonController : Controller
    {
        // GET: Person
        public ActionResult Index()
        {
            return View();
        }

        // GET: Person/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Person/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Person/Create
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

        // GET: Person/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Person/Edit/5
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

        // GET: Person/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Person/Delete/5
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

        public ActionResult Search()
        {
            Storage.Instance.ResultList.Clear();
            return View(Storage.Instance.ResultList);
        }

        [HttpPost]
        public ActionResult Search(FormCollection collection)
        {
            try
            {
                Storage.Instance.ResultList.Clear();

                string Nombre = collection["Nombre"];
                string Apellido = collection["Apellido"];
                string CUI = collection["CUI"];

                if (Nombre == "" && Apellido == "" && CUI == "")
                {
                    ViewBag.Result = "Debe llenar al menos un campo requerido";
                    return View(Storage.Instance.ResultList);
                }
                if (CUI != "")
                {
                    PersonModel personToSearch = new PersonModel { CUI = CUI };
                    Storage.Instance.ResultList.Add(PersonModel.Tree_Search(personToSearch));
                    return View(Storage.Instance.ResultList);
                }
                if (Nombre != "" && Apellido != "")
                {
                    string key = Nombre + Apellido;
                    Storage.Instance.ResultList = PersonModel.CustomTree_Filter(key).Where(x=>x.Nombre==Nombre && x.Apellido==Apellido).ToList();
                    return View(Storage.Instance.ResultList);
                }
                if (Nombre != "" || Apellido != "")
                {
                    if (Nombre != "")
                    {
                        Storage.Instance.ResultList = PersonModel.CustomTree_Filter(Nombre).Where(x => x.Nombre == Nombre).ToList();
                        return View(Storage.Instance.ResultList);
                    }
                    else
                    {
                        Storage.Instance.ResultList = PersonModel.CustomTree_Filter(Apellido).Where(x => x.Apellido == Apellido).ToList();
                        return View(Storage.Instance.ResultList);
                    }
                }
                return View();
            }
            catch
            {
                return View("Error");
            }
        }

        public new ActionResult Profile(string id)
        {

            PatientModel patient = new PatientModel { CUI = id };
            patient = PatientModel.Tree_Search(patient);
            return View(patient);
        }

    }
}
