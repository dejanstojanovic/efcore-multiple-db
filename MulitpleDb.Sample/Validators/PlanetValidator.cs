using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MulitpleDb.Sample.Data;
using System;
using System.Linq;

namespace MulitpleDb.Sample.Validators
{
    public class PlanetValidator : AbstractValidator<String>
    {
        public PlanetValidator(Database1Context database1Context)
        {
            When(p => !String.IsNullOrEmpty(p), () =>
             {
                 RuleFor(p => p).Custom((value, context) =>
                 {
                     if (!database1Context.Planets.AsNoTracking().Any(p => p.Name == value))
                     {
                         context.AddFailure($"Planet {value} does not exist in solar system");
                     }
                 });
             });
        }
    }
}
