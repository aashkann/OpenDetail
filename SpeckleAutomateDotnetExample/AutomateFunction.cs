using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using MongoDB.Bson;
using MongoDB.Driver;
using Objects;
using Objects.Geometry;
using Speckle.Automate.Sdk;
using Speckle.Automate.Sdk.Schema;
using Speckle.Core.Logging;
using Speckle.Core.Models.Extensions;
using SpeckleAutomateDotnetExample;

public static class AutomateFunction
{
  public static async Task Run(
    AutomationContext automationContext,
    FunctionInputs functionInputs
  )
  {

        // Connect to MongoDB
        const string connectionUri = "mongodb+srv://aechack2024:aechack2024@opendetail.e8ia6ef.mongodb.net/?retryWrites=true&w=majority&appName=OpenDetail";
        var settings = MongoClientSettings.FromConnectionString(connectionUri);
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        var client = new MongoClient(settings);

        var database = client.GetDatabase("opendetail");
        var collection = database.GetCollection<BsonDocument>("details");

        Console.WriteLine("Starting execution");
    _ = typeof(ObjectsKit).Assembly; // INFO: Force objects kit to initialize

    Console.WriteLine("Receiving version");
    var commitObject = await automationContext.ReceiveVersion();

    Console.WriteLine("Received version: " + commitObject);

        var openDetailObject = new OpenDetailObject();
        
    var count = commitObject
      .Flatten()
      .Count(b => b.speckle_type == functionInputs.SpeckleTypeToCount);
        var speckleURL = commitObject.Flatten();
        openDetailObject.URL = automationContext.SpeckleClient.ServerUrl;

        var bsonDocument = openDetailObject.ToBsonDocument();

        // Insert the BSON document into the collection
        collection.InsertOne(bsonDocument);

        Console.WriteLine($"Counted {count} objects");

    automationContext.MarkRunSuccess($"Counted {count} objects");
  }
}
