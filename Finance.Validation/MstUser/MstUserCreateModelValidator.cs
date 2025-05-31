using Finance.Repository.SqlServer;
using Finance.Shared.Models.MstUser;
using FluentValidation;
using Microsoft.Data.SqlClient;

namespace Finance.Validation.MstUser;

public class MstUserCreateModelValidator : AbstractValidator<MstUserCreateModel>
{
    private readonly SqlServerDatabaseHelper _helper;
    public MstUserCreateModelValidator(SqlServerDatabaseHelper helper)
    {
        _helper = helper;


        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("User name is required.")
            .MaximumLength(30).WithMessage("User name must be less 30 characters.")
            .MustAsync(BeUniqueUserNameAsync).WithMessage("User name must be unique.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(50).WithMessage("Full name must be less than 50 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 6 characters long.")
            .Must(BeValidPassword).WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
    }

    private bool BeValidPassword(string password)
    {
        bool hasUpper = password.Any(char.IsUpper);
        bool hasLower = password.Any(char.IsLower);
        bool hasDigit = password.Any(char.IsDigit);
        bool hasSpecial = password.Any("!@#$%^&*?".Contains);
        return hasUpper && hasLower && hasDigit && hasSpecial;
    }

    private async Task<bool> BeUniqueUserNameAsync(string username, CancellationToken token)
    {
        string sql = "SELECT TOP 1 1 FROM MstUser WHERE UserName = @UserName";
        var parameters = new SqlParameter[]
        {
            new("@UserName", username)
        };
        int result = await _helper.ExecuteScalarAsync<int>(sql, token, parameters);
        return result == 0;
    }
}
