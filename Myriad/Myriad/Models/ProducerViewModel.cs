using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Myriad.Models
{
    public class ProducerViewModel
    {
        public int ProID { get; set; }
      
        [DisplayName("Name"), Required]
        [DataType(DataType.Text, ErrorMessage = "Movie Name should be a text")]
        [StringLength(50, ErrorMessage = "Movie Name must not be more than 50 char")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Gender is Required")]
        public int? Sex { get; set; }

        [DisplayName("Birthday"), DataType(DataType.Date, ErrorMessage = "Birthday should be date")]
        public Nullable<System.DateTime> DOB { get; set; }
        [DisplayName("About")]
        public string Bio { get; set; }
    }
}