using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Orchard.Caching;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.MetaData.Services;
using Orchard.Core.Settings.Metadata.Records;
using Orchard.Data;
using Orchard.Logging;

namespace Orchard.Core.Settings.Metadata {
    public class ContentDefinitionManager : Component, IContentDefinitionManager {
        private const string ContentDefinitionSignal = "ContentDefinitionManager";
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;
        private readonly IRepository<ContentTypeRecord> _typeDefinitionRepository;
        private readonly IRepository<ContentPartRecord> _partDefinitionRepository;
        private readonly IRepository<ContentFieldRecord> _fieldDefinitionRepository;
        private readonly ISettingsFormatter _settingsFormatter;

        public ContentDefinitionManager(
            ICacheManager cacheManager,
            ISignals signals,
            IRepository<ContentTypeRecord> typeDefinitionRepository,
            IRepository<ContentPartRecord> partDefinitionRepository,
            IRepository<ContentFieldRecord> fieldDefinitionRepository,
            ISettingsFormatter settingsFormatter) {
            _cacheManager = cacheManager;
            _signals = signals;
            _typeDefinitionRepository = typeDefinitionRepository;
            _partDefinitionRepository = partDefinitionRepository;
            _fieldDefinitionRepository = fieldDefinitionRepository;
            _settingsFormatter = settingsFormatter;
        }

        public ContentTypeDefinition GetTypeDefinition(string name) {
            if (String.IsNullOrWhiteSpace(name)) {
                return null;
            }

            var contentTypeDefinitions = AcquireContentTypeDefinitions();
            if (contentTypeDefinitions.ContainsKey(name)) {
                return contentTypeDefinitions[name];
            }

            return null;
        }

        public void DeleteTypeDefinition(string name) {
            var record = _typeDefinitionRepository.Table.SingleOrDefault(x => x.Name == name);

            // deletes the content type record associated
            if (record != null) {
                _typeDefinitionRepository.Delete(record);
            }

            // invalidates the cache
            TriggerContentDefinitionSignal();
        }

        public void DeletePartDefinition(string name) {
            // remove parts from current types
            var typesWithPart = ListTypeDefinitions().Where(typeDefinition => typeDefinition.Parts.Any(part => part.PartDefinition.Name == name));

            foreach (var typeDefinition in typesWithPart) {
                this.AlterTypeDefinition(typeDefinition.Name, builder => builder.RemovePart(name));
            }

            // delete part
            var record = _partDefinitionRepository.Table.SingleOrDefault(x => x.Name == name);

            if (record != null) {
                _partDefinitionRepository.Delete(record);
            }

            // invalidates the cache
            TriggerContentDefinitionSignal();

        }

        public ContentPartDefinition GetPartDefinition(string name) {
            if (String.IsNullOrWhiteSpace(name)) {
                return null;
            }

            var contentPartDefinitions = AcquireContentPartDefinitions();
            if (contentPartDefinitions.ContainsKey(name)) {
                return contentPartDefinitions[name];
            }

            return null;
        }

        public IEnumerable<ContentTypeDefinition> ListTypeDefinitions() {
            return AcquireContentTypeDefinitions().Values;
        }

        public IEnumerable<ContentPartDefinition> ListPartDefinitions() {
            return AcquireContentPartDefinitions().Values;
        }

        public IEnumerable<ContentFieldDefinition> ListFieldDefinitions() {
            return AcquireContentFieldDefinitions().Values;
        }

        public void StoreTypeDefinition(ContentTypeDefinition contentTypeDefinition) {
            Apply(contentTypeDefinition, Acquire(contentTypeDefinition));
            TriggerContentDefinitionSignal();
        }

        public void StorePartDefinition(ContentPartDefinition contentPartDefinition) {
            Apply(contentPartDefinition, Acquire(contentPartDefinition));
            TriggerContentDefinitionSignal();
        }

        private void MonitorContentDefinitionSignal(AcquireContext<string> ctx) {
            ctx.Monitor(_signals.When(ContentDefinitionSignal));
        }

        private void TriggerContentDefinitionSignal() {
            _signals.Trigger(ContentDefinitionSignal);
        }

        private IDictionary<string, ContentTypeDefinition> AcquireContentTypeDefinitions() {
            return _cacheManager.Get("ContentTypeDefinitions", ctx => {
                MonitorContentDefinitionSignal(ctx);

                AcquireContentPartDefinitions();

                var contentTypeDefinitionRecords = _typeDefinitionRepository.Table
                    .FetchMany(x => x.ContentTypePartRecords)
                    .ThenFetch(x => x.ContentPartRecord)
                    .Select(Build);

                return contentTypeDefinitionRecords.ToDictionary(x => x.Name, y => y, StringComparer.OrdinalIgnoreCase);
            });
        }

        private IDictionary<string, ContentPartDefinition> AcquireContentPartDefinitions() {
            return _cacheManager.Get("ContentPartDefinitions", ctx => {
                MonitorContentDefinitionSignal(ctx);

                var contentPartDefinitionRecords = _partDefinitionRepository.Table
                    .FetchMany(x => x.ContentPartFieldRecords)
                    .ThenFetch(x => x.ContentFieldRecord)
                    .Select(Build);

                return contentPartDefinitionRecords.ToDictionary(x => x.Name, y => y, StringComparer.OrdinalIgnoreCase);
            });
        }

