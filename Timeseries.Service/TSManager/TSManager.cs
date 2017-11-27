using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TSManager
{
    public class TsManager
    {
        private readonly IDataAdaptor.IDataAdaptor _dataAdaptor;

        public bool CheckConnection()
        {
            return _dataAdaptor.CheckConnection();
        }
        public async Task<bool> DeleteTag(string tagName)
        {
            await _dataAdaptor.DeleteCollection(tagName);
          return true;
        }
        public TsManager()
        {
            try
            {
                _dataAdaptor = new DataAdaptorAzureCosmosDb.DataAdaptorAzureCosmosDb();
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
           
        }
       
        public async Task<List<string>> ParseJson(object jsonvalue)
        {
            List<string> assetsCreatedList = new List<string>();
            var serialized = JsonConvert.SerializeObject(jsonvalue);
            JToken entireJson = JToken.Parse(serialized);

            var collectionLink = string.Empty;

            foreach (var item in entireJson)
            {
                if (item is JProperty)
                {
                    var key = (item as JProperty).Name;
                    var value = (item as JProperty).Value.ToString();
                    if (key == "TagName")
                    {
                        string[] tokens = value.Split('/').Where(x => !string.IsNullOrEmpty(x)).ToArray();
                        var baseCollectionName = tokens[0];
                        collectionLink = await _dataAdaptor.CreateCollection(baseCollectionName);
                        await _dataAdaptor.CreateDocument(collectionLink, jsonvalue);
                    }
                }
                else
                {
                    var x = item.Value<JObject>();
                    foreach (var y in x)
                    {
                        var key = y.Key;
                        var value = y.Value;
                        if (key == "TagName")
                        {
                            string[] tokens = value.ToString().Split('/').Where(x1 => !string.IsNullOrEmpty(x1))
                                .ToArray();
                            var baseCollectionName = tokens[0];
                            collectionLink = await _dataAdaptor.CreateCollection(baseCollectionName);
                            await _dataAdaptor.CreateDocument(collectionLink, item);
                        }


                    }
                }
            }
            return assetsCreatedList;
        }

        public async Task<object> GetDocumentsCount(string collectionName = "")
        {
            dynamic jsonObject = new JObject();
            if (collectionName != "")
            {
                var result = _dataAdaptor.GetCollectionDataCount(collectionName);
                jsonObject.TagName = collectionName;
                jsonObject.Count = result;
                return jsonObject;
            }
            else
            {
                var tags = await _dataAdaptor.GetCollections();
                jsonObject = new List<JObject>();
                foreach (var tagName in (List<string>)tags)
                {
                    var result = await _dataAdaptor.GetCollectionDataCount(tagName);
                    dynamic _obj = new JObject();
                    _obj.TagName = tagName;
                    _obj.Count = result;
                    jsonObject.Add(_obj);
                }
            }

            return jsonObject;
        }

        public async Task<object>  GetAllTags()
        {
           return await _dataAdaptor.GetCollections();
        }

        public async Task<IList<object>> GetDataPoints(TsQueryModel query)
        {
            var _tag = query.Tags;
            var start = query.Start;
            var end = query.End;
            var order = query.Tags.Order;
            if(start == null & end==null)
            {
              return  await GetDocuments(_tag.Name, _tag.limit);
            }
            return await GetDocuments(_tag.Name, start, end, _tag.limit);
        }
        private async Task<IList<object>> GetDocuments(string[] collectionName,string start,string end,int limit)
        {
            IList<object> resultList = new List<object>();
            foreach (var name in collectionName)
            {
                resultList.Add(await _dataAdaptor.GetCollectionDocuments(name,start,end,limit));
            }
            return resultList;

        }

        private async Task<IList<object>> GetDocuments(string[] collectionName, string start, string end, int limit,string order)
        {
            IList<object> resultList = new List<object>();
            foreach (var name in collectionName)
            {
                resultList.Add(await _dataAdaptor.GetCollectionDocuments(name, start, end, limit));
            }
            return resultList;

        }
        private async  Task<IList<object>> GetDocuments(string[] collectionName,int limit = 0)
        {
            IList<object> resultList = new List<object>();
            foreach (var name in collectionName)
            {
               resultList.Add(await _dataAdaptor.GetCollectionDocuments(name,limit));
            }
            return resultList;

        }
    }
}
