using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using COVID_19.Helpers;

namespace COVID_19.Models
{
    public class PatientModel : PersonModel
    {
        public string Sintomas { get; set; }
        public string Descripcion { get; set; }
        public string Estatus { get; set; }
        public string Categoria { get; set; }
        public int Prioridad { get; set; }
        public string Hospital { get; set; }
        public DateTime FechaDeIngreso { get; set; }

        /// <summary>
        /// Compara y concatena las palabras conincidentes de dos archivos CSV 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pathdb"></param>
        /// <returns>Palabras clave definidas por pathdb</returns>
        public static string ObtenerCoincidencias(string path, string pathdb)
        {
            string Resultado = "";
            List<string> PalabrasClave = new List<string>();
            using (var fileStream = new FileStream(pathdb, FileMode.Open))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    streamReader.ReadLine();
                    while (!streamReader.EndOfStream)
                    {
                        var row = streamReader.ReadLine();
                        Regex regx = new Regex("," + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                        string[] line = regx.Split(row);
                        PalabrasClave.Add(line[0]);
                    }
                }
            }
            string readText = File.ReadAllText(path);
            for (int i = 0; i < PalabrasClave.Count(); i++)
            {
                if (readText.ToUpper().Contains(PalabrasClave.ElementAt(i).ToUpper()))
                {
                    Resultado += PalabrasClave.ElementAt(i) + ", ";
                }
            }
            return Resultado;
        }

        /// <summary>
        /// Descompone los atributos de la edad y asigna una prioridad según su estatus
        /// </summary>
        /// <param name="Edad"></param>
        /// <param name="Estatus"></param>
        /// <returns>El número de prioridad</returns>
        public static int AsignarPrioridad(string Edad, string Estatus)
        {
            if (Estatus.ToUpper() == "RECUPERADO")
            {
                return 0;
            }
            if (Edad.Contains("año") || Edad.Contains("años"))
            {
                int edad = 0;
                bool edadEncontrada = true;
                int i = 0;

                while (edadEncontrada)
                {
                    if (Edad[i].ToString() == " ")
                    {
                        edad = Convert.ToInt32(Edad.Substring(0, i));
                        edadEncontrada = false;
                    }
                    i++;
                }

                if (edad >= 1 && edad <= 26 && Estatus.ToUpper() == "SOSPECHOSO")
                {
                    return 8;
                }
                if (edad >= 27 && edad <= 59 && Estatus.ToUpper() == "SOSPECHOSO")
                {
                    return 7;
                }
                if (edad >= 1 && edad <= 26 && Estatus.ToUpper() == "CONFIRMADO")
                {
                    return 5;
                }
                if (edad >= 60 && Estatus.ToUpper() == "SOSPECHOSO")
                {
                    return 4;
                }
                if (edad >= 27 && edad <= 59 && Estatus.ToUpper() == "CONFIRMADO")
                {
                    return 3;
                }
                if (edad >= 60 && Estatus.ToUpper() == "CONFIRMADO")
                {
                    return 1;
                }
            }
            else
            {
                if (Estatus.ToUpper() == "SOSPECHOSO")
                {
                    return 6;
                }
                if (Estatus.ToUpper() == "CONFIRMADO")
                {
                    return 2;
                }
            }
            return -1;
        }

        /// <summary>
        /// Describe la definición de la prioridad en texto
        /// </summary>
        /// <param name="Prioridad"></param>
        /// <returns>La descripción de la prioridad</returns>
        public static string DescripcionPrioridad(int Prioridad)
        {
            switch (Prioridad)
            {
                case (0): return "Paciente recuperado";
                case (1): return "Paciente confirmado de la 3era edad";
                case (2): return "Paciente confirmado recién nacido";
                case (3): return "Paciente confirmado adulto";
                case (4): return "Paciente sospechoso de la 3era edad";
                case (5): return "Paciente confirmado niño o joven";
                case (6): return "Paciente sospechoso recién nacido";
                case (7): return "Paciente sospechoso adulto";
                case (8): return "Paciente sospechoso niño o joven";
                default: return "";
            }
        }

