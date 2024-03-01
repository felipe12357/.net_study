using AutoMapper;
using DotnetApi.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController] //permite enviar y recibir json
[Route("[controller]")] //con esto hago q busque el nombre de la clase omitiendo la palabra controller
//El nombre del controlador define el nombre de la ruta
public class UserEFController: ControllerBase{

    IUserRepository _userRepository;
    IMapper _mapper;

    public UserEFController(IConfiguration config, IUserRepository userRepository){
        _userRepository = userRepository;
        
        _mapper = new Mapper(new MapperConfiguration((cfg)=>{
            cfg.CreateMap<UserDto,User>(); //convierte de UserDto a User
        }));
    }

    //el primer parametro sera el complemento a la ruta, en este caso GetUsers
    [HttpGet("GetUsers")] //aca la ruta es User/GetUsers
    public IEnumerable<User> GetUsers(){
        return _userRepository.GetUsers();
    }

    [HttpGet("{userId}")] //el parametro entre {} se utiliza para la ruta sea : UserEF/34
    //[HttpGet("userId")] //de lo contrario la ruta es UserEF/UserId?userId=32
    //importante el parametro "{userId}" debe coincidir con int userId
    public User? GetUser(int userId){
       return _userRepository.GetSingleUser(userId);
    }

    [HttpPut("{userId}")]
    public IActionResult UpdateUser(int userId,UserDto user){
       User? userFound = _userRepository.GetSingleUser(userId);

       if(userFound!=null){
            userFound.FirstName = user.FirstName;
            userFound.LastName = user.LastName;
            userFound.Gender = user.Gender;
            userFound.Active = user.Active;
            userFound.Email = user.Email;

            _userRepository.SaveChanges("Failed to update User");
            return Ok();
       }
        throw new Exception("Failed to update User");
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
        _userRepository.Insert(nuser);
        _userRepository.SaveChanges("Failed to insert User");
        return Ok();
    }

    [HttpDelete("{userId}")]
    public IActionResult DeleteUser(int userId){

        User? userFound = _userRepository.GetSingleUser(userId);
        if(userFound==null){
            throw new Exception("User not Found");
        }

        _userRepository.Delete(userFound);
        
        _userRepository.SaveChanges("Failed to delete User");
        return Ok();
    }

    [HttpGet("Salary/{userId}")]
    public UserSalary? GetUserSalary(int userId){
        return _userRepository.GetSingleUserSalary(userId);
    }

    [HttpPost("Salary")]
    public IActionResult InsertUserSalary(UserSalary userSalary){

        _userRepository.Insert(userSalary);

        _userRepository.SaveChanges("Failed to insert User");
        return Ok();
    }

    [HttpPut("Salary/{userId}")]
    public IActionResult UpdateUserSalary(int userId,UserSalary userSalary){
       UserSalary? userFound =  _userRepository.GetSingleUserSalary(userId);

       if(userFound!=null){
            userFound.Salary = userSalary.Salary;

            _userRepository.SaveChanges("Failed to update User");
            return Ok();
       }
        throw new Exception("Failed to update User");
    }

    [HttpDelete("Salary/{userId}")]
    public IActionResult DeleteUserSalary(int userId){

        UserSalary? userFound =  _userRepository.GetSingleUserSalary(userId);;
        if(userFound==null){
            throw new Exception("User not Found");
        }

        _userRepository.Delete(userFound);
        
        _userRepository.SaveChanges("Failed to delete User Salary");
        return Ok();
    }

    [HttpGet("JobInfo/{userId}")]
    public UserJobInfo GetUserJobInfo(int userId){
       UserJobInfo? user = _userRepository.GetSingleUserJobInfo(userId);

        if(user!=null)
            return user;

        throw new Exception ("User not found");
    }

    [HttpPost("JobInfo")]
    public IActionResult InsertUserJobInfo(UserJobInfo userJobInfo){

        _userRepository.Insert(userJobInfo);
        _userRepository.SaveChanges("Failed to insert User job info");
        return Ok();
    }

    [HttpPut("JobInfo/{userId}")]
    public IActionResult UpdateUserJobInfo(int userId,UserJobInfo userJobInfo){
       UserJobInfo? userFound = _userRepository.GetSingleUserJobInfo(userId);

        if(userFound!=null){
            userFound.JobTitle = userJobInfo.JobTitle;
            userFound.Department = userJobInfo.Department;

            _userRepository.SaveChanges("Failed to update User job info");
            return Ok();
        }
        throw new Exception("Failed to update User");
    }

    [HttpDelete("JobInfo/{userId}")]
    public IActionResult DeleteUserJobInfo(int userId){

        UserJobInfo? userFound = _userRepository.GetSingleUserJobInfo(userId);
        if(userFound==null){
            throw new Exception("User not Found");
        }

        _userRepository.Delete(userFound);
        
        _userRepository.SaveChanges("Failed to delete User Job Info");
        return Ok();
    }

}