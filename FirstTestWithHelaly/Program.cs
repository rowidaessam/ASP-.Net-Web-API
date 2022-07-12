using FirstTestWithHelaly.Models;
using FirstTestWithHelaly.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//Add Database Model
var connectionstring = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options=>options.UseSqlServer(connectionstring));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//to register services in api ==> interface GenreServices
builder.Services.AddTransient<IGenreServices, GenreServices>();
//to register services in api ==> interface MovieServices
builder.Services.AddTransient<IMovieServices, MovieServices>();
//register to automapper
builder.Services.AddAutoMapper(typeof(Program));
//to allow cors
builder.Services.AddCors();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Rowida",
        Description = "This is my first API",
        TermsOfService = new Uri("https://www.google.com"),
        Contact = new OpenApiContact
        {
            Name = "Rowida",
            Email = "rowidaessam21@gmail.com",
            Url = new Uri("https://www.google.com")
        },
        License = new OpenApiLicense
        {
            Name = "Rowida Licence",
            Url = new Uri("https://www.google.com")
        }
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
      Name="Authorization",
      Type=SecuritySchemeType.ApiKey,
      Scheme= "Bearer",
      BearerFormat="JWT",
      In=ParameterLocation.Header,
      Description="Enter Your JWT Please"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference= new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                },
                Name="Authorization",
                In=ParameterLocation.Header
            },
            new List<string>()
        }
    });
   }) ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//to allow cors too
app.UseCors(c=>c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
