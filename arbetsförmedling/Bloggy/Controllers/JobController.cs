using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using JobApi.Models;
using Newtonsoft.Json.Linq;

namespace JobApi.Controllers
{
    [Route("api/Job")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly JobContext _context;

        public JobController(JobContext context)
        {
            _context = context;

            if (_context.JobItems.Count() == 0)
            {
                
                string apiUrl = "https://jobsearch.api.jobtechdev.se/search?q=hudiksvall&offset=0&limit=100&sort=pubdate-desc";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "GET"; //använder get metod
                request.Headers.Add("api-key", "INSERT_API_KEY_HERE"); //egen nyckel för api

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //om statusen är okej och inge fel uppstod kommer den fortsätta lägga in information i databasen
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    }

                    var data = readStream.ReadToEnd();

                    var jObj = JObject.Parse(data);
                    //int size = jObj["total"]["value"].ToObject<int>();                  
                    
                    for (int x = 0; x < 100 ; x++)
                    {
                         
                        var title = jObj["hits"][x]["headline"].ToString();
                        var text = jObj["hits"][x]["description"]["text"].ToString();
                        var dateEnd = DateTime.Parse(jObj["hits"][x]["application_deadline"].ToString());
                        var place = jObj["hits"][x]["workplace_address"]["municipality"].ToString();
                        dateEnd.ToString("yyyy-MM-dd");
                        
                        _context.JobItems.Add(new JobItem { Title = title, Text = text, Place = place, DateEnd = dateEnd });
                    }
                                        
                    _context.SaveChanges();

                    //stänger anslutning till api
                    response.Close();
                    readStream.Close();
                }

                
            }
        }
        // GET: api/Job
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobItem>>> GetJobItems()
        {
            return await _context.JobItems.ToListAsync();
        }

        // GET: api/Job/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobItem>> GetJobItem(int id)
        {
            var jobItem = await _context.JobItems.FindAsync(id);

            if (jobItem == null)
            {
                return NotFound();
            }

            return jobItem;
        }

    }
}
