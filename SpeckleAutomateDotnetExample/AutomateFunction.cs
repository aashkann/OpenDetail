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
        string connectionUri = "mongodb+srv://aecuser:aechack2024@opendetailcluster.qgxprtm.mongodb.net/?retryWrites=true&w=majority&appName=OpenDetailCluster";
        public static MONGO_CONNECT = GetEnvironmentVariable("MONGO_CONNECT");
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

        var openDetailObject = new DetailObject();
        openDetailObject.Id = ObjectId.GenerateNewId();
        var count = commitObject
          .Flatten();
          
            var speckleURL = commitObject.Flatten();
            openDetailObject.URL = automationContext.SpeckleClient.ServerUrl;

            var bsonDocument = openDetailObject.ToBsonDocument();

            // Insert the BSON document into the collection
            collection.InsertOne(bsonDocument);

            Console.WriteLine($"Counted {count} objects");

        automationContext.MarkRunSuccess($"Counted {count} objects");
      }
    }
