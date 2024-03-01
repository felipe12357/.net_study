using DotnetAPI.Data;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController] //permite enviar y recibir json
[Route("[controller]")] //con esto hago q busque el nombre de la clase omitiendo la palabra controller
//El nombre del controlador define el nombre de la ruta
public class UserSalaryEFController: ControllerBase{
    DataContextEF _dataEF;
    
    public UserSalaryEFController(){
        _dataEF = new DataContextEF();
    }

    [HttpGet("{userId}")] //el parametro entre {} se utiliza para la ruta sea : UserSalaryEF/34
    //[HttpGet("userId")] //de lo contrario la ruta es UserSalaryEF/UserId?userId=32
    //importante el parametro "{userId}" debe coincidir con int userId
    public UserSalary GetUserSalary(int userId){
       UserSalary? user = _dataEF.UserSalary.Where(u => u.UserId == userId).FirstOrDefault();

       if(user!=null)
        return user;

        throw new Exception ("User not found");
    }

    //el primer parametro sera el complemento a la ruta, en este caso GetUsersSalary
    [HttpGet("GetUsersSalary")] //aca la ruta es UserSalaryEF/GetUsersSalary
    public IEnumerable<UserSalary> GetUsersSalary(){
        return _dataEF.UserSalary.ToList<UserSalary>();
    }

    [HttpPost("")]
    public IActionResult InsertUserSalary(UserSalary userSalary){

        _dataEF.UserSalary.Add(userSalary);

        if(_dataEF.SaveChanges() == 0)
            throw new Exception("Failed to insert User");
      
        return Ok();
    }

    [HttpPut("{userId}")]
    public IActionResult UpdateUser(int userId,UserSalary userSalary){
       UserSalary? userFound = _dataEF.UserSalary.Where(u => u.UserId == userId).FirstOrDefault();

       if(userFound!=null){
            userFound.Salary = userSalary.Salary;

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

        UserSalary? userFound = _dataEF.UserSalary.Where(u => u.UserId == userId).FirstOrDefault();
        if(userFound==null){
            throw new Exception("User not Found");
        }

        _dataEF.UserSalary.Remove(userFound);
        int response = _dataEF.SaveChanges();
        
        if(response>0)
            return Ok();
        else
            throw new Exception("Failed to delete User");
    }
}