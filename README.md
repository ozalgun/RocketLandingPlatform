# RocketLandingPlatform
## Prerequirements
.Visual Studio 2019
.NET Core SDK

## Library Utility
Install package >
- Install-Package RocketLandingPlatform -Version 1.0.0
- After package added, before using library you have to inject  library service  as "Singelton"

```cs
 static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                            .AddSingleton(_ =>new LandingChecker(platformWtih: int, platformHeight: int ,landingPlatformStartPositionX: int,landingPlatformStartPositionY ))
                            );
```

Or you can inject the dependency via constructor injection. Firstly add "LandingChecker" service as "Singlton" e on Startup:

```cs
 services.AddSingleton(_ =>new LandingChecker(platformWtih: int, platformHeight: int ,landingPlatformStartPositionX: int,landingPlatformStartPositionY ));
```

Then use it in any service. 
Method parameters can be configure by user.
