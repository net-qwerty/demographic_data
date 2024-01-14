using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddDbContext<FormDb>(opt => opt.UseInMemoryDatabase("FormList"));
// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDbContext<Context>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddCors(options =>
{
    options.AddPolicy("any", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("any");

var formItems = app.MapGroup("/formitems");

formItems.MapGet("/", GetAllForms);
formItems.MapGet("/complete", GetCompleteForms);
formItems.MapGet("/{id}", GetForms);
formItems.MapPost("/", CreateForm);
formItems.MapPut("/{id}", UpdateForm);
formItems.MapDelete("/{id}", DeleteForm);

app.Run();

static async Task<IResult> GetAllForms(Context db)
{
    return TypedResults.Ok(await db.Forms.ToArrayAsync());
}

static async Task<IResult> GetCompleteForms(Context db)
{
    return TypedResults.Ok(await db.Forms.Where(t => t.IsComplete).ToListAsync());
}

static async Task<IResult> GetForms(int id, Context db)
{
    return await db.Forms.FindAsync(id)
        is FormItem form
            ? TypedResults.Ok(form)
            : TypedResults.NotFound();
}

static async Task<IResult> CreateForm(FormItem form, Context db)
{
    db.Forms.Add(form);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/formitems/{form.Id}", form);
}

static async Task<IResult> UpdateForm(int id, FormItem inputForm, Context db)
{
    var form = await db.Forms.FindAsync(id);

    if (form is null) return TypedResults.NotFound();

    form.Name = inputForm.Name;
    form.IsComplete = inputForm.IsComplete;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteForm(int id, Context db)
{
    if (await db.Forms.FindAsync(id) is FormItem form)
    {
        db.Forms.Remove(form);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}
