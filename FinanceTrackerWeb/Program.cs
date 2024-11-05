using Microsoft.EntityFrameworkCore;
using FinanceTrackerWeb.Data;
using Microsoft.AspNetCore.Identity;
using FinanceTrackerWeb.Models;
using FinanceTrackerWeb.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using FinanceTrackerWeb.Contracts;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();


builder.Services.AddDbContext<FinanceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FinanceContext")));

//RequireConfirmedAccount is for email confirmed accounts - set to false for development
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<FinanceContext>();

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<ISpendingService, SpendingService>();
builder.Services.Configure<PlaidSettings>(builder.Configuration.GetSection("PlaidSettings"));
builder.Services.AddHttpClient<PlaidService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7190/");
});
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapRazorPages();

app.Run();