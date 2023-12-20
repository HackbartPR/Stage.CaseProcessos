using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using static Stage.Domain.Config.Constants;

namespace Stage.Application.Services.Processos.Command.AddProcesso
{
    public class AddProcessoCommandValidator : AbstractValidator<AddProcessoCommand>
    {
        public AddProcessoCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .Must(name => !name.IsNullOrEmpty())
                    .WithMessage(ValidationsMessages.NameField);

            RuleFor(p => p.IdParentProccess)
                .Must(id => id == null || id > 0)
                    .WithMessage(ValidationsMessages.IdParent);

            RuleFor(p => p.IdsFerramentas)
                .Must(ids => !ids.Any(id => id <= 0))
                    .WithMessage(ValidationsMessages.IdFerramentaField);

            RuleFor(u => u.IdsAreas)
                .Must(ids => !ids.Any(id => id <= 0))
                    .WithMessage(ValidationsMessages.IdAreaField);

            RuleForEach(p => p.SubProcessos)
                .SetValidator(this);
        }
    }
}
