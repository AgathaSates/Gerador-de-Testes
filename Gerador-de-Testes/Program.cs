using Gerador_de_Testes.ActionFilters;
using Gerador_de_Testes.DependencyInjection;
using Gerador_de_Testes.Orm;

namespace Gerador_de_Testes
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

            //builder.Services.AddScoped<IRepositorioEntidade, RepositorioEntidadeOrm>(); *Adicionar repositório de cada entidade aqui*

            builder.Services.AddEntityFrameworkConfig(builder.Configuration);
            builder.Services.AddSerilogConfig(builder.Logging);

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
                app.UseExceptionHandler("/Error");
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