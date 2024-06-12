using FluentValidation;
using HospitalApi.Models.DTOs.Sala;

namespace HospitalApi.Models.Validators
{
    public class SalaDTOValidator : AbstractValidator<SalaDTO>
    {
        public SalaDTOValidator()
        {
            RuleFor(x => x.NumeroSala).NotEmpty().NotNull().WithMessage("Ingresa el numero de la sala");
        }
    }
}
