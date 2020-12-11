using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MulitpleDb.Sample.Data;
using MulitpleDb.Sample.Models;
using System;
using System.Linq;

namespace MulitpleDb.Sample.Validators
{
    public class RocketQueryModelValidator : AbstractValidator<RocketQueryModel>
    {
        public RocketQueryModelValidator(Database1Context database1Context)
        {
            RuleFor(q => q.Planet).NotEmpty().NotNull().WithMessage("Planet name is required");
            When(q => !String.IsNullOrEmpty(q.Planet), () =>
             {
                 RuleFor(p => p).Custom((value, context) =>
                 {
                     if (!database1Context.Planets.AsNoTracking().Any(p => p.Name == value.Planet))
                     {
                         context.AddFailure($"Planet {value.Planet} does not exist in solar system");
                     }
                 });
             });
        }
    }
}
