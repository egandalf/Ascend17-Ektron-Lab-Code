<%@ Page Title="" Language="C#" MasterPageFile="~/Components/Masters/Bootstrap.master" AutoEventWireup="true" CodeFile="PBLanding.aspx.cs" Inherits="Components_Templates_PBLanding" %>

<%@ Register Assembly="Ektron.Cms.Controls" Namespace="Ektron.Cms.Controls" TagPrefix="CMS" %>
<%@ Register Src="~/Workarea/PageBuilder/PageControls/PageHost.ascx" TagPrefix="PH" TagName="PageHost" %>
<%@ Register Src="~/Workarea/PageBuilder/PageControls/DropZone.ascx" TagPrefix="DZ" TagName="DropZone" %>
<%@ Register Assembly="Ektron.Cms.Widget" Namespace="Ektron.Cms.PageBuilder" TagPrefix="PB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="PageHost" ContentPlaceHolderID="PageHostPlaceholder" runat="server">
    <PH:PageHost ID="uxPageHost" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <DZ:DropZone ID="uxRow1" runat="server" AllowAddColumn="true" AllowColumnResize="true">
    </DZ:DropZone>
    <DZ:DropZone ID="uxRow2" runat="server" AllowAddColumn="true" AllowColumnResize="true">
    </DZ:DropZone>
    <DZ:DropZone ID="uxRow3" runat="server" AllowAddColumn="true" AllowColumnResize="true">
    </DZ:DropZone>
</asp:Content>