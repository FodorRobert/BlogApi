using BlogApi.Models;
using BlogApi.Models.DIOs;
using BlogApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace BlogApi.Controllers
{
    [Route("blogger")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        public class BloggerController : ControllerBase
        {

            public readonly string ConnectionString = "server=localhost;database=blog;uid=root;password=";

            [HttpGet]
            public List<BlogPost> GetAllPost()
            {

                List<BlogPost> blogposts = new();

                var connector = new MySqlConnection(ConnectionString);
                connector.Open();

                string sql = "SELECT * FROM blogpost;";

                var cmd = new MySqlCommand(sql, connector);
                var dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    var post = new BlogPost
                    {
                        Id = dataReader.GetInt32(0),
                        Title = dataReader.GetString(1),
                        Content = dataReader.GetString(2),
                        PostTime = dataReader.GetDateTime(4),
                        UpdateTime = dataReader.GetDateTime(5)
                    };

                    blogposts.Add(post);
                }

                connector.Close();

                return blogposts;

            }

            [HttpPost]
            public BlogPost AddNewPost(AddPostDTO blogpost)
            {

                var connector = new MySqlConnection(ConnectionString);
                connector.Open();

                var post = new BlogPost
                {

                    Title = blogpost.Title,
                    Content = blogpost.Content,
                    BlogId = blogpost.BlogId

                };

                var sql = $"INSERT INTO `blogpost`(`Title`, `Content`, `postTime`, `updateTime`, `blogId`) VALUES ('@title','@content','@postTime','updateTime','@blogid')";

                var cmd = new MySqlCommand(sql, connector);

                cmd.Parameters.AddWithValue("@name", post.Title);
                cmd.Parameters.AddWithValue("@email", post.Content);
                cmd.Parameters.AddWithValue("@age", post.PostTime);
                cmd.Parameters.AddWithValue("@password", post.UpdateTime);
                cmd.Parameters.AddWithValue("@registrationtime", post.BlogId);

                cmd.ExecuteNonQuery();

                connector.Close();

                return post;

            }

            [HttpPut]
            public object UpdatePost([FromQuery] int id, [FromBody] UpdatePostDTO updatePostDTO)
            {

                var connector = new MySqlConnection(ConnectionString);
                connector.Open();

                var sql = @"UPDATE `blogpost` SET 
                    `Title`='@title',`Content`='@content',`updateTime`='@updatetime',`blogId`='@blogid' 
                        WHERE 1";

                var cmd = new MySqlCommand(sql, connector);

                cmd.Parameters.AddWithValue("@title", updatePostDTO.Title);
                cmd.Parameters.AddWithValue("@content", updatePostDTO.Content);
                cmd.Parameters.AddWithValue("@updatetime", DateTime.Now);
                cmd.Parameters.AddWithValue("@blogid", id);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();

                var updatePost = new BlogPost
                {
                    Title = updatePostDTO.Title,
                    Content = updatePostDTO.Content
                };

                connector.Close();

                return updatePost;
            }

            [HttpDelete]
            public object DeleteBlogger(int id)
            {

                var connector = new MySqlConnection(ConnectionString);
                connector.Open();

                var sql = $"DELETE FROM blogger WHERE id = @id";

                var cmd = new MySqlCommand(sql, connector);

                cmd.Parameters.AddWithValue(@"id", id);

                cmd.ExecuteNonQuery();

                connector.Close();

                return new { message = "Sikeres törlés!" };

            }
        }
    }
}