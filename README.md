<a href="https://www.twilio.com">
  <img src="https://static0.twilio.com/marketing/bundles/marketing/img/logos/wordmark-red.svg" alt="Twilio" width="250" />
</a>

This application example demonstrates how to do Simple phone verification with C# ASP.NET Core MVC, and Twilio Verify.

[![build status](https://ci.appveyor.com/api/projects/status/5b8cmuf5598em9t1/branch/master?svg=true)](https://ci.appveyor.com/project/TwilioDevEd/automated-survey-csharp)

## Local Development

1. First clone this repository and `cd` into it.

   ```bash
   $ git clone git@github.com:TwilioDevEd/verify-v2-quickstart-csharp.git
   $ cd verify-v2-quickstart-csharp/VerifyV2Quickstart/
   ```

1. Create a new file `twilio.json` and update the content.
   ```json
   {
     "Twilio": {
       "AccountSid": "Your Twilio Account SID",
       "AuthToken": "Your Twilio Auth Token",
       "VerificationSid": "Your Verify Service SID"
     }
   }
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
* [MIT License](http://www.opensource.org/licenses/mit-license.html)
* Lovingly crafted by Twilio Developer Education.
