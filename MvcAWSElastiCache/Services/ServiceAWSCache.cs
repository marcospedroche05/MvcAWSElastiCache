using MvcAWSElastiCache.Helpers;
using MvcAWSElastiCache.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MvcAWSElastiCache.Services
{
    public class ServiceAWSCache
    {
        private IDatabase cache;

        public ServiceAWSCache()
        {
            this.cache = HelperCache.Connection.GetDatabase();
        }

        public async Task<List<Coche>> GetCochesAsync()
        {
            string jsonCoches = await this.cache.StringGetAsync("favoritos");
            if (jsonCoches == null)
            {
                return null;
            }
            else
            {
                List<Coche> coches = JsonConvert.DeserializeObject
                    <List<Coche>>(jsonCoches);
                return coches;
            }
        }

        public async Task AddFavoritoAsync(Coche car)
        {
            List<Coche> coches = await this.GetCochesAsync();
            if (coches == null)
            {
                coches = new List<Coche>();
            }
            coches.Add(car);
            string json = JsonConvert.SerializeObject(coches);
            await this.cache.StringSetAsync("favoritos", json, TimeSpan.FromMinutes(30));
        }

        public async Task DeleteCocheAsync(int idCoche)
        {
            List<Coche> coches = await this.GetCochesAsync();
            if (coches != null)
            {
                Coche carDelete = coches.FirstOrDefault(x => x.IdCoche == idCoche);
                coches.Remove(carDelete);
                if (coches.Count == 0)
                {
                    await this.cache.KeyDeleteAsync("favoritos");
                }
                else
                {
                    string jsonCoches = JsonConvert.SerializeObject(coches);
                    await this.cache.StringSetAsync("favoritos", jsonCoches,
                        TimeSpan.FromMinutes(30));
                }
            }
        }
    }
}
