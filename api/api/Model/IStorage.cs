namespace api.Model
{
    public interface IStorage
    {
        Document Load(string fileName);
        void Save(Document document, string fileName);
    }
}
