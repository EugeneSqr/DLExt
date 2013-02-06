<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" Inherits="DLExt.WebApplication.Default"
    ValidateRequest="false" CodeBehind="Default.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>
    <script type="text/javascript" src="Scripts/default.js"></script>
</head>
<body>
    <form id="Form1" runat="server">
    <table>
        <tr>
            <td class="locations" rowspan="2">
                <div class="header">
                    Отделения</div>
                <div id="checkBoxListLocations">
                </div>
            </td>
            <td class="generate">
                <asp:Label ID="Label1" runat="server" Text="Количество людей в рассылке:"></asp:Label>
                <label id="personsCountLabel">
                    0</label>
                <br />
                <a id="sendLetter" href="mailto:">Отправить письмо</a>
            </td>
        </tr>
        <tr>
            <td class="list">
                <div class="header">
                    Список исключений</div>
                <select id="personsDropDownList">
                </select>
                <input type="button" id="excludeButton" value="Исключить" />
                <input type="button" id="clearButton" value="Очистить" />
                <br />
                <asp:Label ID="LabelExcluded" runat="server" ReadOnly="True" Height="150" Width="300"
                    CssClass="personList" Visible="False"></asp:Label>
                <asp:Repeater ID="RepeaterPersons" runat="server" OnItemCommand="OnLinkButtonCommand"
                    Visible="False">
                    <ItemTemplate>
                        <asp:Label runat="server"><%#Eval("DisplayName")%></asp:Label>
                        <asp:LinkButton runat="server" CommandName="Delete" CommandArgument='<%#Eval("Email")%>'>удалить</asp:LinkButton>
                    </ItemTemplate>
                    <SeparatorTemplate>
                        <br></br>
                    </SeparatorTemplate>
                </asp:Repeater>
                <div id="excludedPersons"style="width: 340px; height: 170px; overflow: auto">
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
