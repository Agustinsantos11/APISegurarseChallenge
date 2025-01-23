using ChallengeAPISegurarse.Context;
using ChallengeAPISegurarse.Interfaces;
using ChallengeAPISegurarse.Repositories;
using ChallengeAPISegurarse.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.Configure<ClienteValidationServiceOptions>(builder.Configuration.GetSection("ClienteValidationService"));


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=DataBase/ApiSegurarse.db"));


builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddHttpClient<IClienteValidationService, ClienteValidationService>();


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
