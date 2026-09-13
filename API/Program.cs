using Services.Input;
using Services.Interfaces.Input;
using Services.Interfaces.Transactions;
using Services.Transactions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//input services
builder.Services.AddScoped<IInputService, InputService>();
builder.Services.AddScoped<IParsingService, SantanderCsvParsingService>();

//entity services
builder.Services.AddScoped<IChangeRequestService, ChangeRequestService>();
builder.Services.AddScoped<ITransactionCategoryService, TransactionCategoryService>();
builder.Services.AddScoped<ITransactionEvidenceService, TransactionEvidenceService>();
builder.Services.AddScoped<ITransactionPropertyService, TransactionPropertyService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

//misc
builder.Services.AddScoped<IMatchingCriteriaService, MatchingCriteriaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
