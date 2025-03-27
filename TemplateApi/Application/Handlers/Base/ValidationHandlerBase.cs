using Application.DTO.Base;
using FluentValidation;

namespace Application.Handlers.Base
{
    public class ValidationHandlerBase<T> : AbstractValidator<T> where T: PagingBase
    {
        public ValidationHandlerBase()
        {
            RuleLevelCascadeMode = CascadeMode.Continue;

            RuleFor(entity => entity.Page)
                .GreaterThan(0)
                .WithMessage("Numero da página deve ser maior que 0")
                .NotNull()
                .WithMessage("Numero da página não deve ser nulo")
                .NotEmpty()
                .WithMessage("Numero da página não pode ser vazio");

            RuleFor(entity => entity.PageSize)
                .GreaterThan(0)
                .WithMessage("Tamanho da página deve ser maior que 0")
                .NotNull()
                .WithMessage("Tamanho da página não deve ser nulo")
                .NotEmpty()
                .WithMessage("Tamanho da página não pode ser vazio");
        }
    }
}
