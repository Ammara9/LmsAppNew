using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.Validation
{
   public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                // Check if the date is not DateTime.MinValue and is in the future
                return date != DateTime.MinValue && date > DateTime.Now;
            }
            return false;
        }
    }
}
