using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingADO.DAL;
using DatingADO.Model;
using Microsoft.AspNetCore.Mvc;

namespace DatingADO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ValuesDAL _dal;

        public ValuesController(ValuesDAL dal)
        {
            _dal = dal;
        }
        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<Values>> Get()
        {
            return await _dal.GetAll();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var value = await _dal.GetValueById(id);
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post(Values value)
        {
            value.Id= await _dal.AddValue(value);

            return Ok(value);
        }

        //post multi rows api/values/multi
        [HttpPost("multi")]
        public async Task<IActionResult> PostMulti(List<Values> values)
        {
            await _dal.AddMultiValues(values);
            return Ok();
        }

        // PUT api/values/5
        [HttpPut]
        public async Task Put(Values value)
        {
            await _dal.UpdateValue(value);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _dal.DeleteData(id);
        }
    }
}
