using DoctorApp.Models.DTOs;
using FluentValidation;
using FluentValidation.Results;

namespace DoctorApp.Models.Validators
{
    public static class LoginValidator
    {
        public static ValidationResult Validate(LoginDTO login)
        {
            var validator = new InlineValidator<LoginDTO>();
            validator.RuleFor(x => x.Contraseña).NotEmpty().WithMessage("La contraseña no puede estar vacía");
            validator.RuleFor(x => x.Usuario).NotEmpty().WithMessage("El nombre de usuario no puede estar vacío");

            return validator.Validate(login);

        }
    }
}
