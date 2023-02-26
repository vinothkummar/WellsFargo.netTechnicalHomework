using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wells.Fargo.Application;

namespace Wells.Fargo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        const String folderName = "Files";
        readonly String folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        readonly ILogger _logger;

        public FileController(ILogger<FileController> logger)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            _logger = logger;
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadDocument([FromForm(Name = "csvFile")] IFormFile csvFile)
        {
            using (var fileContentStream = new MemoryStream())
            {
                await csvFile.CopyToAsync(fileContentStream);
                await System.IO.File.WriteAllBytesAsync(Path.Combine(folderPath, csvFile.FileName), fileContentStream.ToArray());
            }
            return CreatedAtRoute(routeName: "csvFile", routeValues: new { filename = csvFile.FileName }, value: null); ;
        }

        [HttpGet("DownloadFile{filename}", Name = "csvFile")]
        public async Task<IActionResult> Get([FromRoute] String filename)
        {
            var filePath = Path.Combine(folderPath, filename);
            if (System.IO.File.Exists(filePath))
            {
                return File(await System.IO.File.ReadAllBytesAsync(filePath), "application/octet-stream", filename);
            }
            return NotFound();
        }
       
    }
}
