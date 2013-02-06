<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" Inherits="DLExt.WebApplication.Default"
    ValidateRequest="false" CodeBehind="Default.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="Head1" runat="server">
    <title></title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>
    <script type="text/javascript" src="Scripts/default.js"></script>
    <link href="Content/bootstrap.css" rel="stylesheet" media="screen" id="commonstyle">
    <script type="text/javascript" src="Scripts/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
</head>
<body>
    <div class="page-header well" style="cursor: pointer; display: block;" onclick="javascript:window.location.href='../Default.aspx'">
        <h2>
            <img src="Images/fl_logo.png" alt="First Line Software" width="202" height="44">
            Составитель списка рассылки <small>cоздавай рассылки правильно</small></h2>
    </div>
    <div class="container">
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
                    <div style="display: block;">
                        Количество людей в рассылке: <span id="personsCountLabel">0</span></div>
                    <a id="sendLetter" href="mailto:">Отправить письмо</a>
                </td>
            </tr>
            <tr>
                <td class="list">
                    <div class="header">
                        Список исключений</div>
                    <div>
                        <select id="personsDropDownList" style="margin: auto;" class="dropdown" />
                        <input type="button" id="excludeButton" value="Исключить" class="btn" />
                        <input type="button" id="clearButton" value="Очистить" class="btn" />
                    </div>
                    <div id="excludedPersons" style="width: 340px; height: 170px; overflow: auto">
                    </div>
                </td>
            </tr>
        </table>
        </form>
    </div>
    <div class="modal-footer well" style="cursor: pointer;" onclick="javascript:window.location.href='../Default.aspx'">
        ©&nbsp;2010&nbsp;–&nbsp;2013 First Line Software, Inc. Все права защищены.
    </div>
</body>
