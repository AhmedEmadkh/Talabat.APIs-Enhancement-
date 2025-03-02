using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Repository;
using Talabat.Services;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddScoped(typeof(IBasketRepository),typeof(BasketRepository));
            Services.AddScoped(typeof(IUnitOfWork),typeof(UnitOfWork));
            Services.AddScoped<IProductService, ProductService>();
            Services.AddScoped<IOrderService, OrderService>();
            //Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddAutoMapper(typeof(MappingProfiles));

            #region Error Handling
            Services.Configure<ApiBehaviorOptions>(options =>
                {
                    options.InvalidModelStateResponseFactory = (actionContext) =>
                    {
                        var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                             .SelectMany(P => P.Value.Errors)
                                                             .Select(P => P.ErrorMessage)
                                                             .ToList();

                        var response = new ApiValidationErrorResponse()
                        {
                            Errors = errors
                        };
                        return new BadRequestObjectResult(response);
                    };
                }); 
            #endregion


            return Services;
        }
    }
}
