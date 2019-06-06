using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HtmlToImageDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageBankApiController : ControllerBase
    {
        private const string BaseDir = "wwwroot";
        private const string DirName = "images";
        private const string ImageName = "image.png";

        [HttpGet]
        public IActionResult Get()
        {
            var fileName = Path.Combine(Directory.GetCurrentDirectory(), BaseDir, DirName, ImageName);
            if (System.IO.File.Exists(fileName))
            {
                var url = $"{HttpContext.Request.Host}/{DirName}/{ImageName}";
                
                return Ok(url);
            }
            else
            {
                return BadRequest("No image is currently stored.");
            }
        }

        [HttpPost]
        public IActionResult Post(BinaryImageModel binaryImageModel)
        {
            try
            {
                var res = SaveImage(binaryImageModel.Data);

                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private bool SaveImage(string imageData)
        {            
            var directory = Path.Combine(Directory.GetCurrentDirectory(), BaseDir, DirName);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var imageStream = ConvertBase64ToImage(imageData);

            using (Stream file = System.IO.File.Create(Path.Combine(directory, ImageName)))
            {
                CopyStream(imageStream, file);

                imageStream.Close();
                file.Close();
            }

            return true;
        }

        private Stream ConvertBase64ToImage(string base64String)
        {
            byte[] data = Convert.FromBase64String(base64String);
            return new MemoryStream(data, 0, data.Length);
        }

        private void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }
    }

    public class BinaryImageModel
    {
        private string _data;
        public string Data
        {
            get
            {
                return _data;
            }
            set { _data = value.Substring(value.IndexOf(',') + 1); }
        }
    }
}
