using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace librato4net
{
    public class UnixDateTimeConverter : DateTimeConverterBase
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime dateTime;

            if (value is DateTime?)
            {
                dateTime = ((DateTime?) value).Value;
            }
            else if (value is DateTime)
            {
                dateTime = (DateTime) value;
            }
            else
            {
                throw new InvalidOperationException("Value must be a DateTime or Nullable<DateTime> to use this converter");
            }

            long unixTime = ToUnixTime(dateTime);

            writer.WriteValue(unixTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            long unixTime = (long)reader.Value;

            return FromUnixTime(unixTime);
        }

        private static DateTime FromUnixTime(long unixTime)
        {
            var dateTime = Epoch;

            return dateTime.AddSeconds(unixTime);
        }

        private static long ToUnixTime(DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
            {
                return 0;
            }

            var delta = dateTime - Epoch;

            return (long)delta.TotalSeconds;
        }
    }
}

