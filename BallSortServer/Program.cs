using BallSortServer.Services;
using WebSocket.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<StateService>();
builder.Services.AddSingleton<IStateReader>(x => x.GetRequiredService<StateService>());
builder.Services.AddSingleton<IStateUpdater>(x => x.GetRequiredService<StateService>());
builder.Services.AddSingleton<ISubscriptionsMgr>(x => x.GetRequiredService<StateService>());
builder.Services.AddControllersWithViews();

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

app.UseWebSockets();
app.UseWebSocketMiddleware();

app.Run();
