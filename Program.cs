using EmailSignatureApp.Models;
using EmailSignatureApp.Services;
using Microsoft.AspNetCore.Authentication.Negotiate;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Windows Authentication
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

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