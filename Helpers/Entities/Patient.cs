using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Entities
{
    public class Patient
    {
        [Key]
        [StringLength(10)]
        public string SSN { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
    }
}
