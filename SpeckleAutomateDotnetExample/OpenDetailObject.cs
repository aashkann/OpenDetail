using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SpeckleAutomateDotnetExample
{

        public class OpenDetailObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? Id { get; set; }

        
        public string Name { get; set; } = null!;

        public string? URL { get; set; }

        public string Category { get; set; } = null!;

        public string Author { get; set; } = null!;

        [BsonExtraElements()]
        public Dictionary<string, object>? ExtraElements { get; set; }

    }


}
