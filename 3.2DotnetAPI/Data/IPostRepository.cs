using DotnetAPI.Models;

namespace DotnetApi.Data {

    public interface IPostRepository{
        public bool SaveChanges(string message);
        public void Insert<T>(T entityToInsert );
        public void Update<T>(T entityToUpdate );
        public void Delete<T>(T entityToDelete );
        public IEnumerable<Post> GetPosts();
        public Post GetSinglePost(int id);
        public IEnumerable<Post> GetPostByUser(int id);
        public IEnumerable<Post> GetPostBySearch(string text);
    }
}