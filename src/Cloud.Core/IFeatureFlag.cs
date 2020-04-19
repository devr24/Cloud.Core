namespace Cloud.Core
{
    public interface IFeatureFlag
    {
        bool GetFeatureFlag(string key, bool defaultValue = false);
    }
}
