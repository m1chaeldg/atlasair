using System;
using System.Collections.Generic;

namespace api.Model
{

    public class FileDb : IFileDb
    {
        private readonly string filedbName;
        private readonly LockService lockService = new LockService();
        private IStorage storage;

        public Document Documents { get; set; }

        public FileDb()
            : this(new Storage())
        {

        }
        public FileDb(IStorage storage)
            : this(storage, "records.json")
        {

        }
        public FileDb(IStorage storage, string filedbName)
        {
            this.filedbName = filedbName;
            this.storage = storage;
        }

        // safe or not?
        public void Initialize()
        {
            using (lockService.LockRead())
                Documents = storage.Load(filedbName);
        }

        public void Insert(Product item)
        {
            using (lockService.LockWrite())
            {
                item.Id = Guid.NewGuid().ToString();

                Documents[item.Id] = item;

                storage.Save(Documents, filedbName);
            }
        }

        public void Delete(string id)
        {
            using (lockService.LockWrite())
            {
                if (Documents.ContainsKey(id))
                {
                    Documents.Remove(id);

                    storage.Save(Documents, filedbName);
                }
            }
        }

        public Product Get(string id)
        {
            using (lockService.LockRead())
            {
                Documents.TryGetValue(id, out Product prod);
                return prod;
            }
        }

        public IReadOnlyList<Product> GetAll()
        {
            List<Product> list;
            using (lockService.LockRead())
            {
                list = new List<Product>(Documents.Values);
            }

            return list.AsReadOnly();
        }

        public void Update(Product item)
        {
            using (lockService.LockWrite())
            {
                Documents[item.Id] = item;

                storage.Save(Documents, filedbName);
            }
        }

        public bool Exist(string id)
        {
            return Documents.ContainsKey(id);
        }
    }
}
