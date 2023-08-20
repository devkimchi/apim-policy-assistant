<%@ Page Title="APIM Policy Assistant" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Async="true" CodeBehind="Default.aspx.cs" Inherits="ApimPolicyAssistant.WebApp.WebForm._Default" %>
<%@ Register TagPrefix="uc" TagName="AssistantControl" Src="~/Controls/AssistantControl.ascx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <uc:AssistantControl ID="AssistantControl" runat="server" />

</asp:Content>
