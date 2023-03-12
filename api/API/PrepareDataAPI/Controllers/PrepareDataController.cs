using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrepareDataAPI.DBService;
using System.Collections.Generic;

namespace PrepareDataAPI.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class PrepareDataController : Controller
    {
        public myDBConnect _context { get; }
        public cassandraDBConnect _cassandraContext { get; }
        public PrepareDataController(myDBConnect context, cassandraDBConnect cassandraContext)
        {
            _context = context;
            _cassandraContext = cassandraContext;
        }

        // GET: api/Preparedata/resourceLevel
        [EnableCors("_myAllowSpecificOrigins")]
        [HttpGet("{data}")]
        public async Task<ActionResult<List<dynamic>>> GetResourceLevel()
        {
            var query = new myDBQuery(_context, _cassandraContext);
            return new ActionResult<List<dynamic>>(await query.GetResourceLevelInfoAsync());
        }

        // POST: api/Preparedata
        // should called once to create levelinfo table in MySQL
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [EnableCors("_myAllowSpecificOrigins")]
        [HttpPost]
        public async Task PostPrepareData()
        {
            var query = new myDBQuery(_context, _cassandraContext);
            await query.CreateLevelInfoAsync();
        }

        // PUT: api/Preparedata
        // should called once to create azurecost table in cassandra
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [EnableCors("_myAllowSpecificOrigins")]
        [HttpPut]
        public async Task CreateAzureCost()
        {
            var query = new myDBQuery(_context, _cassandraContext);
            await query.CreateAzureCostAsync();
        }

        // GET: api/Preparedata
        // to protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [EnableCors("_myAllowSpecificOrigins")]
        [HttpGet]
        public async Task<ActionResult<List<dynamic>>> GetAzureCost()
        {
            var query = new myDBQuery(_context, _cassandraContext);
            return new ActionResult<List<dynamic>>(await query.SelectAzureCostAsync());
        }
    }
}
