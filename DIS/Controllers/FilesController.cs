using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
//using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DIS.Models
{
    [Route("api/v1/[controller]")]
    //[ApiController]
    public class FilesController : ControllerBase
    {
        public static IConfiguration _configuration;
        public FilesController(IConfiguration configuration)
        {
            _configuration = configuration;


        }
        // GET api/files/AllFiles
        //[HttpGet("AllFiles")]
        //string allfiles = _configuration.GetValue("APIEndpoint:allfiles");
        [HttpGet("allfiles")]
        public IActionResult Get()
        {
            var db = new DIS.Models.DataAccess(_configuration);
            var file = db.GetAllFiles();
            //var filesDAO = new AllFilesDAO();
            //filesDAO = Mapper.Map<AllFilesDAO,Files>(file);

            var file_list = file.ToList();
            return Ok(file_list);
        }

        // GET api/files/GetFilesByFilename
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var request_time = DateTime.Now;
            var db = new DIS.Models.DataAccess(_configuration);
            var file = db.GetFile(id);
            if (file == null){
                var notfound = new NotFound
                {
                    FileName = id,
                    RequestedAt = request_time,
                    Error = "Requested file has not been integrated."
                };
                return (IActionResult)NotFound(notfound);
            }
            else{
                return (IActionResult)Ok(file);
            }

        }

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
