var builder = WebApplication.CreateBuilder(args);
var _configuration = builder.Configuration;
var _key = Environment.GetEnvironmentVariable(DefaultProperties.JWTTokenPwd);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(ConfigureSwaggerGen);
builder.Services.AddHealthChecks();

builder.Services.ConfigureJwtAndPolicies();

builder.Services.AddSingleton(new JwtTokenService(_key));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapHealthChecks("/health");
app.MapCredentialsEndpoints();
app.MapBlogEndpoints();


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
