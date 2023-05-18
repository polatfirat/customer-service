using Customer.Api.Middleware;
using Customer.Repository.Abstract;
using Customer.Repository.Concrete;
using Customer.Repository.Context;
using Customer.Service.Abstract;
using Customer.Service.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt => opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                    new HeaderApiVersionReader("x-api-version"),
                                                    new MediaTypeApiVersionReader("x-api-version"));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddTransient<IRepository<,>>
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddDbContext<CustomerEFContext>(options => options.UseInMemoryDatabase("CustomerDatabase"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
