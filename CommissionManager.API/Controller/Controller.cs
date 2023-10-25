using CommissionManager.Data;
using Microsoft.AspNetCore.Mvc;

namespace CommissionManager.API.Controller
{

    [Route("api[controller]")]
    [ApiController]
    public class Controller : ControllerBase
    {
        private AppDbContext _dbcontext;

        public Controller(AppDbContext dbContext) 
        {
            _dbcontext = dbContext;
        }

        /*
        [HttpPut("item")]
        public ActionResult<Item> Add(Item item)
        {

        }
        */
    }
}
