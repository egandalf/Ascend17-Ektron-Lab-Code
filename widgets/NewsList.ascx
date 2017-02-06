<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewsList.ascx.cs" Inherits="widgets_NewsList" %>
<%@ Import Namespace="Lab" %>
<%@ Import Namespace="Lab.SmartForms.News" %>
<asp:MultiView ID="uxWidgetView" runat="server">
    <asp:View ID="uxEditView" runat="server">
        <div class="form-control">
            <asp:Label for="uxFolderSelector" runat="server" ID="uxFolderSelectorLabel">Folder:</asp:Label>
            <asp:DropDownList ID="uxFolderSelector" runat="server">
            </asp:DropDownList>
            <p>
                <asp:Button ID="uxSaveButton" runat="server" Text="Save" OnClick="uxSaveButton_Click" />
                <asp:Button ID="uxCancelButton" runat="server" Text="Cancel" OnClick="uxCancelButton_Click" />
            </p>
        </div>
    </asp:View>
    <asp:View ID="uxPublishView" runat="server">
        <asp:ListView ID="uxNewsList" runat="server" ItemPlaceholderID="itemPlaceholder">
            <LayoutTemplate>
                <div class="news-list list-group">
                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <a  href="<%#((ContentType<root>)Container.DataItem).Content.Quicklink %>" class="list-group-item">
                    <h4 class="list-group-item-heading"><%#((ContentType<root>)Container.DataItem).SmartForm.Headline %></h4>
                    <p class="list-group-item-text"><%#string.Join(" ", ((ContentType<root>)Container.DataItem).SmartForm.Release.Any.GetText().Split(' ').Take(30).ToArray()) %>...</p>
                </a>
            </ItemTemplate>
        </asp:ListView>
    </asp:View>
</asp:MultiView>