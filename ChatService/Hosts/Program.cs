var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Add OpenAPI 3.0 document serving middleware
    // Available at: http://localhost:<port>/swagger/v1/swagger.json
    app.MapOpenApi();
    
    // Add web UIs to interact with the document
    // Available at: http://localhost:<port>/swagger
    //app.UseSwaggerUi(); // UseSwaggerUI Protected by if (env.IsDevelopment())
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
