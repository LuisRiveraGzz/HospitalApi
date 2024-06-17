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
            RuleFor(x => x.Rol).Must(IsRol).WithMessage("Seleccione un rol");
        }

        private bool IsRol(sbyte rol) => rol == 1 || rol == 2;
    }
}
