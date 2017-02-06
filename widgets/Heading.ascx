<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Heading.cs" Inherits="Ektron.Cms.CampaignManagement.LandingPages.Widgets.Heading" %>
 
 
<asp:MultiView ID="Views" runat="server" ActiveViewIndex="0">
    <asp:View ID="Display" runat="server">
        <asp:Literal ID="headingLiteral" runat="server"></asp:Literal>
    </asp:View>
    <asp:View ID="Edit" runat="server">
        
        <ektronUI:CssBlock runat="server" ID="buttonSetCss" Visible="true">
            <CssTemplate>
                #<%# this.WrapperClientID %> .theButtons{
                    padding-left: 7px;
                }

                #<%# this.WrapperClientID %> .theButtons h1, 
                #<%# this.WrapperClientID %> .theButtons h2, 
                #<%# this.WrapperClientID %> .theButtons h3,
                #<%# this.WrapperClientID %> .theButtons h4,
                #<%# this.WrapperClientID %> .theButtons h5,
                #<%# this.WrapperClientID %> .theButtons h6{
                    height: 80px;
                    -moz-box-sizing: border-box;
                    -webkit-box-sizing: border-box;
                    box-sizing: border-box;
                    display:inline-block;
                    padding: 0;
                    vertical-align:top;
                    margin: 2px -2px 2px .05px;
            
                }

                #<%# this.WrapperClientID %> .theButtons h1 > label.ui-button, 
                #<%# this.WrapperClientID %> .theButtons h2 > label.ui-button, 
                #<%# this.WrapperClientID %> .theButtons h3 > label.ui-button,
                #<%# this.WrapperClientID %> .theButtons h4 > label.ui-button, 
                #<%# this.WrapperClientID %> .theButtons h5 > label.ui-button, 
                #<%# this.WrapperClientID %> .theButtons h6 > label.ui-button {
                    margin: 0;
                    height: 80px;
                    width: 120px;
                    line-height: 80px;
                    vertical-align: middle;
                    background-position:bottom;
                    border-radius: 0;
               
                }

                #<%# this.WrapperClientID %> .theButtons h1 > label.ui-button > span, 
                #<%# this.WrapperClientID %> .theButtons h2 > label.ui-button > span, 
                #<%# this.WrapperClientID %> .theButtons h3 > label.ui-button > span,
                #<%# this.WrapperClientID %> .theButtons h4 > label.ui-button > span, 
                #<%# this.WrapperClientID %> .theButtons h5 > label.ui-button > span, 
                #<%# this.WrapperClientID %> .theButtons h6 > label.ui-button > span{
                    line-height:80px;
                    vertical-align:top;
                    margin: 0px;
                    display:inline;
                }

                #<%# this.ButtonWrapperClientID %>{
                    text-align: right;
                }

                #<%# this.WrapperClientID %> .editableWrapper{
                    margin: 2px auto;
                    padding:0;
                    overflow: visible;
                }

                #<%# this.WrapperClientID %> .editableWrapper [contenteditable=true]{
                    border:solid;
                    min-height:20px;
                    margin: 0px;
                    word-wrap: break-word;
                    color: black;
                    background-color: white;
                }
            </CssTemplate>
        </ektronUI:CssBlock>
       

        
        <div runat="server" id="wrapper">
             
            <div class="theButtons">
                <input type="radio" id="heading1" name="headingValues" checked="checked" value="1" /><h1><label for="heading1">H1</label></h1>
                <input type="radio" id="heading2" name="headingValues" value="2" /><h2><label for="heading2">H2</label></h2>
                <input type="radio" id="heading3" name="headingValues" value="3" /><h3><label for="heading3">H3</label></h3>
                <input type="radio" id="heading4" name="headingValues" value="4" /><h4><label for="heading4">H4</label></h4>
                <input type="radio" id="heading5" name="headingValues" value="5" /><h5><label for="heading5">H5</label></h5>
                <input type="radio" id="heading6" name="headingValues" value="6" /><h6><label for="heading6">H6</label></h6>
            </div>
            <div class="editableWrapper">
                
            </div>
        </div>
        <asp:HiddenField runat="server" ID="StaticHtml" Visible="false" />
        <div runat="server" id="buttonWrapper">
            <ektronUI:Button runat="server" DisplayMode="Button" ID="uxSave" OnClick="SaveClick" OnClientClick="Ektron.Cms.CampaignManagement.Widgets.Heading.saveClickHandler()" CausesValidation="True" Checked="False" CommandArgument="" CommandName="" meta:resourcekey="uxSaveResource1" NavigateUrl="" PostBackUrl="" PrimaryIcon="None" SecondaryIcon="None" ValidationGroup="" />
            <ektronUI:Button runat="server" DisplayMode="Button" ID="uxCancel" OnClick="CancelClick" CausesValidation="True" Checked="False" CommandArgument="" CommandName="" meta:resourcekey="uxCancelResource1" NavigateUrl="" PostBackUrl="" PrimaryIcon="None" SecondaryIcon="None" ValidationGroup="" />
        </div>

        <ektronUI:JavaScriptBlock runat="server" ID="AlohaJavascriptHandlerBlock" ExecutionMode="OnEktronReady" Visible="false">
                <ScriptTemplate>
                    
                    var $theButtons = $ektron('#<%# this.WrapperClientID %> .theButtons'),
                        $theHeadingRadios = $theButtons.children(':input:radio'),
                        $theHiddenField = $ektron('#<%# this.HiddenStaticHtmlClientID %>'),
                        
                        $theEditableWrapper = $ektron('#<%# this.WrapperClientID %> .editableWrapper'),
                        $theEditable = ($theHiddenField.val() !== '' ? $ektron($theHiddenField.val()).prop('contenteditable','true') :
                            $('<h1 contenteditable="true"></h1>'));

                    $theEditableWrapper.append($theEditable);
                    $theButtons.buttonset();
                    $theHeadingRadios.on('change', radioChange);
                    $theHiddenField.val('');

                    $theButtons.children('[type=radio]').each(function(index, elem){
                        var $elem = $(this),
                            headingTagNumber = $theEditable[0].tagName.substring(1);
                        if($elem.val() === headingTagNumber){
                            $elem.prop('checked', true);
                            $theButtons.buttonset('refresh');
                            return false;
                        }
                    });

                    function radioChange(eventObject) {
                        updateTheViewChildNode(eventObject.target.value);
                    }

                    function updateTheViewChildNode(headingNumber) {
                        $theEditable.replaceWith(function(){
                            return '<h' + headingNumber.toString() + ' contenteditable="true">' + $(this).html()   +' </' + headingNumber.toString() + '>';
                        }).remove();

                        $theEditable = $('#<%# this.WrapperClientID %> :header[contenteditable=true]');
                    }



                    if (Ektron.Namespace.Register) {
                        Ektron.Namespace.Register('Ektron.Cms.CampaignManagement.Widgets.Heading');
                        Ektron.Namespace.Register('Ektron.Support.Debugging');
                    
                        Ektron.Support.Debugging.log = function(msg){
                             if(window.console && window.console.log){
                                console.log(msg);
                            }
                        };
                    
                        Ektron.Cms.CampaignManagement.Widgets.Heading.saveClickHandler = function()
                        {
                            try
                            {
                                $ektron('#<%# this.WrapperClientID %> .editableWrapper').children().removeAttr('contenteditable');
                                var savedContent = escape($ektron('#<%# this.WrapperClientID %> .editableWrapper').html());
                                Ektron.Support.Debugging.log('static content value: ' + savedContent.toString());
                                $theHiddenField.val(savedContent);
                                Ektron.Support.Debugging.log('hidden field value: ' + $ektron('#<%# this.HiddenStaticHtmlClientID %>').val().toString());
                                return true;
                            }
                            catch(err){
                                return false;
                            }
                        }
                    }
                    else{
                        if(window.console && window.console.log){
                            console.log('Static Heading Widget requires Ektron Namespace Javascript to be registered.');
                        }
                    }
                </ScriptTemplate>
            </ektronUI:JavaScriptBlock>
        
    </asp:View>
</asp:MultiView>

