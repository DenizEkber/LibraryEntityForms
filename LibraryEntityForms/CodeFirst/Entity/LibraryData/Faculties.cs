using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntityForms.CodeFirst.Entity.LibraryData
{
    public class Faculties
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Groups> Groups { get; set; }
    }
}
