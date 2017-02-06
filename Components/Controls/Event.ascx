<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Event.ascx.cs" Inherits="Components_Controls_Event" %>
<div class="row">
    <div class="col-md-6">
        <h1>
            <asp:Literal ID="uxHeading" runat="server" /></h1>
        <p>
            Location:
            <asp:Literal ID="uxLocation" runat="server" />
        </p>
        <p>
            Dates/Times:
        </p>
        <asp:ListView runat="server" ID="uxDateList" ItemPlaceholderID="itemPlaceholder">
            <LayoutTemplate>
                <ul>
                    <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                </ul>
            </LayoutTemplate>
            <ItemTemplate>
                <li>
                    <%#((DateTime)Container.DataItem).ToLongDateString() %>
                </li>
            </ItemTemplate>
        </asp:ListView>
        <asp:Literal ID="uxBody" runat="server" />
    </div>
    <div class="col-md-6">
        <%--AIzaSyAB6-Du-AfvNKLKKhdak8RUQwINuXmUo4o--%>
        <div class="google-map">
            <iframe
                runat="server"
                id="uxGoogleMap"
               
                frameborder="0"
                style="border: 0"
                allowfullscreen></iframe>
        </div>
    </div>
</div>
