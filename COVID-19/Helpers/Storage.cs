using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COVID_19.Models;

namespace COVID_19.Helpers
{
    public class Storage
    {
        private static Storage _instance = null;

        public static Storage Instance
        {
            get
            {
                if (_instance == null) _instance = new Storage();
                return _instance;
            }
        }

        ///<!--MANEJO DEL CSV-->
        ///<summary>
        ///Validador primera carga de data.cvs
        ///</summary>
        public bool first_load = true;

        //ESTRUCTURAS


        /// <summary>
        /// Árbol AVL de personas, sirve como primer filtro para la búsqueda de pacientes (llave CUI)
        /// </summary>
        public NoLinealStructures.Structures.Tree<PersonModel> PersonTree = new NoLinealStructures.Structures.Tree<PersonModel>();

        /// <summary>
        /// Árbol AVL de customizado de personas, sirve para optimizar la busqueda por nombre y apellido (llave Nombre+Apellido+CUI)
        /// </summary>
        public NoLinealStructures.Structures.Tree<PersonModel> CustomPersonTree = new NoLinealStructures.Structures.Tree<PersonModel>();

        /// <summary>
        /// Árbol AVL de pacientes, sirve para mostrar la descripción completa de la persona (llave CUI)
        /// </summary>
        public NoLinealStructures.Structures.Tree<PatientModel> PatientTree = new NoLinealStructures.Structures.Tree<PatientModel>();       

        /// <summary>
        /// Lista simplemente enlazada de personas, sirve para mostrar el resultado de la búsqueda o filtro
        /// </summary>
        public List<PersonModel> ResultList = new List<PersonModel>();

        /// <summary>
        /// Tabla Hash de pacientes, sirve para guardar los datos del hospital
        /// </summary>
        public NoLinealStructures.Structures.CustomDictionary<PatientModel> PatientHashTable = new NoLinealStructures.Structures.CustomDictionary<PatientModel>();

        /// <summary>
        /// Heap donde se guardan los pacientes confirmados asignados al hospital del departamento de Guatemala.
        /// </summary>
        public NoLinealStructures.Structures.Heap<PatientModel> Heap_GU_C = new NoLinealStructures.Structures.Heap<PatientModel>();

        /// <summary>
        /// Heap donde se guardan los pacientes sospechosos asignados al hospital del departamento de Guatemala.
        /// </summary>
        public NoLinealStructures.Structures.Heap<PatientModel> Heap_GU_S = new NoLinealStructures.Structures.Heap<PatientModel>();

        /// <summary>
        /// Heap donde se guardan los pacientes confirmados asignados al hospital del departamento de Escuintla.
        /// </summary>
        public NoLinealStructures.Structures.Heap<PatientModel> Heap_ES_C = new NoLinealStructures.Structures.Heap<PatientModel>();

        /// <summary>
        /// Heap donde se guardan los pacientes sospechosos asignados al hospital del departamento de Escuintla.
        /// </summary>
        public NoLinealStructures.Structures.Heap<PatientModel> Heap_ES_S = new NoLinealStructures.Structures.Heap<PatientModel>();

        /// <summary>
        /// Heap donde se guardan los pacientes confirmados asignados al hospital del departamento de Quetzaltenango.
        /// </summary>
        public NoLinealStructures.Structures.Heap<PatientModel> Heap_QZ_C = new NoLinealStructures.Structures.Heap<PatientModel>();

        /// <summary>
        /// Heap donde se guardan los pacientes sospechosos asignados al hospital del departamento de Quetzaltenango.
        /// </summary>
        public NoLinealStructures.Structures.Heap<PatientModel> Heap_QZ_S = new NoLinealStructures.Structures.Heap<PatientModel>();

        /// <summary>
        /// Heap donde se guardan los pacientes confirmados asignados al hospital del departamento de Chiquimula.
        /// </summary>
        public NoLinealStructures.Structures.Heap<PatientModel> Heap_CQ_C = new NoLinealStructures.Structures.Heap<PatientModel>();

        /// <summary>
        /// Heap donde se guardan los pacientes sospechosos asignados al hospital del departamento de Chiquimula.
        /// </summary>
        public NoLinealStructures.Structures.Heap<PatientModel> Heap_CQ_S = new NoLinealStructures.Structures.Heap<PatientModel>();

        /// <summary>
        /// Heap donde se guardan los pacientes confirmados asignados al hospital del departamento de Petén.
        /// </summary>
        public NoLinealStructures.Structures.Heap<PatientModel> Heap_PE_C = new NoLinealStructures.Structures.Heap<PatientModel>();

        /// <summary>
        /// Heap donde se guardan los pacientes sospechosos asignados al hospital del departamento de Petén.
        /// </summary>
        public NoLinealStructures.Structures.Heap<PatientModel> Heap_PE_S = new NoLinealStructures.Structures.Heap<PatientModel>();

    }
}