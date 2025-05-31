using Finance.Shared.Models.MstUser;
using FluentValidation;

namespace Finance.Validation.MstUser
{
    public class MstUserUpdateInforModelValidator : AbstractValidator<MstUserUpdateInforModel>
    {
        public MstUserUpdateInforModelValidator()
        {
            RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(50).WithMessage("Full name must be less than 50 characters.");
        }
    }
}
