using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Domain.ValueObjects
{
    public record BaseEntityName
    {
        public string Value { get; private set; }

        public BaseEntityName(string value)
        {
            value = value.Trim();
            IsValidName(value);
            Value = value;
        }

        private void IsValidName(string? value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value), "Наименование задачи не может быть null");

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Наименование задачи не может быть пустым или состоять только из пробелов",
                nameof(value));
        }

        public override string ToString() => Value;
    }
}
