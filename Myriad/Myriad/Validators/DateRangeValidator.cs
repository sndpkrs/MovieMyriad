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
            FirstDate = Convert.ToDateTime("01/01/1950");
            SecondDate = DateTime.Today;
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
                return new ValidationResult("Must be of age between 5 to 60");
            }
        }
    }
}