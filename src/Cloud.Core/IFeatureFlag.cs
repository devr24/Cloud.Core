namespace Cloud.Core
{
    public interface IFeatureFlag
    {
        bool GetFeatureFlag(string key, string id, bool defaultValue = false);
    }
}