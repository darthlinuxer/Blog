using Application.Extensions;

var builder = WebApplication.CreateBuilder(args);
var _configuration = builder.Configuration;
var _key = Environment.GetEnvironmentVariable(StaticText.JWTTokenPwd) ?? "";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(ConfigureSwaggerGen);
builder.Services.AddHealthChecks();
builder.Services.AddControllers();

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.MaxDepth = 10;
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
});
builder.Services.AddCarter();

builder.Services.ConfigureJwtAndPolicies();
builder.Services.ConfigIdentityCore();
builder.Services.ConfigCustomDbContext(_configuration);
builder.Services.AddCustomScopedServices();

builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    //Validations will be done inside services, not controllers
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
//app.MapHealthChecks("/health");
//app.MapCarter();
app.UseEndpoints(configure =>
{
    configure.MapControllers();
    configure.MapCarter();
    configure.MapHealthChecks("/health");
});

// Role and Editor seeding
using var scope = app.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;
var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userService = serviceProvider.GetRequiredService<IUserService>();
RoleInitializer.InitializeAsync(roleManager).Wait();
SeedInitializer.InitializeAsync(userService).Wait();

app.Run();

#region Swagger configurations

/// <summary>
///     Swagger configuration.
/// </summary>
/// <param name="options">Option instances for configuring Swagger.</param>
void ConfigureSwaggerGen(SwaggerGenOptions options)
{
    var projectAssemblyName = _configuration.GetValue<string>("ServiceName");

    // add JWT Authentication
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: Bearer 1safsfsdfdfd",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer", // must be lower case
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });

    options.SwaggerDoc("v1", new OpenApiInfo { Title = projectAssemblyName, Version = "v1" });
}

#endregion Swagger configurations