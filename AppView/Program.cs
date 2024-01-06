using AppView.Controllers;
using AppView.Hubs;
using AppView.Middleware;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromDays(30);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
app.UseStatusCodePagesWithRedirects("/Home/NotFound");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseMiddleware<InactiveCartMiddleware>();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<ConnecttedHub>("/ConnectedHub");
app.Run();
//hii