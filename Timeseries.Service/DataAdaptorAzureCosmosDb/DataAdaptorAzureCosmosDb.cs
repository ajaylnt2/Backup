using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAdaptorAzureCosmosDb
{
    public class DataAdaptorAzureCosmosDb : IDataAdaptor.IDataAdaptor
    {
        private const string DatabaseName = "TimeSeriesData";
        private const string EndpointUrl = "https://tsservice.documents.azure.com:443/";

        private const string PrimaryKey = "ixUNu9piv6Rh5dEfpqikLV7GHjyNQEAVuWtLFJS1krknQLgNKJqq2n6DeQB3DA7Inleh9U2Xll7e2V47VHY2dw==";

        private readonly DocumentClient _client;

        public DataAdaptorAzureCosmosDb()
        {
            _client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
            if (!InitDb())
            {
                throw new Exception("Database not initialized");
            }
        }

        public bool CheckConnection()
        {
            if (_client != null && _client.Session != null)
                return true;
            return false;
        }

        public async Task<string> CreateCollection(string name)
        {
            try
            {
                var result =
               await _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DatabaseName),
                   new DocumentCollection { Id = name });
                return result.Resource.SelfLink;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CreateDocument(string collectionName, object data)
        {
            try
            {
                Document created = await _client.CreateDocumentAsync(collectionName, data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public async Task<bool> DeleteCollection(string name)
        {
            try
            {
                var selfLink = await GetCollectionLink(name);
                var response = await _client.DeleteDocumentCollectionAsync(selfLink);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool DeleteDocument(string collectionName, string id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetCollectionDataCount(string name)
        {
            try
            {
                    var collectionSelfLink = await GetCollectionLink(name);
                int res =   await Task.FromResult(_client.CreateDocumentQuery(collectionSelfLink, "SELECT c.id FROM c").ToList().Count);
                return res;
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> GetCollectionDocuments(string name,int limit =0)
        {
            try
            {
                var collectionSelfLink = await GetCollectionLink(name);
                return await ReadDocumentFeed(collectionSelfLink,limit);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> GetCollectionDocuments(string name, string start, string end, int limit)
        {
            List<dynamic> _documents = new List<dynamic>();
            try
            {
                var collectionSelfLink = await GetCollectionLink(name);
                var sql = $"SELECT TOP {limit} * FROM c WHERE c.TimeStamp BETWEEN {start} AND {end}";
                var query = _client
                   .CreateDocumentQuery(collectionSelfLink, sql)
                   .AsDocumentQuery();

                while (query.HasMoreResults)
                {
                    var documents = await query.ExecuteNextAsync();

                    foreach (var doc in documents)
                    {
                        _documents.Add(doc);
                    }
                }
                return JsonConvert.SerializeObject(_documents);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetCollectionLink(string name)
        {
            try
            {
                return await CreateCollection(name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> GetCollections()
        {
            try
            {
                var colls = await _client.ReadDocumentCollectionFeedAsync(UriFactory.CreateDatabaseUri(DatabaseName));
                return colls.Select(x => x.Id).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object GetDocumentDetails(string collectionName, string id)
        {
            throw new NotImplementedException();
        }

        public bool GetDocumentLink(string collectionName, string id)
        {
            throw new NotImplementedException();
        }

        public object GetDocumentsCountInDb()
        {
            throw new NotImplementedException();
        }

        public bool InitDb()
        {
            try
            {
                GetDbInit().Wait();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object RetrieveCollectionDetails(string name)
        {
            throw new NotImplementedException();
        }

        private string GenerateQueryString(string collectionName, string property, string value)
        {
            return $"SELECT*FROM{collectionName}f WHERE f.{property}={value}";
        }

        private async Task GetDbInit()
        {
            try
            {
                await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = DatabaseName });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Uri GetDocumentUri(string databaseId, string collectionId, string documentId)
        {
            try
            {
                return UriFactory.CreateDocumentUri(databaseId, collectionId, documentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<string> ReadDocumentFeed(string collectionLink,int limit =0)
        {
            try
            {
                var _limit = (limit == 0) ? 1000 : limit;
                var result = await _client.ReadDocumentFeedAsync(collectionLink, new FeedOptions { MaxItemCount = _limit });
                List<Document> documents = new List<Document>();
                foreach (Document doc in result)
                {
                    documents.Add(doc);
                }
                return JsonConvert.SerializeObject(documents);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}