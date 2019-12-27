using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.DAL;
using DatingApp.model;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
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
            List<Values> data = await _dal.GetAllValues();
            return data;
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<Values> Get(int id)
        {
            return await _dal.GetValueById(id);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post(Values value)
        {
            value = await _dal.AddValue(value);

            return Ok(value);
        }

        //for multi-values
        [HttpPost("multi")]
        public async Task<IActionResult> PostMulti(List<Values> value)
        {
            value = await _dal.AddMultiValues(value);

            return Ok(value);
        }


        // PUT api/values/5
        [HttpPut]
        public async Task<IActionResult> Put(Values value)
        {
            value = await _dal.UpdateValue(value);

            return Ok(value);

        }

        //put api/values/multi
        [HttpPut("multi")]
        public async Task<IActionResult> PutMulti(List<Values> value)
        {
            value = await _dal.UpdateMultiValues(value);
            return Ok(value);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _dal.DeleteValue(id);
            return Ok();
        }
    }
}
