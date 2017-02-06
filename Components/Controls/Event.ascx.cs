using Ektron.Cms.Framework.Calendar;
using Lab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Components_Controls_Event : UserControlBase
{
    private Lazy<WebEventManager> EventManager = new Lazy<WebEventManager>(() => new WebEventManager());
    protected void Page_Load(object sender, EventArgs e)
    {
        if(RawContent != null && RawContent.XmlConfiguration.Id == Constants.EktronContentTypes.WebEvent) // Built-in Event Id
        {
            var cEvent = EventManager.Value.GetItem(RawContent.Id);

            uxHeading.Text = cEvent.Title;
            uxLocation.Text = cEvent.Location;
            //var eventDates = new List<DateTime>() { cEvent.EventStart };

            var eventCriteria = new Ektron.Cms.Common.WebEventCriteria(Ektron.Cms.Common.EventProperty.RecurrenceEndDate, Ektron.Cms.Common.EkEnumeration.OrderByDirection.Ascending);
            eventCriteria.AddFilter(Ektron.Cms.Common.EventProperty.Id, Ektron.Cms.Common.CriteriaFilterOperator.EqualTo, cEvent.Id);
            var occurrences = EventManager.Value.GetEventOccurrenceList(eventCriteria);
            var eventDates = occurrences.Select(d => d.EventStart);
            uxDateList.DataSource = eventDates.Take(10);
            uxDateList.DataBind();

            uxBody.Text = cEvent.Description;

            var apikey = "AIzaSyAB6-Du-AfvNKLKKhdak8RUQwINuXmUo4o";
            uxGoogleMap.Attributes.Add("src", string.Format("https://www.google.com/maps/embed/v1/place?key={0}&q={1}", apikey, cEvent.Location));
        }
    }
}