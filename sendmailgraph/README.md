# SendMailWithAzureFunctionsAndMicrosoftGraph

An example of sending mail using Azure Functions and Microsoft Graph.

Open the MailService.cs file and change the following constants:

```csharp
private const string TenantId = ""; // Your tenant id
private const string ClientId = ""; // Your client id
private const string ClientSecret = ""; // Your client secret

private const string ToEmail = ""; // The email to send the message
private const string FromEmail = ""; // The email from send the message
```