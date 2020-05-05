<a href="https://www.twilio.com">
  <img src="https://static0.twilio.com/marketing/bundles/marketing/img/logos/wordmark-red.svg" alt="Twilio" width="250" />
</a>

This application example demonstrates how to do Simple phone verification with C# ASP.NET Core MVC, and Twilio Verify.

> We are currently in the process of updating this sample template. If you are encountering any issues with the sample, please open an issue at [github.com/twilio-labs/code-exchange/issues](https://github.com/twilio-labs/code-exchange/issues) and we'll try to help you.

![](https://github.com/TwilioDevEd/verify-v2-quickstart-csharp/workflows/dotNETCore/badge.svg)

## Local Development

1. First clone this repository and `cd` into it.

   ```bash
   git clone git@github.com:TwilioDevEd/verify-v2-quickstart-csharp.git
   cd verify-v2-quickstart-csharp/VerifyV2Quickstart/
   ```

1. Update the file `twilio.json` with your Account SID, Auth Token and Verification SID. For the `VerificationSid` variable you'll need to provision a [Verification Service](https://www.twilio.com/console/verify/services) 

1. Install [EF Core CLI](https://docs.microsoft.com/en-gb/ef/core/what-is-new/ef-core-3.0/breaking-changes#the-ef-core-command-line-tool-dotnet-ef-is-no-longer-part-of-the-net-core-sdk) if it's not already installed.

    ```
    dotnet tool install --global dotnet-ef --version 3.1.1
    ```

1. Build the solution `dotnet build`.

1. Run `dotnet ef database update` to create the local DB.

1. Run the application `dotnet run`.

1. Check it out at [http://localhost:5000](http://localhost:5000)

That's it!

### Run unit tests

1. cd into VerifyV2Quickstart.Tests project

    `cd verify-v2-quickstart-csharp/VerifyV2Quickstart.Tests`
    
1. Run tests

    `dotnet test`

## Meta

* No warranty expressed or implied. Software is as is. Diggity.
* The CodeExchange repository can be found [here](https://github.com/twilio-labs/code-exchange/).
* [MIT License](http://www.opensource.org/licenses/mit-license.html)
* Lovingly crafted by Twilio Developer Education.
