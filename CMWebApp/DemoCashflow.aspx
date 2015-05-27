<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/default.master" AutoEventWireup="true" CodeFile="DemoCashflow.aspx.cs" Inherits="DemoCashflow" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">
    <form id="form1" runat="server">
    <asp:Button ID="btnCheckOut" runat="server" Text="CheckOut" OnClick="btnCheckOut_Click" />
        </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" Runat="Server">
</asp:Content>

