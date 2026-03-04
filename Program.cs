using EmailSignatureApp.Models;
using EmailSignatureApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// No IIS, o Windows Authentication é gerenciado pelo próprio servidor.
// Não usamos AddNegotiate() aqui — apenas habilitamos IISIntegration.
builder.Services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

// Configurações
builder.Services.Configure<EmailSignatureApp.Models.LdapSettings>(
    builder.Configuration.GetSection("LdapSettings")
);
builder.Services.Configure<EmailSignatureApp.Models.SignatureSettings>(
    builder.Configuration.GetSection("SignatureSettings")
);

// Serviços
builder.Services.AddScoped<AdService>();
builder.Services.AddScoped<SignatureService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();