using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ektron.Cms;
using Ektron.Cms.Extensibility;
using Lab;
using News = Lab.SmartForms.News.root;
using Ektron.Cms.Framework.Content;
/// <summary>
/// Summary description for UpdateSortDateMetadata
/// </summary>

namespace LabExtensions {
    public class UpdateSortDateMetadata : ContentStrategy
    {
        private Lazy<ContentTypeManager<News>> _newsManager = new Lazy<ContentTypeManager<News>>(() => new ContentTypeManager<News>());
        private Lazy<ContentManager> _contentManager = new Lazy<ContentManager>(() => new ContentManager());
        public override void OnAfterPublishContent(ContentData contentData, CmsEventArgs eventArgs)
        {
            if (contentData.XmlConfiguration.Id != Lab.Constants.SmartForms.NewsTypeId) return;

            var newsItem = _newsManager.Value.Make(contentData);
            if (newsItem == null) return;

            var sortDateMeta = newsItem.Content.MetaData.Where(m => m.Id == Lab.Constants.Metadata.MetaSortByDateId).FirstOrDefault();

            if (sortDateMeta == null) return;

            DateTime newSortDate;
            if (!DateTime.TryParse(newsItem.SmartForm.PublicationDate, out newSortDate)) return;

            DateTime currentSortDate;
            var currentDateSet = DateTime.TryParse(sortDateMeta.Text, out currentSortDate);

            if (currentDateSet && currentSortDate == newSortDate)
                return;

            _contentManager.Value.UpdateContentMetadata(contentData.Id, sortDateMeta.Id, newSortDate.ToString("MM/dd/yyyy"));
        }
    }
}