        public static string AsignarHospital(string Departamento)
        {
            string DepartamentoNormalizado = RemoverAcentos(Departamento.ToLower()).ToUpper();
            switch (DepartamentoNormalizado)
            {
                case ("PETEN"): return "Petén";
                case ("ALTA VERAPAZ"): return "Petén";
                case ("QUICHE"): return "Petén";
                case ("HUEHUETENANGO"): return "Petén";

                case ("QUETZALTENANGO"): return "Quetzaltenango";
                case ("SAN MARCOS"): return "Quetzaltenango";
                case ("TOTONICAPAN"): return "Quetzaltenango";
                case ("SOLOLA"): return "Quetzaltenango";

                case ("ESCUINTLA"): return "Escuintla";
                case ("RETALHULEU"): return "Escuintla";
                case ("SUCHITEPEQUEZ"): return "Escuintla";
                case ("SANTA ROSA"): return "Escuintla";

                case ("GUATEMALA"): return "Guatemala";
                case ("EL PROGRESO"): return "Guatemala";
                case ("BAJA VERAPAZ"): return "Guatemala";
                case ("SACATEPEQUEZ"): return "Guatemala";
                case ("CHIMALTENANGO"): return "Guatemala";

                case ("CHIQUIMULA"): return "Chiquimula";
                case ("IZABAL"): return "Chiquimula";
                case ("ZACAPA"): return "Chiquimula";
                case ("JALAPA"): return "Chiquimula";
                case ("JUTIAPA"): return "Chiquimula";

                default: return "";
            }
        }

        public static string DescripcionSintomas(string indicador1, string indicador2, string indicador3, string indicador4, string indicador5)
        {
            string sintomas = "";
            if (indicador1 == "on") sintomas += "Fiebre,";
            if (indicador2 == "on") sintomas += " Tos seca,";
            if (indicador3 == "on") sintomas += " Cansancio,";
            if (indicador4 == "on") sintomas += " Dificultad respiratoria,";
            if (indicador5 == "on") sintomas += " Escalofríos y dolores corporales";
            return sintomas;
        }

        public static int PruebaCovid(int pbase)
        {
            Random random = new Random();
            int resultado= random.Next(0,5);
            switch(resultado)
            {
                case (0): return pbase+5;
                case (1): return pbase+10;
                case (2): return pbase+15;
                case (3): return pbase+30;
                case (4): return pbase+5;
                default: return -1;
            }
        }

        //INSERT TREE
        public static void Tree_Add(PatientModel patient)
        {
            Storage.Instance.PatientTree.Comparer = CUIComparison;
            Storage.Instance.PatientTree.Root = Storage.Instance.PatientTree.InsertAVL(Storage.Instance.PatientTree.Root, patient);
        }

        //SEARCH TREE               
        public static PatientModel Tree_Search(PatientModel key)
        {
            Storage.Instance.PatientTree.Comparer = CUIComparison;
            return Storage.Instance.PatientTree.Find(key);
        }

        //HASH
        public static void HashTable_Add(string key, PatientModel patient)
        {
            Storage.Instance.PatientHashTable.Add(key,patient);
        }
        public static void HashTable_Delete(string key, string datakey)
        {
            Storage.Instance.PatientHashTable.Comparer = PatientComparison;
            Storage.Instance.PatientHashTable.Remove(key, datakey);
        }

        public static Func<PatientModel, string, int> PatientComparison = delegate (PatientModel patient, string cui)
        {
            if (patient.CUI == cui) return 0;
            return -1;
        };

