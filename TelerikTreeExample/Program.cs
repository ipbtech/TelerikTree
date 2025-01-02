using Microsoft.AspNetCore.Localization;
using TelerikTreeExample.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(opt =>
    {
        // turn off standart serialization for property names because telerik doesn't match camelCase
        opt.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// add localization because I ran into with telerik problem when it doesn't match double fields
builder.Services.Configure<RequestLocalizationOptions>(opt =>
{
    opt.DefaultRequestCulture = new RequestCulture("en-US");
});

builder.Services.AddKendo(); // telerik & kendo
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "TreeList.Content";
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HttpSessionStorage>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler("/Home/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseRequestLocalization(); // add localization because I ran into with telerik problem when it doesn't match double fields
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
