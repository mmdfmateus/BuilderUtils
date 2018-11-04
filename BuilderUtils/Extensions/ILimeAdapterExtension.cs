namespace BuilderUtils.Extensions
{
    public interface ILimeAdapterExtension
    {
        string CarrousselToString(object deserializedJson);
        string DocumentCollectionToString(object deserializedJson);
        string MenuToString(object deserializedJson);
        string QuickReplyToString(object deserializedJson);
    }
}