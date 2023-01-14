using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/payu", async (HttpContext context) =>
{
    var data = await context.Request.ReadFromJsonAsync<dynamic>();
    Console.WriteLine(data);
    context.Response.StatusCode = (int)HttpStatusCode.OK;
    return "OK";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
