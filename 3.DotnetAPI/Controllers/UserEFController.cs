using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;
namespace DotnetAPI.Controllers;

[ApiController] //permite enviar y recibir json
[Route("[controller]")] //con esto hago q busque el nombre de la clase omitiendo la palabra controller
//El nombre del controlador define el nombre de la ruta
public class UserEFController: ControllerBase{

    DataContextEF _dataEF;
    IMapper _mapper;
    public UserEFController(){
        _dataEF = new DataContextEF();

        _mapper = new Mapper(new MapperConfiguration((cfg)=>{
            cfg.CreateMap<UserDto,User>(); //convierte de UserDto a User
        }));
    }

    [HttpGet("{userId}")] //el parametro entre {} se utiliza para la ruta sea : UserEF/34
    //[HttpGet("userId")] //de lo contrario la ruta es UserEF/UserId?userId=32
    //importante el parametro "{userId}" debe coincidir con int userId
    public User GetUser(int userId){
       User? user = _dataEF.Users.Where(u => u.UserId == userId).FirstOrDefault();

       if(user!=null)
        return user;

        throw new Exception ("User not found");
    }

    //el primer parametro sera el complemento a la ruta, en este caso GetUsers
    [HttpGet("GetUsers")] //aca la ruta es User/GetUsers
    public IEnumerable<User> GetUsers(){
        return _dataEF.Users.ToList<User>();
    }

    [HttpPost("")]
    public IActionResult InsertUser(UserDto user){
        
        /* User nuser = new User(){};
        nuser.FirstName = user.FirstName;
        nuser.LastName = user.LastName;
        nuser.Email = user.Email;
        nuser.Active = user.Active; */
        
        //mapeo el usuario gracias lo creado en el constructor:
        User nuser = _mapper.Map<User>(user);

        //_dataEF.Add(nuser);
        _dataEF.Users.Add(nuser);

        if(_dataEF.SaveChanges() == 0)
            throw new Exception("Failed to insert User");
      
        return Ok();
    }

    [HttpPut("{userId}")]
    public IActionResult UpdateUser(int userId,UserDto user){
       User? userFound = _dataEF.Users.Where(u => u.UserId == userId).FirstOrDefault();

       if(userFound!=null){
            userFound.FirstName = user.FirstName;
            userFound.LastName = user.LastName;
            userFound.Gender = user.Gender;
            userFound.Active = user.Active;
            userFound.Email = user.Email;

            int response = _dataEF.SaveChanges();

            if(response > 0)
                return Ok();
            else
            {
                throw new Exception("Failed to update User");
            }
       }
        throw new Exception("Failed to update User");
    }

    [HttpDelete("{userId}")]
    public IActionResult DeleteUser(int userId){

        User? userFound = _dataEF.Users.Where(u => u.UserId == userId).FirstOrDefault();
        if(userFound==null){
            throw new Exception("User not Found");
        }

        _dataEF.Users.Remove(userFound);
        int response = _dataEF.SaveChanges();
        
        if(response>0)
            return Ok();
        else
            throw new Exception("Failed to delete User");
    }
}