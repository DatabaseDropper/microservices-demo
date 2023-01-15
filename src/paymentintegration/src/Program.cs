using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddCors(o =>
{
    o.AddPolicy("cors", b =>
    {
        b.AllowAnyHeader();
        b.AllowAnyMethod();
        b.AllowAnyOrigin();
    });
});

var app = builder.Build();
app.UseCors("cors");

app.MapPost("/payu", async (HttpContext context) =>
{
    var data = await context.Request.ReadFromJsonAsync<dynamic>();
    Console.WriteLine(data);
    context.Response.StatusCode = (int)HttpStatusCode.OK;
    return "{\"status\": \"ok\"}";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
