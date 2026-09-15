using Common.DbSetup;
using Services.Setup;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();


//add config
var connectionString = ConnectionString.GetConnectionString();
builder.Services.SetupServices(connectionString);


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
