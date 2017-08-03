using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Myriad.Validators
{
    sealed public class DateRangeValidator : ValidationAttribute
    {
        public DateTime FirstDate { get; set; }
        public DateTime SecondDate { get; set; }
        public DateRangeValidator()
        {
            FirstDate = DateTime.Now.AddYears(-90); ;
            SecondDate = DateTime.Now.AddYears(-14);
        }
        

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime cvalue = Convert.ToDateTime(value);
            // your validation logic
            if (cvalue >= FirstDate && cvalue <= SecondDate)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Must be of age between 14 to 90");
            }
        }
    }
}