var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod().SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
        });
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(r => true));
app.UseAuthorization();
app.MapControllers();

app.Run();
