using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Myriad.Models
{
    public class CreateAll
    {
        public CreateAll()
        {
            this.movieModel = new MovieViewModel();
            this.producer = new Producer();
            this.actor = new Actor();
        }
        public MovieViewModel movieModel { get; set; }
        public Actor actor{ get; set; }
        public Producer producer { get; set; }
    }
}