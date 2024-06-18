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
    public static class LoginValidator
    {
        public static ValidationResult Validate (LoginDTO login)
        {
            var validator = new InlineValidator<LoginDTO>();
            validator.RuleFor(x => x.Contraseña).NotEmpty().WithMessage("La contraseña no puede estar vacía");
            validator.RuleFor(x => x.Usuario).NotEmpty().WithMessage("El nombre de usuario no puede estar vacío");

            return validator.Validate(login);

        }
    }
}
