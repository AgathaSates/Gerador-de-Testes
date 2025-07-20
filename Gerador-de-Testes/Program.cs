using Gerador_de_Testes.Dominio.ModuloDisciplina;
using Gerador_de_Testes.Dominio.ModuloMateria;
using Gerador_de_Testes.Dominio.ModuloQuestao;
using Gerador_de_Testes.Dominio.ModuloTeste;
using Gerador_de_Testes.Infraestrutura.Orm.ModuloDisciplina;
using Gerador_de_Testes.Infraestrutura.Orm.ModuloMateria;
using Gerador_de_Testes.Infraestrutura.Orm.ModuloQuestao;
using Gerador_de_Testes.Infraestrutura.Orm.ModuloTeste;
using Gerador_de_Testes.WebApp.ActionFilters;
using Gerador_de_Testes.WebApp.DependencyInjection;
using Gerador_de_Testes.WebApp.Orm;

namespace Gerador_de_Testes.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<ValidarModeloAttribute>();
                options.Filters.Add<LogarAcaoAttribute>();
            });

            builder.Services.AddScoped<IRepositorioDisciplina, RepositorioDisciplina>();
            builder.Services.AddScoped<IRepositorioMateria, RepositorioMateria>();
            builder.Services.AddScoped<IRepositorioQuestao, RepositorioQuestao>();
            builder.Services.AddScoped<IRepositorioTeste, RepositorioTeste>();

            builder.Services.AddEntityFrameworkConfig(builder.Configuration);
            builder.Services.AddSerilogConfig(builder.Logging);

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/error");
                app.UseStatusCodePagesWithReExecute("/error");
            }
            else
                app.UseDeveloperExceptionPage();

            app.ApplyMigrations();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAntiforgery();
            app.MapDefaultControllerRoute();
            app.Run();
        }
    }
}