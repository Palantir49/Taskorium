using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Domain.ValueObjects
{
    public class EmailAdress
    {
        public string Value { get; private set; }
        public EmailAdress(string value)
        {
            IsValidEmail(value);
            Value = value;
        }
        private void IsValidEmail(string? value)
        {
            //TODO: Email validation
            if (value is null)
                throw new ArgumentNullException(nameof(value), "Email не может быть null");

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Email не может быть пустым или состоять только из пробелов",
                nameof(value));
        }
        public override string ToString() => Value;
    }
}
