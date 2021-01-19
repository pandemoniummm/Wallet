using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wallet.Model;
using Newtonsoft.Json;
using System.IO;

namespace Wallet.Data
{
    public class FileUserRepository : IUserRepository
    {
        private readonly string pathFile;
        public FileUserRepository(string pathFile)
        {
            this.pathFile = pathFile;
        }

        private async Task<IList<User>> GetListFromFile()
        {
            var collection = await Task.Run(() => JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(pathFile)));

            if(collection == null)
            {
                return new List<User>();
            }

            return collection;
        }

        private async Task WriteToFIle(IList<User> collection)
        {
            await Task.Run(() => File.WriteAllText(pathFile, JsonConvert.SerializeObject(collection)));
        }

        public async Task Create(User item)
        {
            var collection = await GetListFromFile();
            
            collection.Add(item);
            await WriteToFIle(collection);
        }

        public async Task Delete(int id)
        {           
            var collection = await GetListFromFile();
            collection.Remove(collection.FirstOrDefault(x => x.Id == id));
            await WriteToFIle(collection);                       
        }
        public async Task<IList<User>> GetList()
        {
            return await GetListFromFile();
        }

        public async Task<User> GetSingle(int id)
        {
            var collection = await GetListFromFile();
            return collection.FirstOrDefault(x => x.Id == id);
        }

        public async Task Update(User item)
        {
            var collection = await GetListFromFile();
            collection.Remove(collection.FirstOrDefault(x => x.Id == item.Id));
            collection.Add(item);
            await WriteToFIle(collection);
        }
    }
}
