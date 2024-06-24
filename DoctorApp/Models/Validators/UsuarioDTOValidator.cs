using DoctorApp.Models.DTOs;
using FluentValidation;

namespace DoctorApp.Models.Validators
{
    public class UsuarioDTOValidator : AbstractValidator<UsuarioDTO>
    {
        public UsuarioDTOValidator()
        {
            RuleFor(x => x.Nombre).NotNull().NotEmpty().WithMessage("Ingresa el nombre del usuario");
            RuleFor(x => x.Contraseña).NotNull().NotEmpty().WithMessage("Ingresa la contraseña del usuario");
        }
    }
}
