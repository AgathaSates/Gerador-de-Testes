using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gerador_de_Testes.WebApp.ActionFilters;

public class LogarAcaoAttribute : ActionFilterAttribute
{
    private readonly ILogger<LogarAcaoAttribute> _logger;

    public LogarAcaoAttribute(ILogger<LogarAcaoAttribute> logger)
    {
        _logger = logger;
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var result = context.Result;

        if (result is ViewResult viewResult && viewResult.Model is not null)
        {
            _logger.LogInformation(
             "\n==========================================================\n" +
             "    !!!!  ACAO DE ENDPOINT EXECUTADA COM SUCESSO  !!!!\n" +
             "----------------------------------------------------------\n" +
             ">>> MODELO RETORNADO:\n>>> {@Modelo}\n" +
             "==========================================================\n", viewResult.Model);
        }

        base.OnActionExecuted(context);
    }
}