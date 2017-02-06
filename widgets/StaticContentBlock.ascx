<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StaticContentBlock.ascx.cs" Inherits="Ektron.Cms.CampaignManagement.LandingPages.Widgets.StaticContentBlock" %>

<asp:MultiView ID="Views" runat="server" ActiveViewIndex="0">
    <asp:View ID="Display" runat="server">
        <div runat="server" id="DisplayContent" />
    </asp:View>
    <asp:View ID="Edit" runat="server">

        <div class="editArea editable">

            <ektronUI:JavaScriptBlock runat="server" ID="AlohaJavascriptHandlerBlock" ExecutionMode="OnEktronReady" Visible="false">
                <ScriptTemplate>
                    if (Ektron.Namespace.Register) {
                        Ektron.Namespace.Register('Ektron.Cms.CampaignManagement.LandingPages.Widgets.StaticContentBlock');
                        Ektron.Namespace.Register('Ektron.Support.Debugging');
                    
                        Ektron.Support.Debugging.log = function(msg){
                             if(window.console && window.console.log){
                                console.log(msg);
                            }
                        };
                    
					
					window.alohaPromise = window.setInterval(function(){
					
						if(Aloha){
							
							clearInterval(window.alohaPromise);
							
							Aloha.ready(function(){
								Aloha.bind('aloha-editable-activated', function () {
								
                                    $('.aloha-ui-toolbar').css('z-index', '99998');
									$ektron('.ui-widget-overlay.ui-front').css('z-index', '99997');
									$ektron('.ektron-ux-dialog').css('z-index','99997');
									$ektron('#aloha-ui-context').css('z-index', '99998');
									$ektron('.aloha-ui.aloha-ui-toolbar.ui-draggable').css('z-index', '99998');
									$ektron('.aloha.aloha-surface.aloha-toolbar').css('z-index', '99998').children().css('z-index', '99998');
									$ektron('div.editArea.editable :hidden').css('z-index', '99998');
                                });
							});
						}
					}, 1);
					

                    

                        Ektron.Cms.CampaignManagement.LandingPages.Widgets.StaticContentBlock.saveClickHandler = function()
                        {
                            try
                            {
                                var savedContent = escape($ektron('#<%# this.AlohaContentControlClientID %>').parent().children('.aloha-editable[contenteditable=true]').html());
                                Ektron.Support.Debugging.log('static content value: ' + savedContent.toString());
                                var theHiddenField = $ektron('#<%# this.HiddenStaticHtmlClientId %>');
                                theHiddenField.val(savedContent);
                                Ektron.Support.Debugging.log('hidden field value: ' + theHiddenField.val().toString());
                                
                    //temp fix because another control is moving the widget body contents outside the form. Another team is working on that widget.
                                if(!$('form').find(theHiddenField)[0]){
                                    $('form').append(theHiddenField);
                                }
                            
                                
                                return true;
                            }
                            catch(err){
                                return false;
                            }
                        }
                    }
                    else{
                        if(window.console && window.console.log){
                            console.log('Static Content Block Widget requires Ektron Namespace Javascript to be registered.');
                        }
                    }
                </ScriptTemplate>
            </ektronUI:JavaScriptBlock>
            <asp:HiddenField runat="server" ID="StaticHtml" Visible="false" />
            <ektronUI:Editor runat="server" ID="Aloha" ToolbarConfig="StaticContentWidget"></ektronUI:Editor>
        </div>
        <div>
            <ektronUI:Button runat="server" DisplayMode="Button" ID="Save" OnClick="SaveClick" OnClientClick="Ektron.Cms.CampaignManagement.LandingPages.Widgets.StaticContentBlock.saveClickHandler()" />
            <ektronUI:Button runat="server" DisplayMode="Button" ID="Cancel" OnClick="CancelClick" />
        </div>
    </asp:View>
</asp:MultiView>