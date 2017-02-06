<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="Components_Controls_Menu" %>
<%--<ul class="nav navbar-nav">
                        <li class="active">
                            <a href="#">About</a>
                        </li>
                        <li>
                            <a href="#">Services</a>
                        </li>
                        <li>
                            <a href="#">Blog</a>
                        </li>
                    </ul>--%>

<asp:ListView ID="uxMenu" runat="server" ItemPlaceholderID="itemPlaceholder">
    <LayoutTemplate>
        <ul class="nav navbar-nav">
            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
        </ul>
    </LayoutTemplate>
    <ItemTemplate>
        <li class="<%#((bool)Eval("Selected")) == true ? "active" : string.Empty %>":>
            <a href="<%#Eval("Link") %>"><%#Eval("Text") %></a>
        </li>
    </ItemTemplate>
</asp:ListView>