using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pera.DTO.DTOs
{
   
    public class StudentSelectionDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // Later 'Class' or 'SchoolNumber' might be added
    }
}
