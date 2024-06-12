using FluentValidation;
using HospitalApi.Models.DTOs;

namespace HospitalApi.Models.Validators
{
    public class UsuarioDTOValidator : AbstractValidator<UsuarioDTO>
    {
        public UsuarioDTOValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().NotNull().WithMessage("Ingresa un nombre");
            RuleFor(x => x.Contraseña).NotEmpty().NotNull().WithMessage("Ingresa una contraseña");
        }
    }
}
