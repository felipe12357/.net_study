using System.Text;
using DotnetApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
builder.Services.AddScoped<IPostRepository,PostRepository>();

string? tokenkeyString = builder.Configuration.GetSection("AppSettings:TokenKey").Value;
//Codigo agregado para validar los tokens
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>{
    options.TokenValidationParameters = new TokenValidationParameters(){
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(tokenkeyString != null ? tokenkeyString : "")
        ),
        ValidateAudience = false,
        ValidateIssuer = false
    };
});

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

//IMPORTANTE EL ORDEN DE ESTAS 2 LINEAS!
app.UseAuthentication();
app.UseAuthorization(); //implementado para los tokens

app.MapControllers();
app.Run();