        //INSERT HEAP
        public static void Heap_Add(PatientModel patient)
        {
            if (patient.Estatus == "SOSPECHOSO")
            {
                Storage.Instance.statsSospechosos++;
                if (patient.Hospital == "Guatemala")
                {
                    Storage.Instance.Heap_GU_S.GetPriorityValue = GetPriorityValue;
                    Storage.Instance.Heap_GU_S.Comparer = PatientHeapComparison;
                    Storage.Instance.Heap_GU_S.DateComparison = PatientDateComparison;
                    Storage.Instance.Heap_GU_S.Add(patient);
                    return;
                }
                if (patient.Hospital == "Escuintla")
                {
                    Storage.Instance.Heap_ES_S.GetPriorityValue = GetPriorityValue;
                    Storage.Instance.Heap_ES_S.Comparer = PatientHeapComparison;
                    Storage.Instance.Heap_ES_S.DateComparison = PatientDateComparison;
                    Storage.Instance.Heap_ES_S.Add(patient);
                    //guardar heap de sospechosos de escuintla
                    return;
                }
                if (patient.Hospital == "Petén")
                {
                    Storage.Instance.Heap_PE_S.GetPriorityValue = GetPriorityValue;
                    Storage.Instance.Heap_PE_S.Comparer = PatientHeapComparison;
                    Storage.Instance.Heap_PE_S.DateComparison = PatientDateComparison;
                    Storage.Instance.Heap_PE_S.Add(patient);
                    //guardar heap de sospechosos de peten
                    return;
                }
                if (patient.Hospital == "Quetzaltenango")
                {
                    Storage.Instance.Heap_QZ_S.GetPriorityValue = GetPriorityValue;
                    Storage.Instance.Heap_QZ_S.Comparer = PatientHeapComparison;
                    Storage.Instance.Heap_QZ_S.DateComparison = PatientDateComparison;
                    Storage.Instance.Heap_QZ_S.Add(patient);
                    //guardar heap de sospechosos de quetzaltenango
                    return;
                }
                if (patient.Hospital == "Chiquimula")
                {
                    Storage.Instance.Heap_CQ_S.GetPriorityValue = GetPriorityValue;
                    Storage.Instance.Heap_CQ_S.Comparer = PatientHeapComparison;
                    Storage.Instance.Heap_CQ_S.DateComparison = PatientDateComparison;
                    Storage.Instance.Heap_CQ_S.Add(patient);
                    //guardar heap de sospechosos de chiquimula
                    return;
                }
            }
            if(patient.Estatus == "CONFIRMADO")
            {
                Storage.Instance.statsConfirmados++;
                if (patient.Hospital == "Guatemala")
                {
                    Storage.Instance.Heap_GU_C.GetPriorityValue = GetPriorityValue;
                    Storage.Instance.Heap_GU_C.Comparer = PatientHeapComparison;
                    Storage.Instance.Heap_GU_C.DateComparison = PatientDateComparison;
                    Storage.Instance.Heap_GU_C.Add(patient);
                    //guardar heap de confirmados de guate
                    return;
                }
                if (patient.Hospital == "Escuintla")
                {
                    Storage.Instance.Heap_ES_C.GetPriorityValue = GetPriorityValue;
                    Storage.Instance.Heap_ES_C.Comparer = PatientHeapComparison;
                    Storage.Instance.Heap_ES_C.DateComparison = PatientDateComparison;
                    Storage.Instance.Heap_ES_C.Add(patient);
                    //guardar heap de confirmados de escuintla
                    return;
                }
                if (patient.Hospital == "Petén")
                {
                    Storage.Instance.Heap_PE_C.GetPriorityValue = GetPriorityValue;
                    Storage.Instance.Heap_PE_C.Comparer = PatientHeapComparison;
                    Storage.Instance.Heap_PE_C.DateComparison = PatientDateComparison;
                    Storage.Instance.Heap_PE_C.Add(patient);
                    //guardar heap de confirmados de peten
                    return;
                }
                if (patient.Hospital == "Quetzaltenango")
                {
                    Storage.Instance.Heap_QZ_C.GetPriorityValue = GetPriorityValue;
                    Storage.Instance.Heap_QZ_C.Comparer = PatientHeapComparison;
                    Storage.Instance.Heap_QZ_C.DateComparison = PatientDateComparison;
                    Storage.Instance.Heap_QZ_C.Add(patient);
                    //guardar heap de confirmados de quetzaltenango
                    return;
                }
                if (patient.Hospital == "Chiquimula")
                {
                    Storage.Instance.Heap_CQ_C.GetPriorityValue = GetPriorityValue;
                    Storage.Instance.Heap_CQ_C.Comparer = PatientHeapComparison;
                    Storage.Instance.Heap_CQ_C.DateComparison = PatientDateComparison;
                    Storage.Instance.Heap_CQ_C.Add(patient);
                    //guardar heap de confirmados de chiquimula
                    return;
                }
            }
            if(patient.Estatus == "RECUPERADO")
            {
                Storage.Instance.statsRecuperados++;
            }

        }

        public static Func<PatientModel, int> GetPriorityValue = delegate (PatientModel Patient)
        {
            return Patient.Prioridad;
        };

        public static Comparison<PatientModel> PatientHeapComparison = delegate (PatientModel Patient1, PatientModel Patient2)
        {
            return Patient1.CUI.CompareTo(Patient2.CUI);
        };

        public static Comparison<PatientModel> PatientDateComparison = delegate (PatientModel Patient1, PatientModel Patient2)
        {
            return Patient1.FechaDeIngreso.CompareTo(Patient2.FechaDeIngreso);
        };



    }
}