using Configuration.Common.Models;
using Configuration.Data.Abstractions.Interfaces;
using Configuration.Data.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Configuration.Data.MongoDB.Repositories
{
    public class ConfigurationStorage : IConfigurationStorage
    {
        private readonly ConfigurationDatabase _configurationDatabase;
        private readonly IMongoCollection<ConfigModel> _configurations;

        public ConfigurationStorage(IOptions<AppSettings> appSettings)
        {
            _configurationDatabase = appSettings.Value.ConfigurationDatabase ?? throw new ArgumentNullException(nameof(appSettings));

            var mongoClient = new MongoClient(_configurationDatabase.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(_configurationDatabase.DatabaseName);

            _configurations = mongoDatabase.GetCollection<ConfigModel>(_configurationDatabase.ConfigurationCollectionName);
        }

        public async Task<ConfigModel> GetAsync(string id)
        {
            var config = await _configurations.Find(c => c.Id == id).FirstOrDefaultAsync();
            return config;
        }

        public async Task<List<ConfigModel>> GetAllByApplicationNameAsync(string applicationName)
        {
            var config = await _configurations.Find(c => c.IsActive == true && c.ApplicationName == applicationName).ToListAsync();
            return config;
        }

        public async Task<List<ConfigModel>> GetAllAsync()
        {
            return await _configurations.Find(_ => true).ToListAsync();
        }

        public async Task<ConfigModel> AddOrUpdateAsync(ConfigModel model)
        {
            var existingConfig = await _configurations.Find(c => c.Id == model.Id).FirstOrDefaultAsync();
            if (existingConfig != null)
            {
                await _configurations.ReplaceOneAsync(x => x.Id == model.Id, model);
            }
            else
            {
                await _configurations.InsertOneAsync(model);
            }

            return model;
        }

        public async Task<bool> DeleteAsync(ConfigModel model)
        {
            var result = await _configurations.DeleteOneAsync(x => x.Id == model.Id);
            return result.DeletedCount == 1;
        }

        public void Seed()
        {
            var hasAnyRecord = _configurations.Find(_ => true).ToList();

            if (hasAnyRecord.Count == 0)
            {
                _configurations.InsertMany(new List<ConfigModel>
                {
                    new() { Id = Guid.NewGuid().ToString(), Name = "SiteName", Type = typeof(string).FullName, Value = "www.beymen.com", IsActive = true, ApplicationName = "SERVICE-A" },
                    new() { Id = Guid.NewGuid().ToString(), Name = "IsBasketEnabled", Type = typeof(bool).FullName, Value = "true", IsActive = true, ApplicationName = "SERVICE-A" },
                    new() { Id = Guid.NewGuid().ToString(), Name = "ItemCount", Type = typeof(int).FullName, Value = "50", IsActive = true, ApplicationName = "SERVICE-A" },

                    new() { Id = Guid.NewGuid().ToString(), Name = "SiteName", Type = typeof(string).FullName, Value = "www.beymenclub.com", IsActive = true, ApplicationName = "SERVICE-B" },
                    new() { Id = Guid.NewGuid().ToString(), Name = "ApiKey", Type = typeof(string).FullName, Value = "123-456", IsActive = false, ApplicationName = "SERVICE-B" },
                    new() { Id = Guid.NewGuid().ToString(), Name = "Rate", Type = typeof(double).FullName, Value = "1,18", IsActive = true, ApplicationName = "SERVICE-B" },

                    new() { Id = Guid.NewGuid().ToString(), Name = "SiteName", Type = typeof(string).FullName, Value = "www.beymengroup.com", IsActive = true, ApplicationName = "SERVICE-C" },
                });
            }
        }
    }
}