using DoctorApp.Models.DTOs;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApp.Models.Validators
{
    public  class SalaValidator
    {
        public  ValidationResult Validate(SalaDTO dTO)
        {
            var validator = new InlineValidator<SalaDTO>();
            validator.RuleFor(x => x.NumeroSala).NotEmpty().WithMessage("El número de la sala no puede estar vacío");
            validator.RuleFor(x => x.Doctor).NotEmpty().WithMessage("El doctor no puede estar vacío");
            return validator.Validate(dTO);
        }
    }
}
