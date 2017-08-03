using Myriad.Validators;
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
        //public const string today = DateTime.Today.ToString(); 
        public int ProID { get; set; }

        [DisplayName("Name"), Required]
        [DataType(DataType.Text, ErrorMessage = "Movie Name should be a text")]
        [StringLength(50, ErrorMessage = "Movie Name must not be more than 50 char")]
        [RegularExpression("^([a-zA-Z ]+)$", ErrorMessage = "Producer Name can be alphabet only")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Gender is Required")]
        public int? Sex { get; set; }

        //[Range(typeof(DateTime), "01/10/1930", today, ErrorMessage = "Producer must be of age between 5 to 85")]
        [DateRangeValidator(ErrorMessage = "Producer must be of age between 5 to 60")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Birthday"), DataType(DataType.Date, ErrorMessage = "Birthday should be date")]
        public Nullable<System.DateTime> DOB { get; set; }
        [DisplayName("About")]
        public string Bio { get; set; }
    }
}