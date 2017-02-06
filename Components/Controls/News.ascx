<%@ Control Language="C#" AutoEventWireup="true" CodeFile="News.ascx.cs" Inherits="Components_Controls_News" %>
<div class="row">
    <div class="col-md-3"></div>
    <div class="col-md-6">
        <ektron:EditorsMenu AddText="Add News" DisplayType="Content" ID="uxEditor" runat="server">
            <h1><asp:Literal ID="uxHeadline" runat="server" /></h1>
            <h3 id="uxSubhead" runat="server" visible="false"></h3>
            <p><i>By: <asp:Literal ID="uxByline" runat="server" /></i> | <i>Source: <asp:Literal ID="uxSource" runat="server" /></i></p>
            <p><i>Published: <asp:Literal ID="uxPubDate" runat="server" /></i></p>
            <hr />
            <div>
                <asp:Literal ID="uxPubBody" runat="server" />
            </div>
            <fieldset>
                <legend>Contact</legend>
                <div>Name: <asp:Literal ID="uxContactName" runat="server" /></div>
                <div>Email: <asp:Literal ID="uxContactEmail" runat="server" /></div>
                <div>Phone: <asp:Literal ID="uxContactPhone" runat="server" /></div>
            </fieldset>
        </ektron:EditorsMenu>
    </div>
    <div class="col-md-3"></div>
</div>
