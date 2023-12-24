using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var formItems = app.MapGroup("/formitems");

formItems.MapGet("/", GetAllForms);
formItems.MapGet("/complete", GetCompleteForms);
formItems.MapGet("/{id}", GetForms);
formItems.MapPost("/", CreateForm);
formItems.MapPut("/{id}", UpdateForm);
formItems.MapDelete("/{id}", DeleteForm);

app.Run();

static async Task<IResult> GetAllForms(FormDb db)
{
    return TypedResults.Ok(await db.Forms.ToArrayAsync());
}

static async Task<IResult> GetCompleteForms(FormDb db)
{
    return TypedResults.Ok(await db.Forms.Where(t => t.IsComplete).ToListAsync());
}

static async Task<IResult> GetForms(int id, FormDb db)
{
    return await db.Forms.FindAsync(id)
        is Form form
            ? TypedResults.Ok(form)
            : TypedResults.NotFound();
}

static async Task<IResult> CreateForm(Form form, FormDb db)
{
    db.Forms.Add(form);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/formitems/{form.Id}", form);
}

static async Task<IResult> UpdateForm(int id, Form inputForm, FormDb db)
{
    var form = await db.Forms.FindAsync(id);

    if (form is null) return TypedResults.NotFound();

    form.Name = inputForm.Name;
    form.IsComplete = inputForm.IsComplete;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteForm(int id, FormDb db)
{
    if (await db.Forms.FindAsync(id) is Form form)
    {
        db.Forms.Remove(form);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}
