using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Infrastructure.Data;
using MongoDB.Driver;

namespace DDD.Domain.DomainEvents
{
    public class SettingsEvent : IDomainEvent
    {
        public required string Settings { get; set; }

        private readonly IMongoCollection<SettingsEntity> _settingsCollection;

        public SettingsEvent(IMongoCollection<SettingsEntity> settingsCollection)
        {
            _settingsCollection = settingsCollection;
        }

        public void Handle()
        {
            var filter = Builders<SettingsEntity>.Filter.Empty;
            var update = Builders<SettingsEntity>.Update.Set(s => s.Value, this.Settings);

            _settingsCollection.UpdateOne(filter, update);
        }
    }
}
