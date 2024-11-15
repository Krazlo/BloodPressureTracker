using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Entities
{
    public class Measurement
    {
        [Key]
        public int ID { get; set; }

        public DateTime DateTime { get; set; }

        public int Systolic { get; set; }

        public int Diastolic { get; set; }

        public bool Seen { get; set; }

        [ForeignKey("Patient")]
        [StringLength(10)]
        public string PatientSSN { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
