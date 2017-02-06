<%@ Page Title="" Language="C#" MasterPageFile="~/Components/Masters/Bootstrap.master" AutoEventWireup="true" CodeFile="PBHome.aspx.cs" Inherits="Components_Templates_PBHome" %>

<%@ Register Assembly="Ektron.Cms.Controls" Namespace="Ektron.Cms.Controls" TagPrefix="CMS" %>
<%@ Register Src="~/Workarea/PageBuilder/PageControls/PageHost.ascx" TagPrefix="PH" TagName="PageHost" %>
<%@ Register Src="~/Workarea/PageBuilder/PageControls/DropZone.ascx" TagPrefix="DZ" TagName="DropZone" %>
<%@ Register Assembly="Ektron.Cms.Widget" Namespace="Ektron.Cms.PageBuilder" TagPrefix="PB" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="PageHost" ContentPlaceHolderID="PageHostPlaceholder" runat="server">
    <PH:PageHost ID="uxPageHost" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <DZ:DropZone ID="uxHeroArea" runat="server" AllowAddColumn="false" AllowColumnResize="false">
        <ColumnDefinitions>
            <PB:ColumnData columnID="0" unit="custom" CssFramework="bootstrap" CssClass="col-md-12" />
        </ColumnDefinitions>
    </DZ:DropZone>
    <DZ:DropZone ID="uxFeaturesArea" runat="server" AllowAddColumn="true" AllowColumnResize="true">
    </DZ:DropZone>
</asp:Content>

