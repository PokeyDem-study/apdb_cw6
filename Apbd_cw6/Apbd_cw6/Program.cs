var builder = WebApplication.CreateBuilder(args);
//w ssms wybieramy winodws connecition
// CREATE TABLE Animal(
//     IdAnimal INT PRIMARY KEY IDENTITY(1,1),
//     Name NVARCHAR(200),
//     Description NVARCHAR(200) NUll,
//     Category NVARCHAR(200),
//     Area NVARCHAR (200),
// )
//nazwa projektu(Apbd_cw6 C#) -> pkm -> manage nuget -> microsoft.data.sqlclient
//apsettings.json dodajemy ConnectionString
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();//1

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();//2

app.Run();