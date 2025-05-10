global using Microsoft.EntityFrameworkCore;
global using System;
global using System.Collections.Generic;
global using Microsoft.EntityFrameworkCore.Sqlite;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using warsztaty_blazor.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton(Database.GetInstance());

builder.Services.AddDbContextFactory<ClinicContext>(options =>
    options.UseSqlite("Data Source=clinic.sqlite"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

// builder.Services.AddQuickGridEntityFrameworkAdapter();
// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

app.Run();