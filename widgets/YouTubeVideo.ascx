<%@ Control Language="C#" AutoEventWireup="true" CodeFile="YouTubeVideo.ascx.cs" Inherits="widgets_YouTubeVideo" %>
<div id="<%= ClientID %>" class="ektronWidgetYouTube">
    <asp:MultiView ID="ViewSet" runat="server" ActiveViewIndex="0">
        <asp:View ID="View" runat="server">
            <asp:PlaceHolder ID="phContent" runat="server">
                <div id="uxRenderVideo" runat="server">
                    <iframe width="100%" style="width: 100%" height="412" src="<%= MovieSourceURL %>" frameborder="0" allowfullscreen></iframe>
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phHelpText" runat="server">
                <div id="divHelpText" runat="server" style="font: normal 12px/15px arial; width: 100%; height: 100%;">
                    <%=_ContentAPI.EkMsgRef.GetMessage("lbl click on the edit icon")%> (<img alt="edit icon" title="edit icon" src="<%=appPath %>PageBuilder/PageControls/Themes/TrueBlue/images/edit_on.png" width="12" height="12" border="0" />) 
                <%=_ContentAPI.EkMsgRef.GetMessage("lbl in the top right corner of this widget to select a video you wish to display")%>
                </div>
            </asp:PlaceHolder>
        </asp:View>
        <asp:View ID="Edit" runat="server">
            <div class="youtube" id="YouTubeVideo1">
                <ul class="ektronWidgetYTTabs clearfix">
                    <li style="display: none;"><a href="#" onclick="Ektron.Widget.YouTubeVideo.SwitchPane(this, 'VideoListTab'); return false;"
                        id="VideoListTab" class="ektronWidgetYTTab "><%=_ContentAPI.EkMsgRef.GetMessage("lbl video list")%></a></li>
                    <li><a href="#" onclick="Ektron.Widget.YouTubeVideo.SwitchPane(this, 'SearchLink'); return false;"
                        id="SearchLink" class="ektronWidgetYTTab selectedTab"><%=_ContentAPI.EkMsgRef.GetMessage("generic search")%></a></li>
                </ul>
                <div class="pane VideoListTab" style="display: none;">
                    <div class="YTOptions YTViewOptions">
                        <%=_ContentAPI.EkMsgRef.GetMessage("lbl sort by")%>:
                        <select id="<%= ClientID %>sort_by" onchange="Ektron.Widget.YouTubeVideo.widgets['<%= ClientID %>'].FirstVideos();">
                            <option value="recently_featured" selected="selected"><%=_ContentAPI.EkMsgRef.GetMessage("lbl recently featured")%></option>
                            <option value="top_rated"><%=_ContentAPI.EkMsgRef.GetMessage("lbl top rated")%></option>
                            <option value="top_favorites"><%=_ContentAPI.EkMsgRef.GetMessage("lbl top favorites")%></option>
                            <option value="most_viewed"><%=_ContentAPI.EkMsgRef.GetMessage("lbl most viewed")%></option>
                            <option value="most_popular"><%=_ContentAPI.EkMsgRef.GetMessage("lbl most popular")%></option>
                            <option value="most_recent"><%=_ContentAPI.EkMsgRef.GetMessage("lbl most recent")%></option>
                        </select>
                        <select style="display: none;" id="<%= ClientID %>sort_order" onchange="Ektron.Widget.YouTubeVideo.widgets['<%= ClientID %>'].FirstVideos();">
                            <option value="DESC"><%=_ContentAPI.EkMsgRef.GetMessage("lbl descending")%></option>
                            <option value="ASC"><%=_ContentAPI.EkMsgRef.GetMessage("lbl ascending")%></option>
                        </select>
                    </div>
                    <ul class="video-list ektronWidgetYTVideos">
                    </ul>
                    <ul class="ektronWidgetYTButtonWrapper">
                        <li><a id="<%= ClientID %>First" onclick="Ektron.Widget.YouTubeVideo.widgets['<%= ClientID %>'].FirstVideos();" class="ektronWidgetYTButton ektronWidgetYTButtonFirst" title="First" style="display: none;"><span>First</span></a></li>
                        <li><a id="<%= ClientID %>Previous" onclick="Ektron.Widget.YouTubeVideo.widgets['<%= ClientID %>'].PreviousVideos();" class="ektronWidgetYTButton ektronWidgetYTButtonPrevious" title="Previous" style="display: none;"><span>Prev</span></a></li>
                        <li><span class="video-result"><%=_ContentAPI.EkMsgRef.GetMessage("lbl no results")%></span></li>
                        <li><a id="<%= ClientID %>Next" onclick="Ektron.Widget.YouTubeVideo.widgets['<%= ClientID %>'].NextVideos();" class="ektronWidgetYTButton ektronWidgetYTButtonNext" title="Next" style="display: none;"><span>Next</span></a></li>
                        <li><a id="<%= ClientID %>Last" onclick="Ektron.Widget.YouTubeVideo.widgets['<%= ClientID %>'].LastVideos();" class="ektronWidgetYTButton ektronWidgetYTButtonLast" title="Last" style="display: none;"><span>Last</span></a></li>
                    </ul>
                </div>
                <div class="pane SearchLink" style="overflow-x: hidden; overflow-y: scroll; max-height: 400px;">
                    <div class="search-box YTOptions YTSearchOptions">
                        <span style="display: none;">Search by:
                        <select id="<%= ClientID %>searchtype" onchange="Ektron.Widget.YouTubeVideo.widgets['<%= ClientID %>'].SearchFirstVideos();">
                            <option value="TEXT"><%=_ContentAPI.EkMsgRef.GetMessage("text")%></option>
                            <option value="TAG"><%=_ContentAPI.EkMsgRef.GetMessage("lbl tag")%></option>
                        </select></span>
                        <input type="text" id="<%= ClientID %>SearchText" onkeypress="Ektron.Widget.YouTubeVideo.widgets['<%= ClientID %>'].KeyPressHandler(this, event, '<%= ClientID %>');" />
                        <%=_ContentAPI.EkMsgRef.GetMessage("lbl sort by")%>:
                        <select id="<%= ClientID %>sort_by1">
                            <option value="relevance" selected="selected">Relevance</option>
                            <option value="title">Title</option>
                            <option value="date">Date</option>
                            <option value="rating">Rating</option>
                            <option value="videoCount">Video Count</option>
                            <option value="viewCount">View Count</option>
                        </select>
                        <a id="<%= ClientID %>Search" title="Search" class="ektronWidgetYTGoButton" onclick="Ektron.Widget.YouTubeVideo.ResetPages();Ektron.Widget.YouTubeVideo.widgets['<%= ClientID %>'].SearchVideos();">Go</a>
                    </div>
                    <ul class="video-search ektronWidgetYTVideos">
                    </ul>
                    <ul class="ektronWidgetYTButtonWrapper ektronWidgetYTSearchButtons">
                        <li><a id="<%= ClientID %>FirstSearch" onclick="Ektron.Widget.YouTubeVideo.widgets['<%= ClientID %>'].SearchFirstVideos();" class="ektronWidgetYTButton ektronWidgetYTButtonFirst" title="First" style="display: none;"><span>First</span></a></li>
                        <li><a id="<%= ClientID %>PreviousSearch" onclick="Ektron.Widget.YouTubeVideo.widgets['<%= ClientID %>'].SearchPreviousVideos();" class="ektronWidgetYTButton ektronWidgetYTButtonPrevious" title="Previous" style="display: none;"><span>Prev</span></a></li>
                        <li><span class="video-search-result"><%=_ContentAPI.EkMsgRef.GetMessage("lbl no results")%></span></li>
                        <li><a id="<%= ClientID %>NextSearch" onclick="Ektron.Widget.YouTubeVideo.widgets['<%= ClientID %>'].SearchNextVideos();" class="ektronWidgetYTButton ektronWidgetYTButtonNext" title="Next" style="display: none;"><span>Next</span></a></li>
                        <li><a id="<%= ClientID %>LastSearch" onclick="Ektron.Widget.YouTubeVideo.widgets['<%= ClientID %>'].SearchLastVideos();" class="ektronWidgetYTButton ektronWidgetYTButtonLast" title="Last" style="display: none;"><span>Last</span></a></li>
                    </ul>
                </div>
            </div>
            <asp:TextBox ID="tbData" runat="server" Style="display: none; width: 95%"> </asp:TextBox>
            <asp:Button ID="SaveButton" runat="server" Text="Save" OnClick="SaveButton_Click" Style="display: none;" UseSubmitBehavior="false" />
            &nbsp;&nbsp;
            <asp:Button ID="CancelButton" runat="server" Text="Cancel" OnClick="CancelButton_Click" CssClass="ytcancelbutton" UseSubmitBehavior="false" />
            <input type="hidden" id="<%= ClientID %>youtubeAPIKey" value="<%= YoutubeAPIKey %>" />
        </asp:View>
    </asp:MultiView>
</div>
