using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add DBContext
string connectionString = builder.Configuration.GetConnectionString("API");
builder.Services.AddDbContext<api.Context.DBContext>(
    options => options.UseSqlServer(connectionString)
);

//CORS config
builder.Services.AddCors(p => p.AddPolicy("CorConfig", build =>
{
    build.WithOrigins("http://localhost:1111").AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();