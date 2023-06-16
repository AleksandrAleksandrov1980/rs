using Microsoft.EntityFrameworkCore;
using wmvc.Models;
using wmvc.Controllers;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<RastrwinContext>(options =>options.UseNpgsql(builder.Configuration.GetConnectionString("RastrDbContext")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); // webapi
builder.Services.AddSwaggerGen();           // webapi

var app = builder.Build();
// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    app.UseSwagger();  // for presentation! 
    app.UseSwaggerUI();// for presentation! 
}
else
{
    app.UseDeveloperExceptionPage(); // ustas enlarge exception description
    app.UseSwagger();   // webapi
    app.UseSwaggerUI(); // webapi
}
app.UseStatusCodePages(); //errs1
app.UseStatusCodePagesWithReExecute("/{0}"); //errs2
/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};
*/
/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};*/
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGeneratorEndpoints(); // add SWAGGER!

app.MapAreaEndpoints();
app.Run();

//from onion
//https://medium.com/@ankushjain358/entity-framework-core-with-postgresql-database-first-ab03bf1079c4
//PackageManagerConsole
//Scaffold-DbContext �Host=192.168.1.84;Database=rastrwin;Username=postgres;Password=pgadmin� Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir Models
//dotnet watch run