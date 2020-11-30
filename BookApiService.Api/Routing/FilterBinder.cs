using BookApiService.Core.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiService.Api.Routing
{
    public class FilterBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {

            var jsonString = bindingContext.ActionContext.HttpContext.Request.Query["filter"];
            if (!StringValues.IsNullOrEmpty(jsonString))
            {
                Filter[] result = JsonConvert.DeserializeObject<Filter[]>(jsonString);
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Success(new Filter[0]);
            }
            return Task.CompletedTask;
        }
    }
}
