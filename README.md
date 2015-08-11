## SemanticLogging.Database.Xml
SemanticLogging.Database.Xml is a sink for the Semantic Logging Application Block that exposes Event Source events to an Sql Server database.
The default database sink uses a Json document to store the payload data whereas this sink uses an xml document to store the payload data.

This sink is also available as a Nuget package.

## Usage
Usage is the same as with the default database sink. This sink provides one additional parameter to provide the name of the stored procedure
that is used to insert the datarecords. This makes it possible to use multiple SemanticLogging.Database.Xml sinks that writes to different tables:


```
            var listener1 = new ObservableEventListener();
            listener1.LogToSqlDatabase
                (
                    Environment.MachineName,
                    @"data source=.\sql2012;initial catalog=SinkTests;integrated security=True",
                    bufferingInterval: TimeSpan.FromSeconds(5),
                    tableName: "Others",
                    storedProcedureName: "WriteOthers"
                );
            listener1.EnableEvents(MyEventSource1.Log, EventLevel.LogAlways);

            var listener2 = new ObservableEventListener();
            listener2.LogToSqlDatabase
                (
                    Environment.MachineName,
                    @"data source=.\sql2012;initial catalog=SinkTests;integrated security=True",
                    bufferingInterval: TimeSpan.FromSeconds(5),
                    tableName: "Sessions",
                    storedProcedureName: "WriteSessions"
                );
            listener2.EnableEvents(MyEventSource2.Log, EventLevel.LogAlways);
```
## Query payload

Since the payload is stored in an xml column it is easy to query, for example:

```
SELECT
	FormattedMessage,
	Payload.value('(/Payload/sampleProperty/text())[1]', 'nvarchar(100)') as member,
FROM Traces
```

## How do I contribute?

Please see [CONTRIBUTE.md](/CONTRIBUTE.md) for more details.
