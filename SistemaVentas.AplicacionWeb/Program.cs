using SistemaVentas.AplicacionWeb.Utilidades.AutoMapper;
using SistemaVentas.IOC;

using SistemaVentas.AplicacionWeb.Utilidades.Extensiones;
//using DinkToPdf;
//using DinkToPdf.Contracts;
//using Wkhtmltopdf.NetCore;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.InyectarDependencia(builder.Configuration);
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

//var context = new CustomAssemblyLoadContext();
//context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(),"Utilidades/LibreriaPDF/libwkhtmltox.dll"));
//builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

//builder.Services.AddHttpClient();

//builder.Services.AddWkhtmltopdf();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseRotativa();

app.Run();
