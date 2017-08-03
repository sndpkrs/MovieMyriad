using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Myriad.Models;
using System.ComponentModel.DataAnnotations;

namespace Myriad.Models
{
    public class MovieViewModel
    {
        MyriadDbEntities db = new MyriadDbEntities();
        

        [Key]
        public int MovID { get; set; }

        [DisplayName("Movie"), Required]
        [DataType(DataType.Text, ErrorMessage = "Movie Name should be a text")]
        [StringLength(50, ErrorMessage = "Movie Name must not be more than 50 char")]
        [RegularExpression("^([a-zA-Z0-9 ]+)$", ErrorMessage = "Movie Name can be alphanumeric only")]
        public string Name { get; set; }

        [DisplayName("Date of Release"), Required, DataType(DataType.Date, ErrorMessage = "Release Date should be a date")]
        public Nullable<System.DateTime> ReleaseDate { get; set; }

        [StringLength(200, ErrorMessage = "Movie Plot must not be more than 200 char")]
        public string Plot { get; set; }


        public string Poster { get; set; }

        [DisplayName("Producer")]

        public Nullable<int> ProID { get; set; }

        [DisplayName("Producer")]
        public Producer Producer { get; set; }
        [DisplayName("Actors")]
        public List<CheckActorsModel> ActorsList { get; set; }
    }
}