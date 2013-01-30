<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" Inherits="DLExt.WebApplication.Default"
    ValidateRequest="false" CodeBehind="Default.aspx.cs" %>

<%--MasterPageFile="~/Site.master"--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <%--<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1
        {
            width: 386px;
        }
    </style>
</asp:Content>
    --%>
    <%--<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">--%>
    <%--<asp:Label ID="DebugInfoLabel" runat="server" Text="Label"></asp:Label>--%>
    <form id="Form1" runat="server">
    <table>
        <tr>
            <td class="locations" rowspan="2">
                <div class="header">
                    Отделения</div>
                <%--<asp:CheckBoxList ID="CheckBoxListLocations" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CheckBoxListLocationsOnSelectedIndexChanged"
                    CssClass="locationsBox">
                </asp:CheckBoxList>--%>
                <asp:Repeater ID="RepeaterLocations" runat="server" OnItemCommand="OnLinkButtonCommand">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBoxLocation" Text='<%#Bind("Name")%>' Checked='<%#Bind("IsSelected")%>' AutoPostBack="True" OnCheckedChanged="CheckBoxListLocationsOnSelectedIndexChanged" runat="server">
                        </asp:CheckBox>
                    </ItemTemplate>
                    <SeparatorTemplate>
                        </br></SeparatorTemplate>
                </asp:Repeater>
            </td>
            <td class="generate">
                <asp:Label ID="Label1" runat="server" Text="Количество людей в рассылке:"></asp:Label>
                <asp:TextBox ID="TextBoxPersonsCount" runat="server" ReadOnly="True" Width="55px"
                    CssClass="rigthAlign">0</asp:TextBox>
                <br />
                <asp:HyperLink ID="HyperLinkMailTo" runat="server">Отправить письмо</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td class="list">
                <div class="header">
                    Список исключений</div>
                
                <asp:DropDownList ID="DropDownListPersons" runat="server">
                </asp:DropDownList>
                <asp:Button ID="ButtonExclude" runat="server" OnClick="ButtonExcludeClick" Text="Исключить" />
                <asp:Button ID="ButtonClear" runat="server" OnClick="ButtonClearClick" Text="Очистить" />
                <%--<asp:LinkButton ID="LinkButtonMailto" runat="server" OnClick="ButtonMailTo_Click" PostBackUrl="mailto:an2114@mail.ru">LinkButton</asp:LinkButton>--%>
                <br />
                <asp:Label ID="LabelExcluded" runat="server" ReadOnly="True" Height="150" Width="300"
                    CssClass="personList" Visible="False"></asp:Label>
                <asp:Repeater ID="RepeaterPersons" runat="server" OnItemCommand="OnLinkButtonCommand">
                    <ItemTemplate>
                        <asp:Label runat="server"><%#Eval("DisplayName")%></asp:Label>
                        <asp:LinkButton runat="server" CommandName="Delete" CommandArgument='<%#Eval("Email")%>'>удалить</asp:LinkButton>
                    </ItemTemplate>
                    <SeparatorTemplate>
                        </br></SeparatorTemplate>
                </asp:Repeater>
            </td>
        </tr>
    </table>
    </form>
    <%--</asp:Content>--%>
</body>
