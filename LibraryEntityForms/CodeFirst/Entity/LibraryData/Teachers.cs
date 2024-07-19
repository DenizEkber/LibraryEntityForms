using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntityForms.CodeFirst.Entity.LibraryData
{
    public class Teachers
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Id_Dep { get; set; }

        public virtual ICollection<T_Cards> T_Cards { get; set; }
        public Departments Departments { get; set; }
    }
}
