using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab
{
    /// <summary>
    /// Summary description for Constants
    /// </summary>
    public class Constants
    {
        public class Metadata
        {
            public const long MetaTitleId = 171;
            public const long MetaKeywordsId = 172;
            public const long MetaDescriptionId = 173;
            public const long MetaSortByDateId = 174;
        }

        public class Menus
        {
            public const long MainMenu = 6;
        }

        public class Strings
        {
            public const string TitleSuffix = " // Ektron to Episerver Lab";
        }

        public class Folders
        {
            public const long BlogFolderId = 79;
            public const long CalendarFolderId = 78;
            public const long CtaFolderId = 83;
            public const long ExecutiveBioFolderId = 81;
            public const long NewsFolderId = 76;
            public const long ServicesFolderId = 80;
            public const long SettingsFolderId = 72;
            public const long PrimaryContentFolder = 73;
            public const long PrimaryPagesFolder = 75;
        }

        public class EktronContentTypes
        {
            public const long WebEvent = 5;
        }

        public class SmartForms
        {
            public const long CtaTypeId = 10;
            public const long NewsTypeId = 8;
            public const long BioTypeId = 7;
            public const long ServiceTypeId = 9;
            public const long SettingsTypeId = 11;
        }
    }
}