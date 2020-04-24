using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace COVID_19.Models
{
    public class PersonModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "FechaDeNacimiento")]
        [Required]
        public DateTime FechaDeNacimiento { get; set; }
        public string CUI { get; set; }
        public string Departamento { get; set; }
        public string Municipio { get; set; }
        public string Edad { get; set; }

        /// <summary>
        /// Calcula la cantidad de años, meses o días que han transcurrido hasta la fecha actual
        /// </summary>
        /// <param name="FechaDeNacimiento"></param>
        /// <returns>Retorna una cantidad de años, meses o días</returns>
        public static string CalcularEdad(DateTime FechaDeNacimiento)
        {
            int Cantidad;
            if (FechaDeNacimiento < DateTime.Now)
            {
                if (FechaDeNacimiento.Year == DateTime.Now.Year && FechaDeNacimiento.Month == DateTime.Now.Month)
                {
                    DateTime inicio = FechaDeNacimiento;
                    DateTime final = DateTime.Now;

                    TimeSpan diferencia = final - inicio;

                    Cantidad = diferencia.Days;

                    if (Cantidad == 1)
                    {
                        return Cantidad.ToString() + " día";
                    }
                    return Cantidad.ToString() + " días";
                }
                if (FechaDeNacimiento.Year == DateTime.Now.Year)
                {
                    Cantidad = DateTime.Now.Month - FechaDeNacimiento.Month;
                    if (FechaDeNacimiento.Day > DateTime.Now.Day)
                    {
                        Cantidad--;
                    }
                    if (Cantidad == 0)
                    {
                        DateTime inicio = FechaDeNacimiento;
                        DateTime final = DateTime.Now;

                        TimeSpan diferencia = final - inicio;

                        Cantidad = diferencia.Days;
                        return Cantidad.ToString() + " días";
                    }
                    if (Cantidad == 1)
                    {
                        return Cantidad.ToString() + " mes";
                    }
                    return Cantidad.ToString() + " meses";
                }
                Cantidad = DateTime.Now.Year - FechaDeNacimiento.Year;
                if (FechaDeNacimiento.Month == DateTime.Now.Month)
                {
                    if (FechaDeNacimiento.Day > DateTime.Now.Day)
                    {
                        Cantidad--;
                    }
                }
                if (FechaDeNacimiento.Month > DateTime.Now.Month)
                {
                    Cantidad--;
                }
                if (Cantidad != 0)
                {
                    if (Cantidad == 1)
                    {
                        return Cantidad.ToString() + " año";
                    }
                    return Cantidad.ToString() + " años";
                }
                else
                {
                    Cantidad = DateTime.Now.Month - FechaDeNacimiento.Month + 12;
                    if (Cantidad == 12)
                    {
                        Cantidad--;
                    }
                    if (Cantidad > 12)
                    {
                        Cantidad = Cantidad - 12;
                    }
                    if (Cantidad == 1)
                    {
                        return Cantidad.ToString() + " mes";
                    }
                    return Cantidad.ToString() + " meses";
                }
            }
            return null;
        }

    }
}