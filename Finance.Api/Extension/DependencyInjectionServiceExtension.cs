using Finance.Repository.Abstraction;
using Finance.Repository.SqlServer;
using Finance.Shared.Models.MstApp;
using Finance.Shared.Models.MstType;
using Finance.Shared.Models.MstUser;
using Finance.Validation.MstApp;
using Finance.Validation.MstType;
using Finance.Validation.MstUser;
using FluentValidation;

namespace Finance.Api.Extension
{
    public static class DependencyInjectionServiceExtension
    {
        public static IServiceCollection AddDependencyInjectionServiceForRepository(this IServiceCollection services)
        {
            services.AddScoped<IMstTypeRepository, SqlServerMstTypeRepository>();
            services.AddScoped<IMstAppRepository, SqlServerMstAppRepository>();
            services.AddScoped<IMstUserRepository, SqlServerMstUserRepository>();
            services.AddScoped<IUserBalanceRepository, SqlServerUserBalanceRepository>();

            return services;
        }
        public static IServiceCollection AddDependencyInjectionServiceForValidation(this IServiceCollection services)
        {
            services.AddScoped<IValidator<MstTypeCreateModel>, MstTypeCreateModelValidator>();
            services.AddScoped<IValidator<MstTypeUpdateModel>, MstTypeUpdateModelValidator>();

            services.AddScoped<IValidator<MstAppCreateModel>, MstAppCreateModelValidator>();
            services.AddScoped<IValidator<MstAppUpdateModel>, MstAppUpdateModelValidator>();

            services.AddScoped<IValidator<MstUserCreateModel>, MstUserCreateModelValidator>();
            services.AddScoped<IValidator<MstUserUpdateInforModel>, MstUserUpdateInforModelValidator>();

            return services;
        }
    }
}
