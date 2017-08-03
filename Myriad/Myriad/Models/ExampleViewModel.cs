using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Myriad.Models
{
    public class ExampleViewModel
    {
        [Key]
        public int MovID { get; set; }

        [DisplayName("Movie")]
        public string Name { get; set; }

        [DisplayName("Date of Release")]
        public Nullable<System.DateTime> ReleaseDate { get; set; }
        public string Plot { get; set; }
        public string Poster { get; set; }
        [DisplayName("Producer")]
        public Nullable<int> ProID { get; set; }
        
        public Producer Producer { get; set; }
        [DisplayName("Actors")]
        public IEnumerable<System.Web.Mvc.SelectListItem> SelectedActorsList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> ActorsList { get; set; }

        public IEnumerable<int> SelectedActorsIdList { get; set; }

    }
}