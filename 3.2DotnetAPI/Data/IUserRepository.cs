using DotnetAPI.Models;

namespace DotnetApi.Data {

    public interface IUserRepository{
        public bool SaveChanges(string message);
        public void Insert<T>(T entityToInsert );
        public void Delete<T>(T entityToDelete );
        public IEnumerable<User> GetUsers();
        public User GetSingleUser(int id);
        public UserSalary? GetSingleUserSalary(int id);
        public UserJobInfo? GetSingleUserJobInfo(int id);
        public User? GetUserByEmail(string mail);
         public UserAuth? GetUserAuth(string mail); 
    }
}