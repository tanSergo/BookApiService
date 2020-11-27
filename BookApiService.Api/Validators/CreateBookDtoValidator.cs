using BookApiService.Api.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookApiService.Api.Validators
{
    public class CreateBookDtoValidator : AbstractValidator<CreateBookDto>
    {
        public CreateBookDtoValidator()
        {
            RuleFor(b => b.Author)
                .Must(x =>
                x == null ? true : !(Regex.Match(x.Trim(), @"[\s-!$%^&*()_+@#№|~=`{}\[\]:"";'<>?,.\/\\]*").Value.Trim().Length == x.Trim().Length && x.Trim().Length > 0)
                ).WithMessage(ValidationErrors.AuthorNameInvalid)
                .NotEmpty().WithMessage(ValidationErrors.AuthorNameRequired)
                .MaximumLength(100);
            RuleFor(b => b.Title)
                .Must(x =>
                x == null ? true : !(Regex.Match(x.Trim(), @"[\s-!$%^&*()_+@#№|~=`{}\[\]:"";'<>?,.\/\\]*").Value.Trim().Length == x.Trim().Length && x.Trim().Length > 0)
                ).WithMessage(ValidationErrors.TitleInvalid)
                .NotEmpty().WithMessage(ValidationErrors.TitleRequired)
                .MaximumLength(100);
            RuleFor(b => b.PublicationYear)
                .LessThanOrEqualTo(DateTime.Now.Year)
                .WithMessage("Publication year must not be greater then current year");
        }
    }
}
