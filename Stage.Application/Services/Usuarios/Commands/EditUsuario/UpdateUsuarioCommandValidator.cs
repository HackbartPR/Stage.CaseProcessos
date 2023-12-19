using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using static Stage.Domain.Config.Constants;

namespace Stage.Application.Services.Usuarios.Commands.EditUsuario
{
    public class UpdateUsuarioCommandValidator : AbstractValidator<UpdateUsuarioCommand>
    {
        public UpdateUsuarioCommandValidator()
        {
            RuleFor(u => u.Id)
                .NotEmpty()
                .Must(id => id > 0)
                    .WithMessage(ValidationsMessages.IdField);

            RuleFor(u => u.Name)
                .NotEmpty()
                .Must(name => !name.IsNullOrEmpty())
                    .WithMessage(ValidationsMessages.NameField);

            RuleFor(u => u.IdsAreas)
                .Must(ids => !ids.Any(id => id <= 0))
                    .WithMessage(ValidationsMessages.IdAreaField);
        }
    }
}
