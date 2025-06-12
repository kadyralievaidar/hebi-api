using Hebi_Api.Features.Core.Extensions;
using Hebi_Api.Features.OpenIdDict;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureRepository();
builder.Services.RegisterRequestHandlers();
builder.Services.RegisterServices();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "front-end",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173",
                                              "http://hebi-client.gonget.net",
                                              "https://localhost:5172",
                                              "https://hebi-client.gonget.net");
                      });
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "You api title", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });
}
);
builder.AddOpenIdDict();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
//await app.MigrateDatabaseAsync();
await app.SeedRoles();
app.UseCors("front-end");
app.UseHttpsRedirection();
app.UseStatusCodePages(); // <== this helps show 401/403/404 responses clearly

app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();

app.Run();
