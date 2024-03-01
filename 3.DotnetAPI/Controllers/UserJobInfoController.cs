using DotnetAPI.Data;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;
namespace DotnetAPI.Controllers;

[ApiController] //permite enviar y recibir json
[Route("[controller]")] //con esto hago q busque el nombre de la clase omitiendo la palabra controller
//El nombre del controlador define el nombre de la ruta
public class UserJobInfoController: ControllerBase{

    DataContextDapper _dapper;

    public UserJobInfoController(){
        _dapper = new DataContextDapper();
    }

    [HttpGet("{userId}")] //el parametro entre {} se utiliza para la ruta sea : UserJobInfo/34
    //[HttpGet("userId")] //de lo contrario la ruta es UserJobInfo/UserId?userId=32
    //importante el parametro "{userId}" debe coincidir con int userId
    public UserJobInfo GetUserJobInfo(int userId){
        string query =  @"
                SELECT * 
                FROM TutorialAppSchema.UserJobInfo
                Where UserId = '"+userId+"' ";
        return _dapper.loadSingle<UserJobInfo>(query);
    }

    //el primer parametro sera el complemento a la ruta, en este caso GetUsers
    [HttpGet("GetUsersJobInfo")] //aca la ruta es UserJobInfo/GetUsersJobInfo
    public IEnumerable<UserJobInfo> GetUsersJobInfo(){
        string query =  @"
                SELECT * 
                FROM TutorialAppSchema.UserJobInfo";
        return _dapper.LoadData<UserJobInfo>(query);
    }

    [HttpPut("{userId}")]
    public IActionResult UpdateUserJobInfo(int userId,UserJobInfo userJobInfo){
        string query =  @"
                UPDATE TutorialAppSchema.UserJobInfo 
                SET JobTitle = '" + userJobInfo.JobTitle + "'"
                + ", Department = '" + userJobInfo.Department + "'"
                + " Where UserId = '"+userId+"' ";
        
        int rowsAffected = _dapper.ExecuteSql(query);

        if(rowsAffected>0)
            return Ok();
        
        throw new Exception("Failed to update UserJobInfo");
    }

    [HttpPost("")]
    public IActionResult InsertUserJobInfo(UserJobInfo userJobInfo){
        string query =  @"
          insert into TutorialAppSchema.UserJobInfo(
                JobTitle,Department,UserId)
            Values( '" + userJobInfo.JobTitle + "' , '" + userJobInfo.Department + "', '" + userJobInfo.UserId + "' )";
        
        int rowsAffected = _dapper.ExecuteSql(query);
        Console.WriteLine(query);
        if(rowsAffected==1)
            return Ok();
        
        throw new Exception("Failed to insert UserJobInfo");
    }

    [HttpDelete("{userId}")]
    public IActionResult DeleteUserJobInfo(int userId){

        string query =  @"Delete from TutorialAppSchema.UserJobInfo where UserId = " + userId;
        Console.WriteLine(query);
        int rowsAffected = _dapper.ExecuteSql(query);

         if(rowsAffected>0)
            return Ok();
        
        throw new Exception("Failed to delete UserJobInfo");

    }

}