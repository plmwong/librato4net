librato4net
===========

[![Build status](https://ci.appveyor.com/api/projects/status/v7wwdoeevkky7x55/branch/master?svg=true)](https://ci.appveyor.com/project/plmw/librato4net/branch/master)

librato4net is a simple library for sending metrics from a .NET application to the Librato API.

Getting
-----
librato4net can be obtained via NuGet at https://www.nuget.org/packages/librato4net


Usage
----

Call `Start()` to initialise the `MetricsPublisher` and begin the metric sending background thread:
```
MetricsPublisher.Start();
```

Or with a source:
```
MetricsPublisher.Start("source-computer");
```

Sending a single measurement:
```
var measuredValue = 3621.0f;
MetricsPublisher.Current.Measure("some.measured.metric", measuredValue);
```

Incrementing a counter (reset when the publisher is created):
```
MetricsPublisher.Current.Increment("some.counted.metric");
```

Sending the timing for a code block:
```
using (MetricsPublisher.Current.Time("some.timed.metric"))
{
	//do some things
}
```

Sending annotations (events) to mark when things happen:
```
MetricsPublisher.Current.Annotate("important-things", "an important thing happened!");
```

Configuration
----------

Configuration by default can be supplied through the `LibratoSettings` configuration section, by supplying
any of the following element attributes:

* `username` (required) : The username for accessing your Librato account.
* `apikey` (required) : The API key for accessing your Librato account. The API key can be set up under 'Account Settings' / 'All API Tokens' in your Librato account.
* `sendInterval` (optional) : TimeSpan of how long to wait between sending batched metric messages to Librato. Defaults to every 5 seconds.

Alternatively, you may supply settings through the `appSettings` instead. Instead of calling `MetricsPublisher.Start()`, you will have to call 
`MetricsPublisher.Start(settingsSource: SettingsSource.AppSettings)` instead for initialisation. The equivalent `appSettings` keys
are `Librato.Username`, `Librato.ApiKey` and `Librato.SendInterval`.

Configuration Example
----------------

```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="LibratoSettings" type="librato4net.LibratoSettings, librato4net"/>
  </configSections>
  ...
  <LibratoSettings apikey="YOUR_API_KEY" username="YOUR_USERNAME" />
  ...
</configuration>
```

```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  ...
  <appSettings>
    <add key="Librato.Username" value="YOUR_USERNAME" />
    <add key="Librato.ApiKey" value="YOUR_API_KEY" />
  </appSettings>
  ...
</configuration>
```