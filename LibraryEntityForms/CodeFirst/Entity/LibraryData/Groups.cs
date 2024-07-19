using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntityForms.CodeFirst.Entity.LibraryData
{
    public class Groups
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Id_Faculty { get; set; }

        public virtual ICollection<Students> Students { get; set; }
        public Faculties Faculties { get; set; }
    }
}
