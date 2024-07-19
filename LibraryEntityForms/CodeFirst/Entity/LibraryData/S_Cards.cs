using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntityForms.CodeFirst.Entity.LibraryData
{
    public class S_Cards
    {

        public int Id { get; set; }
        public int Id_Student { get; set; }
        public int Id_Book { get; set; }
        public DateTime DataOut { get; set; }
        public DateTime DataIn { get; set; }
        public int Id_Lib { get; set; }
        public int TimeLimit { get; set; }

        public Books Books { get; set; }
        public Students Students { get; set; }
        public Libs Libs { get; set; }

    }
}
