using Finance.Repository.SqlServer;
using Finance.Shared.Models.MstApp;
using FluentValidation;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Validation.MstApp
{
    public class MstAppUpdateModelValidator : AbstractValidator<MstAppUpdateModel>
    {
        private readonly SqlServerDatabaseHelper _helper;

        public MstAppUpdateModelValidator(SqlServerDatabaseHelper helper)
        {
            _helper = helper;

            RuleFor(x => x.TypeApp)
                .NotEmpty().WithMessage("TypeApp is required.")
                .MaximumLength(20).WithMessage("TypeApp must be less than 30 characters.");
            RuleFor(x => x.Description)
                .MaximumLength(50).WithMessage("Description must be less than 50 characters.");
            RuleFor(x => new { x.NameApp, x.Description })
            .MustAsync((x, cancellation) => BeUniqueNameAsync(x.NameApp, x.Description, cancellation))
            .WithMessage("Name app and Description combination must be unique.");
        }

        private async Task<bool> BeUniqueNameAsync(string nameApp, string? description, CancellationToken cancellation)
        {
            string sql = "SELECT TOP 1 1 FROM MstApp WHERE NameApp = @NameApp AND Description = @Description";
            var parameters = new SqlParameter[]
            {
                new("@NameApp", nameApp),
                new("@Description", description)
            };
            int result = await _helper.ExecuteScalarAsync<int>(sql, cancellation, parameters);
            return result == 0;
        }
    }
}
