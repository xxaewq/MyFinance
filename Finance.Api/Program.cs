using Finance.Api.Extension;
using Finance.Repository.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped(provider =>
        new SqlServerDatabaseHelper(builder.Configuration.GetConnectionString("FinanceDb") ?? throw new Exception ("Finance Db connection string is not found")));

builder.Services.AddDependencyInjectionServiceForRepository();

builder.Services.AddDependencyInjectionServiceForValidation();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();