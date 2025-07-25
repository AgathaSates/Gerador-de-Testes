﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Gerador_de_Testes.WebApp.ActionFilters;

public class ValidarModeloAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.Controller is not Controller controller)
            return;

        ModelStateDictionary modelState = context.ModelState;

        object? viewModel = context.ActionArguments.Values
            .FirstOrDefault(
            x => x?.GetType().Name.EndsWith("ViewModel") == true);

        if (!modelState.IsValid && viewModel != null)
            context.HttpContext.Items["ModelStateInvalid"] = true;
    }
}