using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

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
            return 0;
        }

        /// <summary>
        /// Describe la definicón de la prioridad en texto
        /// </summary>
        /// <param name="Prioridad"></param>
        /// <returns>La descripción de la prioridad</returns>
        public static string DescripcionPrioridad(int Prioridad)
        {
            switch (Prioridad)
            {
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
            string DepartamentoNormalizado = RemoverAcentos(Departamento).ToUpper();
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
        /// Sustituye las vocales con acentos contenidas en el texto
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns>La palabra sin acentos</returns>
        public static string RemoverAcentos(string Departamento)
        {
            Regex Acentos_a = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex Acentos_e = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex Acentos_i = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex Acentos_o = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex Acentos_u = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            Departamento = Acentos_a.Replace(Departamento, "a");
            Departamento = Acentos_e.Replace(Departamento, "e");
            Departamento = Acentos_i.Replace(Departamento, "i");
            Departamento = Acentos_o.Replace(Departamento, "o");
            Departamento = Acentos_u.Replace(Departamento, "u");
            return Departamento;
        }

    }
}