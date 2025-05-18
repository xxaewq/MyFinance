using Finance.Repository.SqlServer;
using Finance.Shared.Models.MstApp;
using FluentValidation;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Validation.MstApp;
public class MstAppCreateModelValidator : AbstractValidator<MstAppCreateModel>
{
    private readonly SqlServerDatabaseHelper _helper;
    public MstAppCreateModelValidator(SqlServerDatabaseHelper helper)
    {
        _helper = helper;

        RuleFor(x => x.TypeApp)
                .NotEmpty().WithMessage("TypeApp is required.")
                .MaximumLength(20).WithMessage("TypeApp must be less than 30 characters.");
        RuleFor(x => x.NameApp)
                .NotEmpty().WithMessage("NameApp is required.")
                .MaximumLength(30).WithMessage("NameApp must be less than 30 characters.")
                .MustAsync(BeUniqueNameAsync).WithMessage("NameApp must be unique.");
        RuleFor(x => x.Description)
                .MaximumLength(50).WithMessage("Description must be less than 50 characters.");
    }

    private async Task<bool> BeUniqueNameAsync(string nameApp, CancellationToken token)
    {
        string sql = "SELECT TOP 1 1 FROM MstApp WHERE NameApp = @NameApp";
        var parameters = new SqlParameter[]
        {
            new("@NameApp", nameApp)
        };
        int result = await _helper.ExecuteScalarAsync<int>(sql, token, parameters);
        return result == 0;
    }
}
