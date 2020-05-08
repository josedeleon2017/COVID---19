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

        ///<!--MANEJO DEL TIPO DE DATO-------------------------------------------------------------------->

        /// <summary>
        /// Compara y concatena las palabras conincidentes de dos archivos CSV 
        /// </summary>
        /// <param name="path">Direccion del archivo adjunto a comparar</param>
        /// <param name="pathdb">Direccion del archivo que contiente los criterios de comparación</param>
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
        /// <param name="Edad">Edad del paciente</param>
        /// <param name="Estatus">Estatus del paciente</param>
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
        /// <param name="Prioridad">Número representativo del grado de prioridad</param>
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

        /// <summary>
        /// Asigna el departamento con hospital mas cercano según el departamento donde se encuentre
        /// </summary>
        /// <param name="Departamento">Departamento del paciente</param>
        /// <returns>El nombre del departamento con hospital mas cercano</returns>
        public static string AsignarHospital(string Departamento)
        {
            string DepartamentoNormalizado = RemoverAcentos(Departamento.ToLower()).ToUpper().Trim();
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

        /// <summary>
        /// Convierte a texto las respuestas marcadas de los sintomas del paciente
        /// </summary>
        /// <param name="indicador1">Resultado de marcar o no el sintoma 1</param>
        /// <param name="indicador2">Resultado de marcar o no el sintoma 2</param>
        /// <param name="indicador3">Resultado de marcar o no el sintoma 3</param>
        /// <param name="indicador4">Resultado de marcar o no el sintoma 4</param>
        /// <param name="indicador5">Resultado de marcar o no el sintoma 5</param>
        /// <returns>Todos los sintomas marcados, concatenados en un solo texto</returns>
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

        /// <summary>
        /// Genera un resultado aleatorio entre 5 posibles escenarios
        /// </summary>
        /// <returns>Un número con dos decimales representado el resultado aleatorio</returns>
        public static double PruebaCovid()
        {
            double pbase = Convert.ToDouble(50);
            Random random = new Random();
            int resultado= random.Next(0,5);
            switch(resultado)
            {
                case (0): return pbase+5;
                case (1): return pbase+10;
                case (2): return pbase+15;
                case (3): return pbase+30;
                case (4): return pbase+5.5;
                default: return -1;
            }
        }

        /// <summary>
        /// Devuelve la descripción larga del resultado de la prueba de covid-19
        /// </summary>
        /// <param name="resultado">Valor númerico de del resultado de la prueba de covid-19</param>
        /// <returns>La descripción del posible resultado de la prueba</returns>
        public static string DescripcionResultado(double resultado)
        {
            if (resultado == 55.00) return "Probabilidad base 5%";
            if (resultado == 60.00) return "Viaje a Europa +10%";
            if (resultado == 65.00) return "Conocido contagiado +15%";
            if (resultado == 80.00) return "Familiar contagiado +30%";
            if (resultado == 55.5) return "Reuniones sociales con sospechosos +5%";
            return "";
        }

        /// <summary>
        /// Cuenta la cantidad de incidencias de un mismo estatus
        /// </summary>
        /// <param name="patient">Paciente a registrar su estatus</param>
        public static void CountEstatus(PatientModel patient)
        {
            if (patient.Estatus == "SOSPECHOSO")
            {
                Storage.Instance.statsSospechosos++;
            }
            if (patient.Estatus == "CONFIRMADO")
            {
                Storage.Instance.statsConfirmados++;
            }
            if (patient.Estatus == "RECUPERADO")
            {
                Storage.Instance.statsRecuperados++;
            }
        }

        ///<!--ESTRUCTURAS----------------------------------------------------------------------------------------->

        /// <summary>
        /// Inserción en el árbol de pacientes
        /// </summary>
        /// <param name="patient">Paciente a ingresar</param>
        public static void Tree_Add(PatientModel patient)
        {
            Storage.Instance.PatientTree.Comparer = CUIComparison;
            Storage.Instance.PatientTree.Root = Storage.Instance.PatientTree.InsertAVL(Storage.Instance.PatientTree.Root, patient);
        }

        /// <summary>
        /// Busqueda en el árbol de pacientes por llave
        /// </summary>
        /// <param name="patient">Paciente a buscar</param>     
        public static PatientModel Tree_Search(PatientModel key)
        {
            Storage.Instance.PatientTree.Comparer = CUIComparison;
            return Storage.Instance.PatientTree.Find(key);
        }

        /// <summary>
        /// Inserción en el Dictionary generico
        /// </summary>
        /// <param name="patient"></param>
        public static void HashAdd(PatientModel patient)
        {
            if (GetHash(patient.Hospital) == -1)
            {
                return;
            }
            else
            {
                Storage.Instance.Hashfinal.Add(GetHash(patient.Hospital), patient);
            }
        }

        /// <summary>
        /// Eliminación del Dictionary generico
        /// </summary>
        /// <param name="key">posición del dato</param>
        /// <param name="datakey">llave única del dato</param>
        public static void HashRemove(string key, string datakey)
        {
            Storage.Instance.Hashfinal.Comparer = PatientComparison;
            Storage.Instance.Hashfinal.Remove(GetHash(key), datakey);
        }

        /// <summary>
        /// Inserción de paciente a cola de prioridad (Heap) 
        /// </summary>
        /// <param name="patient"></param>
        public static void Heap_Add(PatientModel patient)
        {
            //Filtrado por estatus sospechoso.
            if (patient.Estatus == "SOSPECHOSO")
            {
                Storage.Instance.statsSimulationSospechosos++;
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
                    return;
                }
                if (patient.Hospital == "Petén")
                {
                    Storage.Instance.Heap_PE_S.GetPriorityValue = GetPriorityValue;
                    Storage.Instance.Heap_PE_S.Comparer = PatientHeapComparison;
                    Storage.Instance.Heap_PE_S.DateComparison = PatientDateComparison;
                    Storage.Instance.Heap_PE_S.Add(patient);
                    return;
                }
                if (patient.Hospital == "Quetzaltenango")
                {
                    Storage.Instance.Heap_QZ_S.GetPriorityValue = GetPriorityValue;
                    Storage.Instance.Heap_QZ_S.Comparer = PatientHeapComparison;
                    Storage.Instance.Heap_QZ_S.DateComparison = PatientDateComparison;
                    Storage.Instance.Heap_QZ_S.Add(patient);
                    return;
                }
                if (patient.Hospital == "Chiquimula")
                {
                    Storage.Instance.Heap_CQ_S.GetPriorityValue = GetPriorityValue;
                    Storage.Instance.Heap_CQ_S.Comparer = PatientHeapComparison;
                    Storage.Instance.Heap_CQ_S.DateComparison = PatientDateComparison;
                    Storage.Instance.Heap_CQ_S.Add(patient);
                    return;
                }
            }
            //Filtrado por status confirmado
            if (patient.Estatus == "CONFIRMADO")
            {
                Storage.Instance.statsSimulationConfirmados++;
                if (patient.Hospital == "Guatemala")
                {
                    Storage.Instance.Heap_GU_C.GetPriorityValue = GetPriorityValue;
                    Storage.Instance.Heap_GU_C.Comparer = PatientHeapComparison;
                    Storage.Instance.Heap_GU_C.DateComparison = PatientDateComparison;
                    Storage.Instance.Heap_GU_C.Add(patient);
                    return;
                }
                if (patient.Hospital == "Escuintla")
                {
                    Storage.Instance.Heap_ES_C.GetPriorityValue = GetPriorityValue;
                    Storage.Instance.Heap_ES_C.Comparer = PatientHeapComparison;
                    Storage.Instance.Heap_ES_C.DateComparison = PatientDateComparison;
                    Storage.Instance.Heap_ES_C.Add(patient);
                    return;
                }
                if (patient.Hospital == "Petén")
                {
                    Storage.Instance.Heap_PE_C.GetPriorityValue = GetPriorityValue;
                    Storage.Instance.Heap_PE_C.Comparer = PatientHeapComparison;
                    Storage.Instance.Heap_PE_C.DateComparison = PatientDateComparison;
                    Storage.Instance.Heap_PE_C.Add(patient);
                    return;
                }
                if (patient.Hospital == "Quetzaltenango")
                {
                    Storage.Instance.Heap_QZ_C.GetPriorityValue = GetPriorityValue;
                    Storage.Instance.Heap_QZ_C.Comparer = PatientHeapComparison;
                    Storage.Instance.Heap_QZ_C.DateComparison = PatientDateComparison;
                    Storage.Instance.Heap_QZ_C.Add(patient);
                    return;
                }
                if (patient.Hospital == "Chiquimula")
                {
                    Storage.Instance.Heap_CQ_C.GetPriorityValue = GetPriorityValue;
                    Storage.Instance.Heap_CQ_C.Comparer = PatientHeapComparison;
                    Storage.Instance.Heap_CQ_C.DateComparison = PatientDateComparison;
                    Storage.Instance.Heap_CQ_C.Add(patient);
                    return;
                }
            }
            //Filtrado por status recuperado
            if (patient.Estatus == "RECUPERADO")
            {
                Storage.Instance.statsSimulationRecuperados++;
            } 
        }

        /// <summary>
        /// Devuelve la posición de almacenamiento en el Dictionary genérico
        /// </summary>
        /// <param name="KeyCode">Llave única de identificación</param>
        /// <returns>La posición de la llave en el Dictionary</returns>
        public static int GetHash(string KeyCode)
        {
            if (KeyCode == "Guatemala" || KeyCode == "GU") return 0;
            if (KeyCode == "Escuintla" || KeyCode == "ES") return 1;
            if (KeyCode == "Chiquimula" || KeyCode == "CQ") return 2;
            if (KeyCode == "Quetzaltenango" || KeyCode == "QZ") return 3;
            if (KeyCode == "Petén" || KeyCode == "PE") return 4;
            return -1;
        }

        ///<!--DELGADOS---------------------------------------------------------------------------------------------------------------->


        /// <summary>
        /// Compara el número de CUI de cada paciente
        /// </summary>
        public static Func<PatientModel, string, int> PatientComparison = delegate (PatientModel patient, string cui)
        {
            if (patient.CUI == cui) return 0;
            return -1;
        };

        /// <summary>
        /// Devuelve el valor de prioridad de un paciente.
        /// </summary>
        public static Func<PatientModel, int> GetPriorityValue = delegate (PatientModel Patient)
        {
            return Patient.Prioridad;
        };

        /// <summary>
        /// Compara el CUI de dos pacientes.
        /// </summary>
        public static Comparison<PatientModel> PatientHeapComparison = delegate (PatientModel Patient1, PatientModel Patient2)
        {
            return Patient1.CUI.CompareTo(Patient2.CUI);
        };

        /// <summary>
        /// Compara la fecha de ingreso de dos pacientes.
        /// </summary>
        public static Comparison<PatientModel> PatientDateComparison = delegate (PatientModel Patient1, PatientModel Patient2)
        {
            return Patient1.FechaDeIngreso.CompareTo(Patient2.FechaDeIngreso);
        };       

    }
}