using BorroApp.Data;

using Azure.Storage.Blobs;

using Microsoft.Extensions.Azure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

using BorroApp.Extensions;

using System.Text;
using System.Text.Json.Serialization;

var builder   = WebApplication.CreateBuilder(args);
var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey    = builder.Configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	   .AddJwtBearer(options => {
		   options.TokenValidationParameters = new TokenValidationParameters {
			   ValidateIssuer           = true,
			   ValidateAudience         = true,
			   ValidateLifetime         = true,
			   ValidateIssuerSigningKey = true,
			   ValidIssuer              = jwtIssuer,
			   ValidAudience            = jwtIssuer,
			   IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
		   };
	   });

 builder.Services.AddAzureClients(x => {
 	x.AddClient<BlobContainerClient, BlobContainerClientOptions>(opt =>
 																	 new BlobContainerClient(
 																		 builder.Configuration[
 																			 "PictureStorage:ConnectionString"],
 																		 builder.Configuration[
 																			 "PictureStorage:ContainerName"]));
 });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
        .AllowAnyOrigin() // Allows requests from any origin
        .AllowAnyMethod()
         .AllowCredentials()                                                   // Allows all HTTP methods
        .AllowAnyHeader()); // Allows any header
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {

            builder.WithOrigins("http://localhost:3000");
        });

   /* options.AddPolicy("AnotherPolicy",
        builder =>
        {
            builder.WithOrigins(CORSComplianceDomains)
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
   */
});
builder.Services.AddControllers().AddJsonOptions(c => {
	c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BorroDbContext>(
	c => c.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();
// Configure the HTTP request pipeline.
var key1 = app.Configuration.GetValue<String>("KEY");

    app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});





app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }