using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Service_Abstraction.Interfaces;
using System.Text;

namespace Presentiton.Attribute
{
    public class CashAttribute(int DurationinSec = 90) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //next => Attribute or New EndPoint
            //1 - CashKey By URL
            string CashKey = CreateCashKey(context.HttpContext.Request);
            //2- Check CashValue Found Or Not

            ICashService CashService = context.HttpContext.RequestServices.GetRequiredService<ICashService>();
            var CashValue = await CashService.GetAsync(CashKey);


            if (CashValue is not null)
            {
                //3- If CashValue Found (is not null) Return Value
                context.Result = new ContentResult()
                {
                    Content = CashValue,
                    ContentType = "application/Json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            else
            {
                //If CashValue Not Found (Cashey Is Null) ==> Invoke => CashData
                var Excetue = await next.Invoke();
                if (Excetue.Result is OkObjectResult result)
                {
                    await CashService.SetAsync(CashKey, result.Value!, TimeSpan.FromSeconds(DurationinSec));
                }
            }
        }

        private string CreateCashKey(HttpRequest request)
        {
            StringBuilder CashKey = new StringBuilder();
            CashKey.Append(request.Path + '?');

            foreach (var item in request.Query.OrderBy(Q => Q.Key))
            {
                CashKey.Append($"{item.Key}={item.Value}&");
            }

            return CashKey.ToString();
        }
    }
}
