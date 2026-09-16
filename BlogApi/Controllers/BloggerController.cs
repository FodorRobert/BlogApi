using BlogApi.Models;
using BlogApi.Models.DIOs;
using BlogApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Security.Cryptography;

namespace BlogApi.Controllers
{
    [Route("blogger")]
    [ApiController]
    public class BloggerController : ControllerBase
    {

        public readonly string ConnectionString = "server=localhost;database=blog;uid=root;password=";

        [HttpGet]
        public List<Blogger> GetAllBlogger()
        {

            List<Blogger> bloggers = new();

            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = "SELECT * FROM blogger;";

            var cmd = new MySqlCommand(sql, connector);
            var dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                var blogger = new Blogger
                {
                    Id = dataReader.GetInt32(0),
                    Name = dataReader.GetString(1),
                    Email = dataReader.GetString(2),
                    Age = dataReader.GetInt32(3),
                    Password = dataReader.GetString(4),
                    RegistrationTime = dataReader.GetDateTime(5)
                };

                bloggers.Add(blogger);
            }

            connector.Close();

            return bloggers;

        }

        [HttpPost]
        public Blogger AddNewBlogger(AddBloggerDTOs blogger)
        {

            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var blg = new Blogger
            {

                Name = blogger.Name,
                Email = blogger.Email,
                Age = blogger.Age,
                Password = blogger.Password,
                RegistrationTime = DateTime.Now

            };

            var sql = $"INSERT INTO `blogger`(`Name`, `Email`, `Age`, `Password`, `RegistrationTime`) VALUES (@name, @email, @age, @password, @registrationtime)";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@name", blg.Name);
            cmd.Parameters.AddWithValue("@email", blg.Email);
            cmd.Parameters.AddWithValue("@age", blg.Age);
            cmd.Parameters.AddWithValue("@password", blg.Password);
            cmd.Parameters.AddWithValue("@registrationtime", blg.RegistrationTime);

            cmd.ExecuteNonQuery();

            connector.Close();

            return blg;

        }

        [HttpPut]
        public UpdateBloggerDTOs UpdateBlogger([FromQuery] int id, [FromBody] UpdateBloggerDTOs updateBloggerDTO)
        {

            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = @"UPDATE `blogger` SET `name` = @name, `email` = @email, `age` = @age, `password` = @password
                       WHERE `id` = 1;";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@name", updateBloggerDTO.Name);
            cmd.Parameters.AddWithValue("@email", updateBloggerDTO.Email);
            cmd.Parameters.AddWithValue("@age", updateBloggerDTO.Age);
            cmd.Parameters.AddWithValue("@password", updateBloggerDTO.Password);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            var updateBlogger = new UpdateBloggerDTOs
            {
                Name = updateBloggerDTO.Name,
                Email = updateBloggerDTO.Email,
                Age = updateBloggerDTO.Age,
                Password = updateBloggerDTO.Password
            };

            connector.Close();

            return updateBlogger;
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

        [HttpGet]
        public object GetBloggerById(int id)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            var sql = $"SELECT `Name` FROM `blogger` " +
                "WHERE 'id' = @id";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();
            datareader.Read();

            var blogger = new Blogger
            {
                Name = datareader.GetString(0),
                Email = datareader.GetString(1),
            };

            connector.Close();

            return blogger;
        }

        [HttpGet]
        public List<object> GetBloggerByBlogId(int id)
        {
            List<object> OnPost = new List<object>();
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = $"SELECT blogger.Name, blogpost.Title, blogpost.Content\r\nFROM `blogger`\r\nINNER JOIN blogpost ON blogger.Id-blogpost.blogId\r\nWHERE blogger.`id` = 4";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();

            while (datareader.Read())
            {
                var bloggerOwnPosts = new
                {
                    Name = datareader.GetString(0),
                    Title = datareader.GetString(1),
                    Content = datareader.GetString(2)
                };
                OnPost.Add(bloggerOwnPosts);
            }

            connector.Close();

            return OnPost;
        }

        [HttpGet("NumberOfPosts")]
        public object GetNumberOfPosts()
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = @"SELECT COUNT(*) FROM blogpost";

            var cmd = new MySqlCommand(sql, connector);

            var db = cmd.ExecuteReader();

            connector.Close();

            return new { message = $"Posztok száma : {db}" };
        }

        [HttpGet("BloggerPostsNumber")]
        public object BloggerPostsNumber()
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = $"SELECT `Name`, COUNT(*) FROM `blogger` INNER JOIN blogpost ON blogger.Id-blogpost.blogId GROUP BY blogger.Id HAVING `id` = 11;";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();

            if(datareader.Read() == true)
            {
                var bloggerpostsnumber = new
                {
                    Name = datareader.GetString[0],
                    NumberOfPosts = datareader.GetInt32(1),
                };
                return bloggerpostsnumber;
            }

            connector.Close();

            return null;
        }
    }
}
