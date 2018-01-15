[## XmlSqlDatabaseSink
XmlSqlDatabaseSink  is a sink for the Semantic Logging Application Block](https://msdn.microsoft.com/en-us/library/dn775014(v=pandp.20).aspx) that exposes Event Source events to an Sql Server database.
The default database sink uses a Json document to store the payload data whereas this sink uses an xml document to store the payload data.

This sink is also available as a Nuget package: https://www.nuget.org/packages/SemanticLogging.Database.Xml/

## Usage
Usage is the same as with the default database sink. This sink provides one additional parameter to provide the name of the stored procedure
that is used to insert the datarecords. This makes it possible to use multiple XmlSqlDatabaseSink instances that writes to different tables:


```c#
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

```sql
SELECT
	FormattedMessage,
	Payload.value('(/Payload/sampleProperty/text())[1]', 'nvarchar(100)') as member,
FROM Traces
```

## Out-of-process logging

A sample configuration for out-of-process logging using a windows service (see https://msdn.microsoft.com/en-us/library/dn774996(v=pandp.20).aspx):

```xml

<?xml version="1.0" encoding="utf-8" ?>
<configuration xmlns="http://schemas.microsoft.com/practices/2013/entlib/semanticlogging/etw"
               xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
			   xmlns:etw="http://schemas.microsoft.com/practices/2013/entlib/semanticlogging/etw"
               xsi:schemaLocation="http://schemas.microsoft.com/practices/2013/entlib/semanticlogging/etw SemanticLogging-svc.xsd">
  
  <sinks>
    <xmlSqlDatabaseSink xmlns="urn:dhs.sinks.xmlSqlDatabaseSink" name="xmlSqlDatabaseSink" type ="SemanticLogging.Database.Xml.XmlSqlDatabaseSink, SemanticLogging.Database.Xml"
		instanceName="localhost"
		connectionString="Data Source=.\sql2012;Initial Catalog=SinkTests;Persist Security Info=True;Integrated Security=True;MultipleActiveResultSets=True"
		tableName="Traces"
		storedProcedureName="WriteTraces"
		bufferingIntervalInSeconds="30"
		bufferingCount="100"
		bufferingFlushAllTimeoutInSeconds="5"
		maxBufferSize="3000">
		<etw:sources>
			<etw:eventSource name="DeHeerSoftware-PlanCare2" level="Warning" />
		</etw:sources>
	</xmlSqlDatabaseSink>
  </sinks>
</configuration>

```

## How do I contribute?

Please see [CONTRIBUTE.md](/CONTRIBUTE.md) for more details.
