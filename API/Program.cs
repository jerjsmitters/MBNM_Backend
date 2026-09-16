using Common.DbSetup;
using Services.Setup;

var builder = WebApplication.CreateBuilder(args);

//configure cors
var policyName = "mbn-policy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: policyName,
        policy => policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials()
    );
});


// Add services to the container.
builder.Services.AddControllers();


//add config
var connectionString = ConnectionString.GetConnectionString();
builder.Services.SetupServices(connectionString);


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseCors(policyName);

app.UseAuthorization();

app.MapControllers();

app.Run();
