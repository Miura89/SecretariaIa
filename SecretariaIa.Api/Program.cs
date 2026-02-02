using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecretariaIa.Api;
using SecretariaIa.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(opt =>
	opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapPost("/webhooks/twilio/whatsapp/inbound",
	async ([FromForm] TwilioInboundForm form, AppDbContext db) =>
	{
		var entity = new InboundMessage
		{
			From = form.From ?? "",
			To = form.To ?? "",
			Body = form.Body ?? "",
			MessageSid = form.MessageSid
		};

		// validação mínima
		if (string.IsNullOrWhiteSpace(entity.From) ||
			string.IsNullOrWhiteSpace(entity.To) ||
			string.IsNullOrWhiteSpace(entity.Body))
			return Results.BadRequest("Payload inválido.");

		db.InboundMessages.Add(entity);
		await db.SaveChangesAsync();

		// pode retornar 200 OK simples (ou TwiML se quiser responder)
		return Results.Ok(new { saved = true, id = entity.Id });
	});


app.Run();
