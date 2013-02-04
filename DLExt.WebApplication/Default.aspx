<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" Inherits="DLExt.WebApplication.Default"
    ValidateRequest="false" CodeBehind="Default.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="Form1" runat="server">
    <table>
        <tr>
            <td class="locations" rowspan="2">
                <div class="header">
                    Отделения</div>
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
                <asp:Label ID="LabelPersonsCount" runat="server" Text="0"></asp:Label>
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
                <br />
                <asp:Label ID="LabelExcluded" runat="server" ReadOnly="True" Height="150" Width="300"
                    CssClass="personList" Visible="False"></asp:Label>
                <asp:Panel ID="PanelRepeater" runat="server" ScrollBars="Vertical" Height="170">
                    <asp:Repeater ID="RepeaterPersons" runat="server" OnItemCommand="OnLinkButtonCommand">
                        <ItemTemplate>
                            <asp:Label runat="server"><%#Eval("DisplayName")%></asp:Label>
                            <asp:LinkButton runat="server" CommandName="Delete" CommandArgument='<%#Eval("Email")%>'>удалить</asp:LinkButton>
                        </ItemTemplate>
                        <SeparatorTemplate>
                            </br></SeparatorTemplate>
                    </asp:Repeater>
                </asp:Panel>
            </td>
        </tr>
    </table>
    </form>
</body>
