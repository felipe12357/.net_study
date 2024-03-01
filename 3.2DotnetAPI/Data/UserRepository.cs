using DotnetAPI.Data;
using DotnetAPI.Models;

namespace DotnetApi.Data {
    public class UserRepository:IUserRepository{
        DataContextEF _contextEF;

        public UserRepository(IConfiguration config){
            _contextEF = new DataContextEF(config);
        }

        public bool SaveChanges(string message ){
            bool result =_contextEF.SaveChanges()>0;
            if(!result)
                throw new Exception ("User not found");
            
            return result;
        }

        public void Insert<T>(T entityToInsert ){
            if(entityToInsert != null)
                _contextEF.Add(entityToInsert);
        }

        public IEnumerable<User> GetUsers(){
            return _contextEF.Users.ToList();
        }

        public User? GetSingleUser(int id){
            User? user = _contextEF.Users.Where(u => u.UserId == id).FirstOrDefault();
            if(user!=null)
                return user;

             throw new Exception ("User not found");
        }


        public void Delete<T>(T entityToDelete ){
            if(entityToDelete != null)
                _contextEF.Remove(entityToDelete);
        }

        public UserSalary? GetSingleUserSalary(int id) {
            UserSalary? user = _contextEF.UserSalary.Where(u => u.UserId == id).FirstOrDefault();

            if(user!=null)
                return user;

             throw new Exception ("User not found");
        }

        public UserJobInfo? GetSingleUserJobInfo(int id){
            UserJobInfo? user = _contextEF.UserJobInfo.Where(u => u.UserId == id).FirstOrDefault();
            
            if(user!=null)
                return user;

            throw new Exception ("User not found");
        }
    }
}