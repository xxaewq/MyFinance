using Finance.Repository.Abstraction;
using Finance.Repository.SqlServer;
using Finance.Shared.Models.MstType;
using Finance.Validation;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient(provider =>
        new SqlServerDatabaseHelper(builder.Configuration.GetConnectionString("FinanceDb") ?? throw new Exception ("Finance Db connection string is not found")));

builder.Services.AddScoped<IValidator<MstTypeCreateModel>, TypeCreateModelValidator>();
builder.Services.AddScoped<IValidator<MstTypeUpdateModel>, TypeUpdateModelValidator>();


builder.Services.AddScoped<ITypeRepository, SqlServerTypeRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();