using AdminApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddScoped<AutenticaciónService>();
builder.Services.AddScoped<DoctoresService>();
builder.Services.AddScoped<SalasService>();
builder.Services.AddScoped<TokenService>();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/" || context.Request.Path == "")
    {
        context.Response.Redirect("/Autenticaci%C3%B3n/Login");
        return;
    }
    await next();
});
app.MapControllerRoute(
     name: "default",
     pattern: "{controller=Autenticación}/{action=Login}"
);


app.Run();
