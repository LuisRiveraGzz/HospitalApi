﻿using FluentValidation;
using HospitalApi.Models.DTOs;

namespace HospitalApi.Models.Validators
{
    public class SalaDTOValidator : AbstractValidator<SalaDTO>
    {
        public SalaDTOValidator()
        {
            RuleFor(x => x.NumeroSala).NotEmpty().NotNull().WithMessage("Ingresa el numero de la sala");
            RuleFor(x => x.Doctor).NotNull().NotEmpty().WithMessage("Ingresa el doctor a la sala");
        }
    }
}
