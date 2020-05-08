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

        ///<!--MANEJO DEL CSV-------------------------------------------------------------------->

        ///<summary>
        ///Validador primera carga de data.cvs
        ///</summary>
        public bool first_load = true;

        ///<!--ESTADÍSTICAS---------------------------------------------------------------------->

        /// <summary>
        /// Cantidades numércias del estatus de todos los pacientes para el uso en las estadísticas
        /// </summary>
        public int statsRecuperados = 0;
        public int statsConfirmados = 0;
        public int statsSospechosos = 0;

        /// <summary>
        /// Cantidades numércias del estatus de todos los pacientes durante la simulación para el uso en las estadísticas
        /// </summary>
        public int statsSimulationRecuperados = 0;
        public int statsSimulationConfirmados = 0;
        public int statsSimulationSospechosos = 0;

        ///<!--ESTRUCTURAS-------------------------------------------------------------------->

        /// <summary>
        /// Dictionary artesanal de pacientes, sirve para guardar los datos del hospital
        /// </summary>
        public NoLinealStructures.Structures.HashTable<PatientModel> Hashfinal = new NoLinealStructures.Structures.HashTable<PatientModel>();

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