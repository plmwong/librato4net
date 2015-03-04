librato4net
===========

librato4net is a basic function library for sending metrics from a .NET application to the Librato API.

Usage
----

Sending a single measurement:
```
MetricsPublisher.Current.Measure("some.measured.metric", 2745.0f);
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
* `apiKey` (required) : The API key for accessing your Librato account. The API key can be set up under 'Account Settings' / 'All API Tokens' in your Librato account.

Configuration Example
----------------

```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="LibratoSettings" type="librato4net.LibratoSettings, librato4net"/>
  </configSections>
  ...
  <LibratoSettings apiKey="YOUR_API_KEY" username="YOUR_USERNAME" />
  ...
</configuration>
```
