<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssistantControl.ascx.cs" Inherits="ApimPolicyAssistant.WebApp.WebForm.Controls.AssistantControl" %>

<div class="container">

    <div class="row">
        <div class="mb-3">
            <label for="Prompt" class="form-label"><strong>Prompt</strong></label>
            <asp:TextBox CssClass="form-control" ID="Prompt" TextMode="MultiLine" runat="server" Rows="10" placeholder="Add prompt here" />
        </div>
    </div>

    <div class="row">
        <div class="mb-3">
            <asp:Button CssClass="btn btn-primary" ID="Complete" runat="server" Text="Complete!" OnClick="Complete_Click" />
            <asp:Button CssClass="btn btn-secondary" ID="Clear" runat="server" Text="Clear!" OnClick="Clear_Click" />
        </div>
    </div>

    <div class="row">
        <div class="mb-3">
            <label for="Completion" class="form-label"><strong>Completion</strong></label>
            <asp:TextBox CssClass="form-control" ID="Completion" TextMode="MultiLine" runat="server" Rows="10" placeholder="Result will show here" ReadOnly="true" />
        </div>
    </div>

</div>
