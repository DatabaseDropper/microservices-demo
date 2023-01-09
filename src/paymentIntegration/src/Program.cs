using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/check_credit_card", async (HttpContext context) =>
{
    var random = new Random().Next(1, 3);

    if (random == 1)
    {
        context.Response.StatusCode = (int)HttpStatusCode.OK;
        return "OK";
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.PaymentRequired;
        return "ERROR";
    }
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
