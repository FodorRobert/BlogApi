using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Models.DTOs
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateBloggerDTOs : ControllerBase
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public int Age { get; set; }

        public string? Password { get; set; }
    }
}
