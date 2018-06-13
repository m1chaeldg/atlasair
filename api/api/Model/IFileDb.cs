using System.Collections.Generic;

namespace api.Model
{
    public interface IFileDb
    {
        bool Exist(string id);

        void Insert(Product item);
        void Delete(string id);
        Product Get(string id);
        void Update(Product item);
        IReadOnlyList<Product> GetAll();
    }
}
