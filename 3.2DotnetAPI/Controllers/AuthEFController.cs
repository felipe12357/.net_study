using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using DotnetApi.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Controllers
{
    [Authorize] //indica q debe enviar un token para acceder
    [ApiController] //permite enviar y recibir json
    [Route("[controller]")] //con esto hago q busque el nombre de la clase omitiendo la palabra controller
    //El nombre del controlador define el nombre de la ruta
    public class AuthEFController: ControllerBase{
        private IConfiguration _config; 
        private IUserRepository _userRepository;
        IMapper _mapper;
        public AuthEFController(IConfiguration config,IUserRepository userRepository){
            _config = config;
            _userRepository = userRepository;

             _mapper = new Mapper(new MapperConfiguration((cfg)=>{
             cfg.CreateMap<UserRegistrationDto,User>(); //convierte de UserRegistrationDto a User
        }));
        }

        [AllowAnonymous] //permite q este metodo sea accesible sin el token
        [HttpPost("Register")]
        public IActionResult Register(UserRegistrationDto user){
            if(user.Password != user.PasswordConfirmation)
                throw new Exception ("Password and PasswordConfirmation not match");

            if(_userRepository.GetUserByEmail(user.Email)!=null)
                throw new Exception ("The email already exits");

            byte[] passwordSalt = new byte[128/8];
            using (RandomNumberGenerator rng= RandomNumberGenerator.Create()){
                rng.GetNonZeroBytes(passwordSalt);
            }
            
            byte[] passwordHash = this.GetPasswordHash(user.Password,passwordSalt);

            UserAuth userAuth = new(){
                Email = user.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            //mapeo el usuario gracias lo creado en el constructor:
            User nuser = _mapper.Map<User>(user);
            nuser.Active = true;

            _userRepository.Insert(userAuth);
            _userRepository.Insert(nuser);
          
            _userRepository.SaveChanges("Failed to insert User");
              Console.WriteLine("aca esta el id"+nuser.UserId);
            return Ok(
                new Dictionary<string,string>(){
                    {"token",CreateToken(nuser.UserId)}
                }
            );
        }

        [AllowAnonymous] //permite q este metodo sea accesible sin el token
        [HttpPost("Login")]
        public IActionResult Login(UserLoginDto user){
            UserAuth? userAuth = _userRepository.GetUserAuth(user.Email);

            if(userAuth == null)
                 return StatusCode(401,"User do not exits" );

            byte[] passwordHash = this.GetPasswordHash(user.Password,userAuth.PasswordSalt);
            
            //if(passwordHash == userAuth.PasswordHash) => toca comparar pada posicion del array
           for (int index =0; index < passwordHash.Length; index ++){
                if(passwordHash[index]!=userAuth.PasswordHash[index])
                    return StatusCode(401,"Password not Valid" );
           }

            User? userDb = _userRepository.GetUserByEmail(user.Email);
            if(userDb == null)
                 throw new Exception ("Error in User");
            Console.WriteLine(userDb.UserId);
            return Ok(new Dictionary<string,string>(){
                        {"token",CreateToken(userDb.UserId)}
                    });           
        }

        [HttpGet("RefreshToken")]
        public string RegreshToken(){
             Console.WriteLine(User);              
            string? userId =  User.FindFirst("userId")?.Value; 
            string?  userId2 = User.FindFirstValue("UserId");

            if(userId == null)
                throw new Exception ("Error in User");

            Console.WriteLine(userId + "-" + userId2);

            int userIdF = _userRepository.GetSingleUser(int.Parse(userId)).UserId;

            return CreateToken(userIdF);
        }

        private byte[] GetPasswordHash(string password,byte[] passwordSalt){
            string passwordSaltPlusString = _config.GetSection("AppSettings:Passwordkey").Value + Convert.ToBase64String(passwordSalt);
            byte[] passwordHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100,
                numBytesRequested: 256/8
            );
            return passwordHash;
        }

        private string CreateToken(int userId){
            Claim[] claims = new Claim[]{
                new("userId", userId.ToString())
            };

            string? tokenkeyString = _config.GetSection("Appsettings:TokenKey").Value;

            SymmetricSecurityKey tokenkey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    tokenkeyString != null ? tokenkeyString : ""
                )
            ); 

            SigningCredentials credentials = new SigningCredentials(tokenkey,SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor(){
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(1)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}