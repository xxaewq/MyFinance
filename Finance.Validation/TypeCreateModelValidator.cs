using Finance.Shared.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Validation
{
    public class TypeCreateModelValidator : AbstractValidator<MstTypeCreateModel>
    {
        public TypeCreateModelValidator()
        {
            RuleFor(x => x.TypeName)
                .NotEmpty().WithMessage("Type name is required.")
                .Length(1, 30).WithMessage("Type name must be between 1 and 30 characters.");
            RuleFor(x => x.Description)
                .MaximumLength(50).WithMessage("Description must be less than 50 characters.");
        }
    }
}
