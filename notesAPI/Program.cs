using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using notesAPI.Entities;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddEntityFrameworkMySQL()
    .AddDbContext<NotesContext>(options =>
    {
        string NotesConnection = builder.Configuration.GetConnectionString("NotesConnection") ?? "";
        options.UseMySQL(NotesConnection);
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


var notesRoutes = app.MapGroup("/api/note");

notesRoutes.MapGet("/", GetAllNotes).WithOpenApi();
notesRoutes.MapGet("/{id}", GetNote).WithOpenApi();
notesRoutes.MapPost("/", CreateNote).WithOpenApi();
notesRoutes.MapPut("/{id}", UpdateNote).WithOpenApi();
notesRoutes.MapDelete("/{id}", DeleteNote).WithOpenApi();

notesRoutes.MapGet("/grouped-by-created-date", GetNotesGroupedByCreatedAtWithDateRange).WithOpenApi();
notesRoutes.MapGet("/grouped-by-updated-date", GetNotesGroupedByUpdatedAtWithDateRange).WithOpenApi();

app.UseCors("AllowAllOrigins");

app.Run();


static async Task<IResult> GetAllNotes(NotesContext dbContext)
{
    return TypedResults.Ok(await dbContext.Notes.ToArrayAsync());
}

static async Task<IResult> GetNote(int id, NotesContext dbContext)
{
    Note? note = await dbContext.Notes.FindAsync(id);

    if (note is null)
        return TypedResults.NotFound();

    return TypedResults.Ok(note);
}

static async Task<IResult> CreateNote(Note note, NotesContext dbContext)
{
    DateTime currentDateTime = DateTime.Now;
    note.CreatedAt = currentDateTime;
    note.UpdatedAt = currentDateTime;
    dbContext.Notes.Add(note);
    await dbContext.SaveChangesAsync();

    return TypedResults.Created($"/notes/{note.Id}", note);
}

static async Task<IResult> UpdateNote(int id, Note inputNote, NotesContext dbContext)
{
    Note? note = await dbContext.Notes.FindAsync(id);

    if (note is null)
        return TypedResults.NotFound();

    note.Title = inputNote.Title;
    note.Body = inputNote.Body;
    note.UpdatedAt = DateTime.Now;

    await dbContext.SaveChangesAsync();

    return TypedResults.Ok(note);
}

static async Task<IResult> DeleteNote(int id, NotesContext dbContext)
{
    Note? note = await dbContext.Notes.FindAsync(id);

    if (note is null)
        return TypedResults.NotFound();

    dbContext.Notes.Remove(note);
    await dbContext.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> GetNotesGroupedByCreatedAtWithDateRange(DateTime fromDate, DateTime toDate, NotesContext dbContext)
{
    var groupedNotes = await dbContext.Notes
        .Where(note => note.CreatedAt >= fromDate && note.CreatedAt <= toDate.AddDays(1))
        .GroupBy(note => note.CreatedAt.Date)
        .Select(group => new
        {
            CreatedAt = group.Key.Date,
            NoteCount = group.Count()
        })
        .ToListAsync();

    if (groupedNotes == null)
    {
        return TypedResults.NotFound();
    }

    return TypedResults.Ok(groupedNotes);
}

static async Task<IResult> GetNotesGroupedByUpdatedAtWithDateRange(DateTime fromDate, DateTime toDate, NotesContext dbContext)
{
    var groupedNotes = await dbContext.Notes
        .Where(note => note.UpdatedAt >= fromDate && note.UpdatedAt <= toDate.AddDays(1) && note.CreatedAt < note.UpdatedAt )
        .GroupBy(note => note.UpdatedAt.Date)
        .Select(group => new
        {
            UpdatedAt = group.Key.Date,
            NoteCount = group.Count()
        })
        .ToListAsync();

    if (groupedNotes == null)
    {
        return TypedResults.NotFound();
    }

    return TypedResults.Ok(groupedNotes);
}




//dotnet ef migrations add InitialCreate
//dotnet ef database update

//dotnet ef dbcontext scaffold "server=note.mysql.database.azure.com;port=3306;user=notes;password=VTA8Q6i8>%T)wJk;database=notes" MySql.EntityFrameworkCore -o Entities

//dotnet ef dbcontext scaffold "Name=ConnectionStrings:NotesConnection" MySql.EntityFrameworkCore -o Entities