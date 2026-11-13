using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Services.Abstractions;
using Services.Repositories.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(); // TODO VS Возможно удалить. Это аналог swagger.

builder.Services.AddDbContext<ChatServiceContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(ChatServiceContext)));
    });
builder.Services.AddScoped<IChatService, Services.Implementations.ChatService>();
builder.Services.AddScoped<IChatRepository, Infrastructure.Repositories.Implementations.ChatRepository>();

builder.Services.AddAutoMapper(cfg => { }, typeof(Services.Implementations.ChatService));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Add OpenAPI 3.0 document serving middleware
    // Available at: http://localhost:<port>/swagger/v1/swagger.json
    // TODO VS доступен json по адресу
    // https://localhost:{PORT}/openapi/v1.json.
    app.MapOpenApi();
    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
