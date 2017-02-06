<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormControl.ascx.cs" Inherits="widgets.WidgetsFormControl" %>
<%@ Register Src="~/widgets/ContentBlock/foldertree.ascx" TagPrefix="UC" TagName="FolderTree" %>
<asp:MultiView ID="ViewSet" runat="server">
    <asp:View ID="View" runat="server">

        <ektron:EditorsMenu runat="server" DisplayType="Content" ID="uxFormEditorsMenu">
            <ektron:FormControl ID="uxFormControl" runat="server" Visible="true" DynamicParameter="ekfrm" />
        </ektron:EditorsMenu>
        <asp:Label ID="errorLb" runat="server" />
    </asp:View>
    <asp:View ID="Edit" runat="server">
        <div id="<%=ClientID%>" class="CBWidget">
            <div class="CBEdit">
                <asp:Label ID="editError" runat="server" />
                <div id="CBFilterOptions">

                    <div style="display: none">
                        <asp:DropDownList ID="CBTypeFilter" runat="server" CssClass="CBTypeFilter">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="CBTabInterface">
                    <ul class="CBTabWrapper clearfix">
                        <li class="CBTab selected"><a href="#ByFolder"><span>Folder</span></a></li>
                    </ul>
                    <div class="ByFolder CBTabPanel">
                        <UC:FolderTree ID="CBFolderTree" runat="server" />
                    </div>
                </div>
                <div id="ResultsToggle"><a href="#" onclick="return Ektron.PFWidgets.ContentBlock.toggleResultsPane();">View Results</a></div>
                <div id="CBResults" style="display: none;"></div>
                <div id="CBPaging"></div>
                <asp:TextBox ID="tbData" CssClass="HiddenTBData" runat="server" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="tbFolderPath" CssClass="HiddenTBFolderPath" runat="server" Style="display: none;"></asp:TextBox>
                <div class="CBEditControls">
                    <asp:Button ID="CancelButton" CssClass="CBCancel" UseSubmitBehavior="false" runat="server" Text="Cancel" OnClick="CancelButton_Click" />
                    <asp:Button ID="SaveButton" CssClass="CBSave" runat="server" Text="Save" UseSubmitBehavior="false" OnClick="SaveButton_Click" OnClientClick="Ektron.PFWidgets.ContentBlock.Save();" />
                </div>
            </div>
            <input type="hidden" id="hdnAppPath" name="hdnAppPath" value="<%=AppPath%>" />
            <input type="hidden" id="hdnLangType" name="hdnLangType" value="<%=LangType%>" />
            <input type="hidden" id="hdnFolderId" name="hdnFolderId" value="0" />
        </div>
    </asp:View>
</asp:MultiView>
