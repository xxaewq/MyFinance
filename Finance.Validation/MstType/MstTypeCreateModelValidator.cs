using Finance.Repository.SqlServer;
using Finance.Shared.Models.MstType;
using FluentValidation;
using Microsoft.Data.SqlClient;

namespace Finance.Validation.MstType;

public class MstTypeCreateModelValidator : AbstractValidator<MstTypeCreateModel>
{
    private readonly SqlServerDatabaseHelper _helper;
    public MstTypeCreateModelValidator(SqlServerDatabaseHelper helper)
    {
        _helper = helper;

        RuleFor(x => x.TypeName)
            .NotEmpty().WithMessage("Type name is required.")
            .MaximumLength(30).WithMessage("Type name must be less than 30 characters.")
            .MustAsync(BeUniqueNameAsync).WithMessage("Type name must be unique.");
        RuleFor(x => x.Description)
            .MaximumLength(50).WithMessage("Description must be less than 50 characters.");
    }

    private async Task<bool> BeUniqueNameAsync(string typeName, CancellationToken token)
    {
        string sql = "SELECT TOP 1 1 FROM MstType WHERE TypeName = @TypeName";
        var parameters = new[]
        {
            new SqlParameter("@TypeName", typeName)
        };
        int result = await _helper.ExecuteScalarAsync<int>(sql,token, parameters);
        return result == 0;
    }
}
