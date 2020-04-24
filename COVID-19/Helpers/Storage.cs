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
        public NoLinealStructures.Structures.Tree<PersonModel> PersonsTree = new NoLinealStructures.Structures.Tree<PersonModel>();

        

    }
}