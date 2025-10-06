using Microsoft.AspNetCore.Mvc;
using SecShare.Base.Document;
using SecShare.Core.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecShare.DocumentAPI.Controllers;
[Route("api/document")]
[ApiController]
public class DocumentController : ControllerBase
{
    private readonly IDocumentAPIService _documentAPIService;
    public DocumentController(IDocumentAPIService documentAPIService)
    {
        _documentAPIService = documentAPIService;
    }
    // GET: api/<DocumentController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<DocumentController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<DocumentController>
    [HttpPost]
    public async Task<IActionResult> UploadMyFile([FromForm] UploadMyFileDto uploadMyFileDto)
    {
        var UserId = User.Identity.Name;

    }

    // PUT api/<DocumentController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<DocumentController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
