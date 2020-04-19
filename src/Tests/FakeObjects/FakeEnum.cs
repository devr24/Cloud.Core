namespace Cloud.Core.Tests.FakeObjects
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [JsonConverter(typeof(GenericEnumStringConverter))]
    public enum FakeEnum
    {
        Default = 0,
        Known1 = 1,
        Known2 = 2
    }
}
