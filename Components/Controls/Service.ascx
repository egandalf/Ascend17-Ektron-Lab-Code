<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Service.ascx.cs" Inherits="Components_Controls_Service" %>
<h1><asp:Literal ID="uxHeading" runat="server"/></h1>
<div class="row">
    <div class="col-md-8">
        <div class="price">
            <asp:Literal ID="uxPrice" runat="server"/>
        </div>
        <div class="body">
            <asp:Literal ID="uxBody" runat="server" />
        </div>
    </div>
    <div class="col-md-4">
        <asp:Image ID="uxServiceImage" runat="server" />
    </div>
</div>