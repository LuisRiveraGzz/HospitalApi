﻿using DoctorApp.Models.DTOs;
using FluentValidation;

namespace DoctorApp.Models.Validators
{
    public class SalaValidator : AbstractValidator<SalaDTO>
    {
        public SalaValidator()
        {
            RuleFor(x => x.NumeroSala).NotEmpty().WithMessage("El número de la sala no puede estar vacío");
            RuleFor(x => x.Doctor).NotEmpty().WithMessage("El doctor no puede estar vacío");
        }
    }
}
