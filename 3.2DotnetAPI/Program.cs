using DotnetApi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//utilizado par apoder exponer el servicio: cors policies
builder.Services.AddCors((options)=>{
    options.AddPolicy("DevCors",(corsBuilder)=>{
        corsBuilder.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:8000","http://localhost:5000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();     
    });

    options.AddPolicy("ProdCors",(corsBuilder)=>{
        corsBuilder.WithOrigins("https://holamundo.com")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();     
    });


});

//Importante con esto permite acceder a la interfaz recibiendola como parametro en el constructor de los controladores
//haciendo algo como una inyeccion
builder.Services.AddScoped<IUserRepository,UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
    app.UseSwagger();
    app.UseSwaggerUI();  
}else{
    app.UseCors("ProdCors");
    app.UseHttpsRedirection();
}

app.MapControllers();
app.Run();