        private IDictionary<string, ContentFieldDefinition> AcquireContentFieldDefinitions() {
            return _cacheManager.Get("ContentFieldDefinitions", ctx => {
                MonitorContentDefinitionSignal(ctx);

                return _fieldDefinitionRepository.Table.Select(Build).ToDictionary(x => x.Name, y => y);
            });
        }

        private ContentTypeRecord Acquire(ContentTypeDefinition contentTypeDefinition) {
            var result = _typeDefinitionRepository.Table.SingleOrDefault(x => x.Name == contentTypeDefinition.Name);
            if (result == null) {
                result = new ContentTypeRecord { Name = contentTypeDefinition.Name, DisplayName = contentTypeDefinition.DisplayName };
                _typeDefinitionRepository.Create(result);
            }
            return result;
        }

        private ContentPartRecord Acquire(ContentPartDefinition contentPartDefinition) {
            var result = _partDefinitionRepository.Table.SingleOrDefault(x => x.Name == contentPartDefinition.Name);
            if (result == null) {
                result = new ContentPartRecord { Name = contentPartDefinition.Name };
                _partDefinitionRepository.Create(result);
            }
            return result;
        }

        private ContentFieldRecord Acquire(ContentFieldDefinition contentFieldDefinition) {
            var result = _fieldDefinitionRepository.Table.SingleOrDefault(x => x.Name == contentFieldDefinition.Name);
            if (result == null) {
                result = new ContentFieldRecord { Name = contentFieldDefinition.Name };
                _fieldDefinitionRepository.Create(result);
            }
            return result;
        }

        private void Apply(ContentTypeDefinition model, ContentTypeRecord record) {
            record.DisplayName = model.DisplayName;
            record.Settings = _settingsFormatter.Map(model.Settings).ToString();

            var toRemove = record.ContentTypePartRecords
                .Where(partDefinitionRecord => model.Parts.All(part => partDefinitionRecord.ContentPartRecord.Name != part.PartDefinition.Name))
                .ToList();

            foreach (var remove in toRemove) {
                record.ContentTypePartRecords.Remove(remove);
            }

            foreach (var part in model.Parts) {
                var partName = part.PartDefinition.Name;
                var typePartRecord = record.ContentTypePartRecords.SingleOrDefault(r => r.ContentPartRecord.Name == partName);
                if (typePartRecord == null) {
                    typePartRecord = new ContentTypePartRecord { ContentPartRecord = Acquire(part.PartDefinition) };
                    record.ContentTypePartRecords.Add(typePartRecord);
                }
                Apply(part, typePartRecord);
            }
        }

        private void Apply(ContentTypePartDefinition model, ContentTypePartRecord record) {
            record.Settings = Compose(_settingsFormatter.Map(model.Settings));
        }

        private void Apply(ContentPartDefinition model, ContentPartRecord record) {
            record.Settings = _settingsFormatter.Map(model.Settings).ToString();

            var toRemove = record.ContentPartFieldRecords
                .Where(partFieldDefinitionRecord => model.Fields.All(partField => partFieldDefinitionRecord.Name != partField.Name))
                .ToList();

            foreach (var remove in toRemove) {
                record.ContentPartFieldRecords.Remove(remove);
            }

            foreach (var field in model.Fields) {
                var fieldName = field.Name;
                var partFieldRecord = record.ContentPartFieldRecords.SingleOrDefault(r => r.Name == fieldName);
                if (partFieldRecord == null) {
                    partFieldRecord = new ContentPartFieldRecord {
                        ContentFieldRecord = Acquire(field.FieldDefinition),
                        Name = field.Name
                    };
                    record.ContentPartFieldRecords.Add(partFieldRecord);
                }
                Apply(field, partFieldRecord);
            }
        }

        private void Apply(ContentPartFieldDefinition model, ContentPartFieldRecord record) {
            record.Settings = Compose(_settingsFormatter.Map(model.Settings));
        }

        ContentTypeDefinition Build(ContentTypeRecord source) {
            return new ContentTypeDefinition(
                source.Name,
                source.DisplayName,
                source.ContentTypePartRecords.Select(Build),
                _settingsFormatter.Map(Parse(source.Settings)));
        }

        ContentTypePartDefinition Build(ContentTypePartRecord source) {
            return new ContentTypePartDefinition(
                Build(source.ContentPartRecord),
                _settingsFormatter.Map(Parse(source.Settings)));
        }

        ContentPartDefinition Build(ContentPartRecord source) {
            return new ContentPartDefinition(
                source.Name,
                source.ContentPartFieldRecords.Select(Build),
                _settingsFormatter.Map(Parse(source.Settings)));
        }

        ContentPartFieldDefinition Build(ContentPartFieldRecord source) {
            return new ContentPartFieldDefinition(
                Build(source.ContentFieldRecord),
                source.Name,
                _settingsFormatter.Map(Parse(source.Settings)));
        }

        ContentFieldDefinition Build(ContentFieldRecord source) {
            return new ContentFieldDefinition(source.Name);
        }

        XElement Parse(string settings) {
            if (string.IsNullOrEmpty(settings))
                return null;

            try {
                return XElement.Parse(settings);
            }
            catch (Exception ex) {
                Logger.Error(ex, "Unable to parse settings xml");
                return null;
            }
        }

        static string Compose(XElement map) {
            if (map == null)
                return null;

            return map.ToString();
        }
    }

}
