﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Validators;

namespace MarsRover.Api.Application.Modules.Infrastructure.Validations
{
    public class GuidValidator : PropertyValidator
    {
        public GuidValidator() : base("Property {PropertyName} has invalid Guid format")
        {
        }
        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = context.PropertyValue.ToString();
            Guid guid;
            return Guid.TryParse(value, out guid);
        }
    }
}
