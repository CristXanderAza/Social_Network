using Social_Network.Core.Application;
using Social_Network.Infraestructure.Identity;
using Social_Network.Infraestructure.Persistence;
using Social_Network.Infraestructure.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services
    .AddIdentityServiceForWebApp(builder.Configuration)
    .AddSharedServices(builder.Configuration)
    .AddApplicationService(builder.Configuration)
    .AddPersistenceService(builder.Configuration);


var app = builder.Build();
await app.Services.RunIdentitySeedAsyn();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
