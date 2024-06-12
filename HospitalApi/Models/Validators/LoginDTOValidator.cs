using FluentValidation;
using HospitalApi.Models.DTOs;

namespace HospitalApi.Models.Validators
{
    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(x => x.Usuario).NotEmpty().NotNull().WithMessage("Ingresa el Nombre del Usuario");
            RuleFor(x => x.Contraseña).NotEmpty().NotNull().WithMessage("Ingresa el Nombre del Usuario");
        }
    }
}
