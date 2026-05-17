using BookRight.Web.Components;
using BookRight.Web.DependencyInjections;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Database
builder.Services.AddDatabaseDI(builder.Configuration);

// Domain (Domain Services
builder.Services.AddDomainDI();

// Repositories (Use Case-interfaces → Infrastructure-implementeringer)
builder.Services.AddRepositoryDI();

// Handlers (Facade-interfaces → Use Case-implementeringer)
builder.Services.AddHandlerDI();

// Discount strategies
builder.Services.AddDiscountStrategyDI();

// Queries (Facade-interfaces → Infrastructure-implementeringer)
builder.Services.AddQueriesDI();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
