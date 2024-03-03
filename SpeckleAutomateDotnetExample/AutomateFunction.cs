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
        string connectionUri = Environment.GetEnvironmentVariable("MONGO_CONNECT") ?? "mongodb+srv://aecuser:aechack2024@opendetailcluster.qgxprtm.mongodb.net/?retryWrites=true&w=majority&appName=OpenDetailCluster";

            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);

            var database = client.GetDatabase("opendetail");
            var collection = database.GetCollection<BsonDocument>("details");


        // Send a ping to confirm a successful connection
        try
        {
            var result = database.RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }


        Console.WriteLine("Starting execution");
        _ = typeof(ObjectsKit).Assembly; // INFO: Force objects kit to initialize

        Console.WriteLine("Receiving version");
        var commitObject = await automationContext.ReceiveVersion();

        Console.WriteLine("Received version: " + commitObject);
        var objectId = commitObject.id; // This retrieves the object ID from the commit object.

        var openDetailObject = new DetailObject();
        //openDetailObject.populate();
        openDetailObject.Id = ObjectId.GenerateNewId();

        var speckleURL = commitObject.Flatten();

        //
        string speckleServerRoot = "https://app.speckle.systems";
        string speckleProject = automationContext.AutomationRunData.ProjectId;
        string streamId = automationContext.AutomationRunData.ProjectId;
        //here i need to get my object id 
        openDetailObject.URL = $"{speckleServerRoot}/streams/{streamId}/objects/{objectId}";

        var bsonDocument = openDetailObject.ToBsonDocument();

            // Insert the BSON document into the collection
            collection.InsertOne(bsonDocument);


        automationContext.MarkRunSuccess($"Counted {openDetailObject.URL} objects");
      }
    }
