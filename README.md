librato4net
===========

[![Build status](https://ci.appveyor.com/api/projects/status/v7wwdoeevkky7x55/branch/master?svg=true)](https://ci.appveyor.com/project/plmw/librato4net/branch/master)

librato4net is a simple library for sending metrics from a .NET application to the Librato API.

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

Configuration
----------

* `username` (required) : The username for accessing your Librato account.
* `apikey` (required) : The API key for accessing your Librato account. The API key can be set up under 'Account Settings' / 'All API Tokens' in your Librato account.

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
