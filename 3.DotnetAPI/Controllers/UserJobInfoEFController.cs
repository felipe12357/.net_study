using DotnetAPI.Data;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController] //permite enviar y recibir json
[Route("[controller]")] //con esto hago q busque el nombre de la clase omitiendo la palabra controller
//El nombre del controlador define el nombre de la ruta
public class UserJobInfoEFController: ControllerBase{

    DataContextEF _dataEF;
    
    public UserJobInfoEFController(){
        _dataEF = new DataContextEF();
    }

    [HttpGet("{userId}")] //el parametro entre {} se utiliza para la ruta sea : UserJobInfoEF/34
    //[HttpGet("userId")] //de lo contrario la ruta es UserJobInfoEF/UserId?userId=32
    //importante el parametro "{userId}" debe coincidir con int userId
    public UserJobInfo GetUserJobInfo(int userId){
       UserJobInfo? user = _dataEF.UserJobInfo.Where(u => u.UserId == userId).FirstOrDefault();

       if(user!=null)
        return user;

        throw new Exception ("User not found");
    }

     //el primer parametro sera el complemento a la ruta, en este caso GetUsers
    [HttpGet("GetUsersJobInfo")] //aca la ruta es UserJobInfoEF/GetUsers
    public IEnumerable<UserJobInfo> GetUsersJobInfo(){
        return _dataEF.UserJobInfo.ToList<UserJobInfo>();
    }

    [HttpPost("")]
    public IActionResult InsertUserJobInfo(UserJobInfo userJobInfo){

        //_dataEF.Add(userJobInfo);
        _dataEF.UserJobInfo.Add(userJobInfo);

        if(_dataEF.SaveChanges() == 0)
            throw new Exception("Failed to insert User");
      
        return Ok();
    }

    [HttpPut("{userId}")]
    public IActionResult UpdateUser(int userId,UserJobInfo userJobInfo){
       UserJobInfo? userFound = _dataEF.UserJobInfo.Where(u => u.UserId == userId).FirstOrDefault();

       if(userFound!=null){
            userFound.JobTitle = userJobInfo.JobTitle;
            userFound.Department = userJobInfo.Department;

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

        UserJobInfo? userFound = _dataEF.UserJobInfo.Where(u => u.UserId == userId).FirstOrDefault();
        if(userFound==null){
            throw new Exception("User not Found");
        }

        _dataEF.UserJobInfo.Remove(userFound);
        int response = _dataEF.SaveChanges();
        
        if(response>0)
            return Ok();
        else
            throw new Exception("Failed to delete User");
    }
}