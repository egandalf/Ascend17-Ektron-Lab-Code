<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ServiceTeaser.ascx.cs" Inherits="widgets_ServiceTeaser" %>
<asp:MultiView ID="uxWidgetView" runat="server">
    <asp:View ID="uxEditView" runat="server">
        <div class="form-control">
            <asp:Label for="uxServiceSelector" runat="server" ID="uxServiceSelectorLabel">Service:</asp:Label>
            <asp:DropDownList ID="uxServiceSelector" runat="server">
            </asp:DropDownList>
            <p>
                <asp:Button ID="uxSaveButton" runat="server" Text="Save" OnClick="uxSaveButton_Click" />
                <asp:Button ID="uxCancelButton" runat="server" Text="Cancel" OnClick="uxCancelButton_Click" />
            </p>
        </div>
    </asp:View>
    <asp:View ID="uxPublishView" runat="server">
        <asp:Panel ID="uxServicePanel" runat="server">
            <a id="uxServiceLink" runat="server">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3><asp:Literal ID="uxServiceHeading" runat="server" /></h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-8">
                                <asp:Literal ID="uxServiceDescription" runat="server" />
                                <div class="row">
                                    <div class="col-md-10"></div>
                                    <div class="col-md-2">
                                        $<asp:Literal ID="uxServicePrice" runat="server" />/hr
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 text-center">
                                <asp:Image ID="uxServiceImage" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </a>
        </asp:Panel>
    </asp:View>
</asp:MultiView>