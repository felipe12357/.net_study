using DotnetAPI.Data;
using DotnetAPI.Models;

namespace DotnetApi.Data {
    public class PostRepository:IPostRepository{
        DataContextEF _contextEF;

        public PostRepository(IConfiguration config){
            _contextEF = new DataContextEF(config);
        }

        public bool SaveChanges(string message ){
            bool result =_contextEF.SaveChanges()>0;
            if(!result)
                throw new Exception ("Post not found");
            
            return result;
        }

        public void Insert<T>(T entityToInsert ){
            if(entityToInsert != null)
                _contextEF.Add(entityToInsert);
        }

        public void Update<T>(T entityToUpdate){
            if(entityToUpdate != null)
                _contextEF.Update(entityToUpdate);
        }

        public IEnumerable<Post> GetPosts(){
            return _contextEF.Post.ToList();
        }

        public Post GetSinglePost(int id){
            Post? post = _contextEF.Post.Where(p => p.PostId == id).FirstOrDefault();
            if(post!=null)
                return post;

             throw new Exception ("Post not found");
        }

        public IEnumerable<Post> GetPostByUser(int id){
            IEnumerable<Post> postList = _contextEF.Post.Where(p => p.UserId == id).ToList();
            return postList;
        }

         public IEnumerable<Post> GetPostBySearch(string text){
            IEnumerable<Post> postList = _contextEF.Post.Where(p => p.PostContent.Contains(text) || p.PostTitle.Contains(text)).ToList();
            return postList;
        }

        public void Delete<T>(T entityToDelete ){
            if(entityToDelete != null)
                _contextEF.Remove(entityToDelete);
        }
    }
}