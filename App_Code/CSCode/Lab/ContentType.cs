using Ektron.Cms;

namespace Lab
{
    /// <summary>
    /// Summary description for ContentType
    /// </summary>
    public class ContentType<T>
    {
        public ContentData Content { get; set; }
        public T SmartForm { get; set; }
    }
}