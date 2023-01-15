using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();
app.UseCors(x => x
          .AllowAnyMethod()
          .AllowAnyHeader()
          .SetIsOriginAllowed(origin => true) // allow any origin
                                              //.WithOrigins("https://localhost:44351")); // Allow only this origin can also have multiple origins separated with comma
          .AllowCredentials()); // allow credentials

app.MapGet("/payu", async (HttpContext context) =>
{
    context.Response.StatusCode = 200;
    return "OK";
});

app.MapPost("/payu", async (HttpContext context) =>
{
    var data = await context.Request.ReadFromJsonAsync<dynamic>();
    Console.WriteLine("data: " + data);
    var client = new RestClient("https://secure.snd.payu.com/");
    var request = new RestRequest("api/v2_1/orders", Method.Post);
    request.AddHeader("Authorization", "Bearer API_KEY");
    request.AddHeader("Content-Type", "application/json");

    var obj = new Rootobject
    {
        notifyUrl = "https://your.ath-sklep-test.com/notify",
        customerIp = "127.0.0.1",
        merchantPosId = "459927",
        description = "RTV market",
        currencyCode = "PLN",
        totalAmount = "1500",
        extOrderId = data.Id,
        buyer = new Buyer
        {
            email = "john.doe@example.com",
            phone = "654111654",
            firstName = "kowalski ath",
            lastName = "ath miodek",
            language = "en"
        },
        products = new[]
        {
            new Product
            {
                name = data.Name,
                quantity = data.Quantity,
                unitPrice = data.Price
            }
        }
    };

    client.Options.FollowRedirects = false;
    request.AddJsonBody(obj);
    var response = await client.PostAsync(request);
    Console.WriteLine(response.ResponseUri);
    context.Response.StatusCode = (int)HttpStatusCode.OK;

    return JsonConvert.SerializeObject(new { Url = response.ResponseUri });
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();



public class Rootobject
{
    public string notifyUrl { get; set; }
    public string customerIp { get; set; }
    public string merchantPosId { get; set; }
    public string description { get; set; }
    public string currencyCode { get; set; }
    public string totalAmount { get; set; }
    public string extOrderId { get; set; }
    public Buyer buyer { get; set; }
    public Product[] products { get; set; }
}

public class Buyer
{
    public string email { get; set; }
    public string phone { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string language { get; set; }
}

public class Product
{
    public string name { get; set; }
    public string unitPrice { get; set; }
    public string quantity { get; set; }
}
