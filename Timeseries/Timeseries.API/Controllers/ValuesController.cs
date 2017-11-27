using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TSManager;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Timeseries.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly TsManager _tsManager = new TsManager();

        [HttpGet]
        [Route("CheckConnection")]
        public bool GetConnection()
        {
            return _tsManager.CheckConnection();
        }


        [HttpGet]
        [Route("Tags")]
        public async Task<object> Tags()
        {
            return await _tsManager.GetAllTags();
        }
        [HttpGet]
        [Route("Tags/DataCount")]
        public async Task<object> TagsDataCount()
        {
          var result = await _tsManager.GetDocumentsCount();
            return result;
        }


        [HttpPost]
        [Route("Ingest")]
        public async void PostIngest([FromBody]object value)
        {
            await _tsManager.ParseJson(value);
        }

        [HttpPost]
        [Route("Tag/Delete")]
        public async void PostTagDelete(string tagname)
        {
            await _tsManager.DeleteTag(tagname);
        }

        [HttpPost]
        [Route("Datapoints")]
        public async Task<IList<object>> PostDatapoints([FromBody]object query)
        {
            var result = JsonConvert.DeserializeObject<TsQueryModel>(query.ToString());
           return  await _tsManager.GetDataPoints(result);
        }
    }
}
