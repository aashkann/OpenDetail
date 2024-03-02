using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SpeckleAutomateDotnetExample
{

    public enum DetailTopologyCategory
    {
        Point,
        Linear,
        Volume,
    }

    public enum ElementRole
    {
        LoadBearing,
        Insulation,
        InnerFinish,
        OuterFinish,
    }

    public enum MaterialCategory
    {
        Wood,
        Concrete,
        Steel,
        Gipsum,
    }

    // No conversion needed for MineralBuildingProducts enum as it's not defined.

    // Interfaces to Classes
    public class BuildupPart
    {
        public int Index { get; set; }
        public MaterialCategory MaterialCategory { get; set; }
        public double Thickness { get; set; }
        public ElementRole ElementRole { get; set; }
    }

    public class DetailObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? Id { get; set; }
        public string? URL { get; set; }

        public string? Keyimage { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DetailTopologyCategory TopologyCategory { get; set; }
        public List<ElementObject> Elements { get; set; } = new List<ElementObject>();

        [BsonExtraElements()]
        public Dictionary<string, object>? ExtraElements { get; set; }
    }

    public class DetailPart
    {
        public MaterialCategory MaterialCategory { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public ElementRole ElementRole { get; set; }
    }

    public class ElementObject
    {
        public List<BuildupPart> BuildupParts { get; set; } = new List<BuildupPart>();
        public List<DetailPart> DetailParts { get; set; } = new List<DetailPart>();
    }



}
