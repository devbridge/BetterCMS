using System.Collections.Generic;
using System.Web.Http;

namespace BetterCms.Module.Users.Api.Controllers
{
    [Route("api/users")]
    public class UsersController : ApiController
    {
        // GET api/<controller>
        [Route("")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [Route("{id:guid}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [Route("")]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [Route("")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [Route("")]
        public void Delete(int id)
        {
        }
    }
}