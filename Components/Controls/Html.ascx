<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Html.ascx.cs" Inherits="Components_Controls_Html" %>
<div class="row">
    <div class="col-md-3"></div>
    <div class="col-md-6">
        <ektron:EditorsMenu ID="uxEditor" runat="server">
            <h1><asp:Literal ID="uxHeading" runat="server" /></h1>
            <hr />
            <asp:Literal ID="uxBody" runat="server" />
        </ektron:EditorsMenu>
    </div>
    <div class="col-md-3"></div>
</div>
