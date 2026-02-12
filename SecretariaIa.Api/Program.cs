using Microsoft.AspNetCore.Builder;
using SecretariaIa.Api.AI.TrainingSamples;
using SecretariaIa.Infrasctructure.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddMediator(Assembly.GetExecutingAssembly());
builder.Services.AddSecurity(builder.Configuration);
builder.Services.AddSwagger<SwaggerFillter>(builder.Configuration);
builder.Services.AddScoped<ITrainingSamplesProvider, TrainingSamplesProvider>();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowFrontend",
		policy =>
		{
			policy
				.WithOrigins(
					"http://localhost:5173",
					"https://secretariamonitoringapp-production.up.railway.app/"
				// seu frontend local (Vite)
				)
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowCredentials(); // se usar cookies ou auth
		});
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseHttpsRedirection();
}
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
