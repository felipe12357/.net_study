using System.Security.Claims;
using AutoMapper;
using DotnetApi.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController] //permite enviar y recibir json
    [Route("[controller]")] //con esto hago q busque el nombre de la clase omitiendo la palabra controller
    //El nombre del controlador define el nombre de la ruta
    public class PostEFController: ControllerBase{

        IPostRepository _postRepository;
        IMapper _mapper;

        public PostEFController(IConfiguration config, IPostRepository postRepository){
            _postRepository = postRepository;
            _mapper = new Mapper(new MapperConfiguration((cfg)=>{
                cfg.CreateMap<PostToAddDto,Post>(); //convierte de PostDto a Post
            }));
        }

        [HttpGet("GetPosts")]
        public IEnumerable<Post> GetPosts(){
            return _postRepository.GetPosts();
        }

        [HttpGet("GetPostsByUser/{userId}")]
        public IEnumerable<Post> GetPostsByUser(int userId){
            return _postRepository.GetPostByUser(userId);
        }

        [HttpGet("GetPostsCurrentUser")]
        public IEnumerable<Post> GetPostsCurrentUser(){

            //Esto se carga en la autentificacion en el metodo: CreateToken
            string? testValueFromToken = User.FindFirstValue("testValue");
            string? userIdstr = User.FindFirstValue("userId");
            if(userIdstr == null)
                 throw new Exception ("User not logged");

            Console.WriteLine(userIdstr + " " + testValueFromToken);

            int userId = Int32.Parse(userIdstr);
            return _postRepository.GetPostByUser(userId);
        }

        [HttpGet("GetPostBySearch/{text}")]
        public IEnumerable<Post> GetPostBySearch(string text){
            return _postRepository.GetPostBySearch(text);
        }

        [HttpGet("{postId}")]
        public Post GetPost(int postId){
            return _postRepository.GetSinglePost(postId);
        }

        [HttpPost("")]
        public IActionResult InsertPost(PostToAddDto postSent){

            string? userIdstr = User.FindFirstValue("userId");
            if(userIdstr == null)
                 throw new Exception ("User not logged");

            //mapeo el usuario gracias lo creado en el constructor:
            Post post = _mapper.Map<Post>(postSent);
            
            post.PostCreated = DateTime.Now;
            post.UserId = Int32.Parse(userIdstr); 

            _postRepository.Insert(post);
            //Console.WriteLine(JsonConvert.SerializeObject(post));
            _postRepository.SaveChanges("Failed to insert Post");
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdatePost(PostToUpdateDto postSent){
            Post postDb = _postRepository.GetSinglePost(postSent.PostId);
            postDb.PostContent = postSent.PostContent;
            postDb.PostTitle = postSent.PostTitle;
            postDb.PostUpdated = DateTime.Now;

            _postRepository.SaveChanges("Failed to update Post");
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePost(int id){
            Post postFound = _postRepository.GetSinglePost(id);
             _postRepository.Delete(postFound);
            _postRepository.SaveChanges("Failed to delete User Job Info");
            return Ok();
        }
    }
}