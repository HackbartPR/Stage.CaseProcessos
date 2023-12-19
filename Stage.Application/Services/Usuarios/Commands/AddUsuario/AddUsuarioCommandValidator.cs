using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using static Stage.Domain.Config.Constants;

namespace Stage.Application.Services.Usuarios.Commands.AddUsuario
{
    public class AddUsuarioCommandValidator : AbstractValidator<AddUsuarioCommand>
    {
        public AddUsuarioCommandValidator()
        {
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
