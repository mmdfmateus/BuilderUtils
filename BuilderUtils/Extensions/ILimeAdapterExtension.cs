namespace BuilderUtils.Extensions
{
    public interface ILimeAdapterExtension
    {
        string DocumentCollectionToString(object deserializedJson);
        string QuickReplyToString(object deserializedJson);
        string MediaLinkToString(object content);
        string LocationInputToString(object content);
    }
}