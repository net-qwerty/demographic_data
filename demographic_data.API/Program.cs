using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddDbContext<RESTfull.Infrastructure.Context>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddDbContext<FormDb>(opt => opt.UseInMemoryDatabase("FormList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/formitems", async (FormDb db) =>
    await db.Forms.ToListAsync());

app.MapGet("/formitems/complete", async (FormDb db) =>
    await db.Forms.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/formitems/{id}", async (int id, FormDb db) =>
    await db.Forms.FindAsync(id)
        is Form form
            ? Results.Ok(form)
            : Results.NotFound());

app.MapPost("/formitems", async (Form form, FormDb db) =>
{
    db.Forms.Add(form);
    await db.SaveChangesAsync();

    return Results.Created($"/formitems/{form.Id}", form);
});

app.MapPut("/formitems/{id}", async (int id, Form inputForm, FormDb db) =>
{
    var form = await db.Forms.FindAsync(id);

    if (form is null) return Results.NotFound();

    form.Name = inputForm.Name;
    form.IsComplete = inputForm.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/formitems/{id}", async (int id, FormDb db) =>
{
    if (await db.Forms.FindAsync(id) is Form form)
    {
        db.Forms.Remove(form);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});










var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
