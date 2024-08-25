using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class ValidationHelper
    {
        internal static void ModelValidation(object model)
        {
            ValidationContext validationContext = new ValidationContext(model);
            var listOfVal = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(model, validationContext, listOfVal, true);
            if (!isValid)
                throw new ArgumentException(listOfVal.FirstOrDefault()?.ErrorMessage);
        }
    }
}
