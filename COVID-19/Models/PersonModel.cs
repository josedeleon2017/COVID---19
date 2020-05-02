using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;
using COVID_19.Helpers;
using System.Text.RegularExpressions;

namespace COVID_19.Models
{
    public class PersonModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyy}", ApplyFormatInEditMode = true)]
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

        /// <summary>
        /// Sustituye las vocales con acentos contenidas en el texto
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns>La palabra sin acentos</returns>
        public static string RemoverAcentos(string key)
        {
            Regex Acentos_a = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex Acentos_e = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex Acentos_i = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex Acentos_o = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex Acentos_u = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            key = Acentos_a.Replace(key, "a");
            key = Acentos_e.Replace(key, "e");
            key = Acentos_i.Replace(key, "i");
            key = Acentos_o.Replace(key, "o");
            key = Acentos_u.Replace(key, "u");
            return key;
        }

        //SEARCH TREE               
        public static PersonModel Tree_Search(PersonModel key)
        {
            Storage.Instance.PersonTree.Comparer = CUIComparison;
            return Storage.Instance.PersonTree.Find(key);
        }

        //INSERT TREE
        public static void Tree_Add(PersonModel person)
        {
            Storage.Instance.PersonTree.Comparer = CUIComparison;
            Storage.Instance.PersonTree.Root = Storage.Instance.PersonTree.InsertAVL(Storage.Instance.PersonTree.Root, person);
        }

        public static void CustomTree_Add(PersonModel person)
        {
            Storage.Instance.CustomPersonTree.Comparer = PersonComparison;
            Storage.Instance.CustomPersonTree.Root = Storage.Instance.CustomPersonTree.InsertAVL(Storage.Instance.CustomPersonTree.Root, person);
        }

        public static List<PersonModel> CustomTree_Filter(string key)
        {
            Storage.Instance.CustomPersonTree.Comparer = PersonComparison;
            PersonModel persontTest = Storage.Instance.CustomPersonTree.Root.Value;
            if (persontTest.Nombre.CompareTo(key) == 1)
            {
                return Storage.Instance.CustomPersonTree.Filter(false);
            }
            else
            {
                return Storage.Instance.CustomPersonTree.Filter(true);
            }           
        }

        //DELEGATES
        public static Comparison<PersonModel> CUIComparison = delegate (PersonModel person1, PersonModel person2)
        {
            if (Convert.ToInt32(person1.CUI) > Convert.ToInt32(person2.CUI)) return 1;
            if (Convert.ToInt32(person1.CUI) < Convert.ToInt32(person2.CUI)) return -1;
            return 0;
        };

        public static Comparison<PersonModel> PersonComparison = delegate (PersonModel person1, PersonModel person2)
        {
            string keyPerson1 = person1.Nombre + person1.Apellido + person1.CUI;
            string keyPerson2 = person2.Nombre + person2.Apellido + person2.CUI;
            return keyPerson1.CompareTo(keyPerson2);
        };

        public static bool HashTable_Add(string key, PersonModel person)
        {
            if (Storage.Instance.HospitalHashTable.ContainsKey(key))
            {
                for (int i = 0; i < Storage.Instance.HospitalHashTable[key].Length; i++)
                {
                    if (Storage.Instance.HospitalHashTable[key][i] == null)
                    {
                        Storage.Instance.HospitalHashTable[key][i] = person;
                        return true;
                    }
                }
            }
            else
            {
                PersonModel[] currentArray = new PersonModel[10];
                Storage.Instance.HospitalHashTable.Add(key, currentArray);
                Storage.Instance.HospitalHashTable[key][0] = person;
                return true;
            }
            return false;
        }

        public static PersonModel HashTable_Find(string key)
        {
            string index = key.Substring(0, 2);
            if (Storage.Instance.HospitalHashTable.ContainsKey(index))
            {
                if (key.Contains("10"))
                {
                    int position = Convert.ToInt32(key.Substring(3, 2));
                    return Storage.Instance.HospitalHashTable[index][position - 1];
                }
                else
                {
                    int position = Convert.ToInt32(key.Substring(3, 1));
                    return Storage.Instance.HospitalHashTable[index][position - 1];
                }
            }
            else
            {
                return null;
            }
        }

        public static int HashTable_CountEmptys(string key)
        {
            if (!Storage.Instance.HospitalHashTable.ContainsKey(key))
            {
                return 10;
            }
            int count = 0;
            for (int i = 0; i < Storage.Instance.HospitalHashTable[key].Count(); i++)
            {
                if (Storage.Instance.HospitalHashTable[key][i]==null)
                {
                    count++;
                }
            }
            return count;
        }

        public static string HashTable_Positions(string key)
        {
            string index = key.Substring(0, 2);
            if (Storage.Instance.HospitalHashTable.ContainsKey(index))
            {
                string result = "";
                for (int i = 0; i < 10; i++)
                {
                    if (Storage.Instance.HospitalHashTable[index][i] == null)
                    {
                        int position = i + 1;
                        result += " [" + Convert.ToString(position) + "] ";
                    }
                }
                return result;
            }
            else
            {
                return " [1-10] ";
            }
        }

        public static void HashTable_Remove(string key, string CUI)
        {
            string index = key.Substring(0, 2);
            if (Storage.Instance.HospitalHashTable.ContainsKey(index))
            {
                for (int i = 0; i < 10; i++)
                {
                    if (Storage.Instance.HospitalHashTable[index][i].CUI == CUI)
                    {
                        Storage.Instance.HospitalHashTable[index][i] = null;
                        return;
                    }
                }
            }
        }

    }
}