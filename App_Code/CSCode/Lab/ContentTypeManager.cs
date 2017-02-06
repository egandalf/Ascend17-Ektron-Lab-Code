using System.Collections.Generic;
using System.Linq;
using Ektron.Cms;
using Ektron.Cms.Content;
using Ektron.Cms.Framework;
using Ektron.Cms.Framework.Content;

namespace Lab
{
    /// <summary>
    /// Summary description for ContentTypeManager
    /// </summary>
    public class ContentTypeManager<T>
    {
        private ContentManager _contentManager;

        public ContentTypeManager()
        {
            _contentManager = new ContentManager();
        }

        public ContentTypeManager(ApiAccessMode mode)
        {
            _contentManager = new ContentManager(mode);
        }

        public ContentType<T> Add(ContentType<T> contentData)
        {
            contentData.Content.Html = EkXml.Serialize(typeof(T), contentData.SmartForm);
            contentData.Content = _contentManager.Add(contentData.Content);
            return contentData;
        }

        public ContentType<T> GetItem(long id, bool returnMetadata = false)
        {
            var contentData = _contentManager.GetItem(id, returnMetadata);
            return Make(contentData);
        }

        public IEnumerable<ContentType<T>> GetList<U>(U criteria) where U : ContentCriteria
        {
            var contentList = _contentManager.GetList(criteria);
            return contentList.Select(c => Make(c));
        }

        public ContentType<T> Save(ContentType<T> contentData)
        {
            contentData.Content.Html = EkXml.Serialize(typeof(T), contentData.SmartForm);
            contentData.Content = _contentManager.Save(contentData.Content);
            return contentData;
        }

        public ContentType<T> Update(ContentType<T> contentData)
        {
            contentData.Content.Html = EkXml.Serialize(typeof(T), contentData.SmartForm);
            contentData.Content = _contentManager.Update(contentData.Content);
            return contentData;
        }

        public ContentType<T> Make(ContentData contentData)
        {
            T smartForm = (T)EkXml.Deserialize(typeof(T), contentData.Html);
            return new ContentType<T>()
            {
                SmartForm = smartForm,
                Content = contentData
            };
        }
    }
}