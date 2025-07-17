using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gerador_de_Testes.ActionFilters;

public class ValidarModeloAttribute : ActionFilterAttribute
{   // Lógica ANTES da execução de cada método Action
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var modelState = context.ModelState;

        if (!modelState.IsValid)
        {
            var controller = (Controller)context.Controller;

            var viewModel = context.ActionArguments
                .Values
                .FirstOrDefault(x => x.GetType().Name.EndsWith("ViewModel"));

            context.Result = controller.View(viewModel);
        }
    }
}