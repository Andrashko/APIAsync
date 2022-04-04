using ExampleAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ExampleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly EPROJECTSWEBAPIEXAMPLEAPIEXAMPLEMDFContext _dbcontext;

        private readonly IHttpContextAccessor _context;

        private readonly StaticSettings _staticSettings;

        private List<string> allowedExtentions = new List<string>()
        {
            ".jpg",
            ".png"
        };

        public UploadController(EPROJECTSWEBAPIEXAMPLEAPIEXAMPLEMDFContext dbcontext, IOptions<StaticSettings> staticSettings , IHttpContextAccessor context)
        {
            _dbcontext = dbcontext;
            _context = context;
            _staticSettings = staticSettings.Value;
        }


        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            var file = Request.Form.Files[0];
            var path = Path.Combine(Directory.GetCurrentDirectory(), _staticSettings.Path);
            if (file.Length == 0)
                return BadRequest("Empty file");
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var extention = Path.GetExtension(fileName);
            if (!allowedExtentions.Contains(extention))
                return BadRequest("Bad extention");

            string serverName = $"{DateTime.Now.Ticks}{extention}";
            var fullPath = Path.Combine(path, serverName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                var request = _context.HttpContext.Request;
                var dbFile = new Files() { 
                    Name = fileName,
                    Url = $"{request.Scheme}://{request.Host}/{_staticSettings.Path}/{serverName}"
                };
                _dbcontext.Files.Add(dbFile);
                await _dbcontext.SaveChangesAsync();
                return Ok(dbFile);
            }
        }
    }
}
