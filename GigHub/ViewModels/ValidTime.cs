using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace GigHub.ViewModels
{
    public class ValidTime : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dateTime;

            bool isValid = DateTime.TryParseExact(Convert.ToString(value), "H:m", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);

            return isValid;
        }
    }
}