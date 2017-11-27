using System.Threading.Tasks;

namespace IDataAdaptor
{
    public interface IDataAdaptor
    {
        bool InitDb();

        bool CheckConnection();

        Task<object> GetCollections();

        object GetDocumentsCountInDb();

        Task<string> CreateCollection(string name);

        Task<bool> DeleteCollection(string name);

        object RetrieveCollectionDetails(string name);

        Task<string> GetCollectionLink(string name);

        Task<int> GetCollectionDataCount(string name);

        Task<object> GetCollectionDocuments(string name, string start, string end, int limit);

        Task<object> GetCollectionDocuments(string name,int limit =0);

        Task<bool> CreateDocument(string collectionName, object data);

        bool DeleteDocument(string collectionName, string id);

        bool GetDocumentLink(string collectionName, string id);

        object GetDocumentDetails(string collectionName, string id);
    }
}