## SemanticLogging.Database.Xml
SemanticLogging.Database.Xml is a sink for the Semantic Logging Application Block that exposes Event Source events to an Sql Server database.
The default database sink uses a Json document to store the payload data whereas this sink uses an xml document to store the payload data.

This sink is also available as a Nuget package.

## Usage
Usage is the same as with the default database sink. This sink provides one additional parameter to provide the name of the stored procedure
that is used to insert the datarecords. This makes it possible to use multiple SemanticLogging.Database.Xml sinks that writes to different tables.

## How do I contribute?

Please see [CONTRIBUTE.md](/CONTRIBUTE.md) for more details.
