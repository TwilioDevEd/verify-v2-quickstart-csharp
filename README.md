<a  href="https://www.twilio.com">
<img  src="https://static0.twilio.com/marketing/bundles/marketing/img/logos/wordmark-red.svg"  alt="Twilio"  width="250"  />
</a>

# Twilio Verify Quickstart with .NET core

![](https://github.com/TwilioDevEd/verify-v2-quickstart-csharp/workflows/dotNETCore/badge.svg)

> We are currently in the process of updating this sample template. If you are encountering any issues with the sample, please open an issue at [github.com/twilio-labs/code-exchange/issues](https://github.com/twilio-labs/code-exchange/issues) and we'll try to help you.

## About

This application example demonstrates how to do Simple phone verification with C# ASP.NET Core MVC, and Twilio Verify.

Implementations in other languages:

| Python | Java | Ruby | PHP | Node |
| :--- | :--- | :----- | :-- | :--- |
| [Done](https://github.com/TwilioDevEd/verify-v2-quickstart-python) | [Done](https://github.com/TwilioDevEd/verify-v2-quickstart-java)  | [Done](https://github.com/TwilioDevEd/verify-v2-quickstart-rails)    | [Done](https://github.com/TwilioDevEd/verify-v2-quickstart-php) | [Done](https://github.com/TwilioDevEd/verify-v2-quickstart-node)  |

<!--
### How it works

**TODO: Describe how it works**
-->

## Set up

### Requirements

- [dotnet](https://dotnet.microsoft.com/)
- A Twilio account - [sign up](https://www.twilio.com/try-twilio)

### Twilio Account Settings

This application should give you a ready-made starting point for writing your
own application. Before we begin, we need to collect
all the config values we need to run the application:

| Config&nbsp;Value | Description                                                                                                                                                  |
| :---------------- | :----------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Account&nbsp;Sid  | Your primary Twilio account identifier - find this [in the Console](https://www.twilio.com/console).                                                         |
| Auth&nbsp;Token   | Used to authenticate - [just like the above, you'll find this here](https://www.twilio.com/console).                                                         |
| Verification&nbsp;Sid |  For Verification Service SID. You can generate one [here](https://www.twilio.com/console/verify/services) |

### Local development

After the above requirements have been met:

1. Clone this repository and `cd` into it

```bash
git clone git@github.com:TwilioDevEd/verify-v2-quickstart-csharp.git
cd verify-v2-quickstart-csharp/VerifyV2Quickstart/
```

2. Build to install the dependencies

```bash
dotnet build
```

3. Set your environment variables

```bash
cp VerifyV2Quickstart/twilio.json.example VerifyV2Quickstart/twilio.json
```

See [Twilio Account Settings](#twilio-account-settings) to locate the necessary environment variables.

4. Install [EF Core CLI](https://docs.microsoft.com/en-gb/ef/core/what-is-new/ef-core-3.0/breaking-changes#the-ef-core-command-line-tool-dotnet-ef-is-no-longer-part-of-the-net-core-sdk) if it's not already installed.

```
dotnet tool install --global dotnet-ef --version 3.0.0
```

5. Create the local DB. This also should be executed in `VerifyV2Quickstart` directory.

```
dotnet ef database update
```

6. Run the application

```bash
dotnet run
```

7. Navigate to [http://localhost:5000](http://localhost:5000)

That's it!

### Docker

If you have [Docker](https://www.docker.com/) already installed on your machine, you can use our `docker-compose.yml` to setup your project.

1. Make sure you have the project cloned.
2. Setup the `twilio.json` file as outlined in the [Local Development](#local-development) steps.
3. Run `docker-compose up`.

### Tests

You can run the tests locally by typing:

```bash
dotnet test
```

## Resources

- The CodeExchange repository can be found [here](https://github.com/twilio-labs/code-exchange/).

## Contributing

This template is open source and welcomes contributions. All contributions are subject to our [Code of Conduct](https://github.com/twilio-labs/.github/blob/master/CODE_OF_CONDUCT.md).

[Visit the project on GitHub](https://github.com/twilio-labs/sample-template-dotnet)

## License

[MIT](http://www.opensource.org/licenses/mit-license.html)

## Disclaimer

No warranty expressed or implied. Software is as is.

[twilio]: https://www.twilio.com
