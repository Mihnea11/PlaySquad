using Server.Repositories;
using Server.Services;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
// Add services
builder.Services.AddControllers();
builder.Services.AddScoped<Npgsql.NpgsqlConnection>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("Supabase");
    return new Npgsql.NpgsqlConnection(connectionString);
});
builder.Services.AddScoped<UserRepository>();
builder.Services.AddHttpClient<SupabaseService>();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
