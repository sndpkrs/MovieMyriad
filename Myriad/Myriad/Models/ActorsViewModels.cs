using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Myriad.Models
{
    public class ActorsViewModels
    {

        public ActorsViewModels()
        {
            
        }
        public int ActID { get; set; }

        [Required(ErrorMessage = "Actor Name is Required")]
        [DisplayName("Name")]
        [DataType(DataType.Text, ErrorMessage = "Actor Name should be a text")]
        [StringLength(50, ErrorMessage = "Actor Name must not be more than 50 char")]
        [RegularExpression("^([a-zA-Z ]+)$", ErrorMessage = "Actor Name can be alphabet only")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Gender is Required")]
        public int Sex { get; set; }

        [DisplayName("Birthday"), DataType(DataType.Date, ErrorMessage = "Birthday should be date")]
        [Range(typeof(DateTime), "01/10/1930", "01/01/2012", ErrorMessage = "Actor must be of age between 5 to 85")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DOB { get; set; }
        [DisplayName("About")]
        public string Bio { get; set; }


        IEnumerable<SelectListItem> glist { get; set; }
        public virtual ICollection<MovieActor> MovieActors { get; set; }
    }
}