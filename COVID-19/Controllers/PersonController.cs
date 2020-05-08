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
                    if (PersonModel.Tree_Search(personToSearch) == null)
                    {
                        ViewBag.Result = "No se encontraron resultados";
                        return View(Storage.Instance.ResultList);
                    }
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();
                    Storage.Instance.ResultList.Add(PersonModel.Tree_Search(personToSearch));
                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    string elapsedTime = Convert.ToString(ts.TotalMilliseconds);
                    ViewBag.Query = "Consulta realizada en " + elapsedTime + " ms";
                    return View(Storage.Instance.ResultList);
                }
                if (Nombre != "" && Apellido != "")
                {
                    string key = Nombre + Apellido;
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();
                    Storage.Instance.ResultList = PersonModel.CustomTree_FilterNA(key).Where(x => x.Nombre == Nombre && x.Apellido == Apellido).ToList();
                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    string elapsedTime = Convert.ToString(ts.TotalMilliseconds);
                    ViewBag.Query = "Consulta realizada en " + elapsedTime + " ms";
                    if (Storage.Instance.ResultList.Count == 0)
                    {
                        ViewBag.Result = "No se encontraron resultados";
                        return View(Storage.Instance.ResultList);
                    }
                    return View(Storage.Instance.ResultList);
                }
                if (Nombre != "" || Apellido != "")
                {
                    if (Nombre != "")
                    {
                        Stopwatch stopWatch = new Stopwatch();
                        stopWatch.Start();
                        Storage.Instance.ResultList = PersonModel.CustomTree_Filter(Nombre).Where(x => x.Nombre == Nombre).ToList();
                        stopWatch.Stop();
                        TimeSpan ts = stopWatch.Elapsed;
                        string elapsedTime = Convert.ToString(ts.TotalMilliseconds);
                        ViewBag.Query = "Consulta realizada en " + elapsedTime + " ms";
                        if (Storage.Instance.ResultList.Count == 0)
                        {
                            ViewBag.Result = "No se encontraron resultados";
                            return View(Storage.Instance.ResultList);
                        }
                        return View(Storage.Instance.ResultList);
                    }
                    if (Apellido != "")
                    {
                        Stopwatch stopWatch = new Stopwatch();
                        stopWatch.Start();
                        Storage.Instance.ResultList = PersonModel.CustomTree_FilterA(Apellido).Where(x => x.Apellido == Apellido).ToList();
                        stopWatch.Stop();
                        TimeSpan ts = stopWatch.Elapsed;
                        string elapsedTime = Convert.ToString(ts.TotalMilliseconds);
                        ViewBag.Query = "Consulta realizada en " + elapsedTime + " ms";
                        if (Storage.Instance.ResultList.Count == 0)
                        {
                            ViewBag.Result = "No se encontraron resultados";
                            return View(Storage.Instance.ResultList);
                        }
                        return View(Storage.Instance.ResultList);
                    }
                }

                ////Lista temporal con todos los datos
                //List<PersonModel> temporalList = Storage.Instance.PersonTree.ToInOrden();
                //int count = 0;
                //for (int i = 0; i < temporalList.Count(); i++)
                //{
                //    PersonModel currentPerson = temporalList.ElementAt(i);

                //    int count1 = 0;
                //    int count2 = 0;

                //    //Filtrado artesanal
                //    Stopwatch stopWatch = new Stopwatch();
                //    stopWatch.Start();
                //    count1 = PersonModel.CustomTree_Filter(currentPerson.Nombre).Where(x => x.Nombre == currentPerson.Nombre).Count();
                //    stopWatch.Stop();
                //    TimeSpan ts = stopWatch.Elapsed;
                //    string elapsedTime = Convert.ToString(ts.TotalMilliseconds);

                //    //Filtrado de LINQ
                //    Stopwatch stopWatch2 = new Stopwatch();
                //    stopWatch2.Start();
                //    count2 = Storage.Instance.PersonTree.ToInOrden().Where(x => x.Nombre == currentPerson.Nombre).Count();
                //    stopWatch2.Stop();
                //    TimeSpan ts2 = stopWatch2.Elapsed;
                //    string elapsedTime2 = Convert.ToString(ts2.TotalMilliseconds);

                //    //Si difieren en las cantidades resultantes devuelve un error
                //    if (count1 != count2)
                //    {
                //        return View("Error");
                //    }
                //    //Cuenta el numero de veces que el filtrado artesanal es mas rapido que el de LINQ
                //    if (Convert.ToDouble(elapsedTime) < Convert.ToDouble(elapsedTime2))
                //    {
                //        count++;
                //    }

                //}
                return View(Storage.Instance.ResultList);
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
