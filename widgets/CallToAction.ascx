<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CallToAction.ascx.cs" Inherits="widgets_CallToAction" %>
<asp:MultiView ID="uxWidgetView" runat="server">
    <asp:View ID="uxEditView" runat="server">
        <div class="form-control">
            <asp:Label for="uxCtaSelector" runat="server" ID="uxCtaSelectorLabel">Call to Action:</asp:Label>
            <asp:DropDownList ID="uxCtaSelector" runat="server">
            </asp:DropDownList>
            <p>
                <asp:Button ID="uxSaveButton" runat="server" Text="Save" OnClick="uxSaveButton_Click" />
                <asp:Button ID="uxCancelButton" runat="server" Text="Cancel" OnClick="uxCancelButton_Click" />
            </p>
        </div>
    </asp:View>
    <asp:View ID="uxPublishView" runat="server">
        <asp:Panel ID="uxCtaPanel" runat="server">
            <div class="hero-feature text-center">
                <div class="thumbnail">
                    <asp:Image ID="uxCtaImage" runat="server" />
                    <div class="caption">
                        <%--<h3><asp:Literal ID="uxCtaHeading" runat="server" /></h3>--%>
                        <p><asp:Literal ID="uxCtaText" runat="server" /></p>
                        <p>
                            <asp:HyperLink ID="uxCtaLink" runat="server" CssClass="btn btn-primary"></asp:HyperLink>
                        </p>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </asp:View>
</asp:MultiView>