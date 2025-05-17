using Finance.Repository.SqlServer;
using Finance.Shared.Models.MstType;
using FluentValidation;
using Microsoft.Data.SqlClient;

namespace Finance.Validation.MstType;

public class MstTypeUpdateModelValidator : AbstractValidator<MstTypeUpdateModel>
{
    private readonly SqlServerDatabaseHelper _helper;
    public MstTypeUpdateModelValidator(SqlServerDatabaseHelper helper)
    {
        _helper = helper;

        RuleFor(x => x.TypeName)
            .NotEmpty().WithMessage("Type name is required.")
            .MaximumLength(30).WithMessage("Type name must be less than 30 characters.");
        RuleFor(x => x.Description)
            .MaximumLength(50).WithMessage("Description must be less than 50 characters.");

        RuleFor(x => new { x.TypeName, x.Description })
            .MustAsync((x, cancellation) => BeUniqueNameAsync(x.TypeName, x.Description, cancellation))
            .WithMessage("Type name and description combination must be unique.");
    }

    private async Task<bool> BeUniqueNameAsync(string typeName, string? description, CancellationToken token)
    {
        string sql = "SELECT TOP 1 1 FROM MstType WHERE TypeName = @TypeName AND Description = @Description";
        var parameters = new[]
        {
           new SqlParameter("@TypeName", typeName),
           new SqlParameter("@Description", description)
        };
        int result = await _helper.ExecuteScalar<int>(sql, token, parameters);
        return result == 0;
    }
}
