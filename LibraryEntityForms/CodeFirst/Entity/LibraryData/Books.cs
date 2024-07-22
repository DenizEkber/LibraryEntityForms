using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntityForms.CodeFirst.Entity.LibraryData
{
    public class Books
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Pages { get; set; }
        public int YearPress { get; set; }
        public int Id_Themes { get; set; }
        public int Id_Category { get; set; }
        public int Id_Author { get; set; }
        public int Id_Press { get; set; }
        public string Comment { get; set; }
        public int Quantity { get; set; }
        public byte[] PhotoData { get; set; }

        public Authors Authors { get; set; }
        public Themes Themes { get; set; }
        public Press Press { get; set; }
        public Categories Categories { get; set; }

        public virtual ICollection<S_Cards> S_Cards { get; set; }
        public virtual ICollection<T_Cards> T_Cards { get; set; }
    }
}
