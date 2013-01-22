<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RecentDoc.ascx.cs" Inherits="RecentDoc.RecentDoc.RecentDoc" %>
<style type="text/css">
    .container
    {
        width: 360px;
    }
    .container .content
    {
        float: left;
        width: 350px;
    }
    .container .right
    {
        float: left;
        width: 150px;
    }
    .container .row
    {
        margin-bottom: 10px;
    }
    .container .header
    {
        font-weight: bold;
    }
    .container .clear
    {
        clear: both;
        float: none;
    }
    .content span
    {
        font-weight: bold;
        font-size: 1.2em;
        color: #96172E;
    }
</style>
<%
    try
    {
        System.Data.DataTable result = ViewState["Result"] as System.Data.DataTable;
        if (result != null && result.Rows.Count> 0)
        {
%>
<div class="container">
    <div class="row header">
        <div class="content">
            <span>What's New</span>
            <br />
            <br />
            Uploaded & updated documents during the last 30 days
        </div>
        <%--<div class="right">
                Site
            </div>--%>
        <div class="clear">
        </div>
    </div>
    <% foreach (System.Data.DataRow row in result.Rows)
       {%>
    <div class="row">
        <div class="content">
            <%
                if (row["DocLink"].ToString().IndexOf(".pdf")> 0)
                {

            %>
            <a href="javascript:showDialog('<%=row["DocLink"].ToString()%>')">
                <%-- <a target="_blank" href='<%= row["DocLink"].ToString() %>'>--%>
                <%=row["Docname"].ToString()%>
            </a>
            <%
                }
                else if (row["DocLink"].ToString().IndexOf(".ppt")> 0)
                {
            %>
            <a href="<%= row["WebURL"]%>/_layouts/PowerPoint.aspx?PowerPointView=ReadingView&PresentationId=<%=row["DocLink"].ToString()%>&">
                <%-- <a target="_blank" href='<%= row["DocLink"].ToString() %>'>--%>
                <%=row["Docname"].ToString()%>
            </a>
            <%
                }
                
                
                
                
                
                else if (row["DocLink"].ToString().IndexOf(".xls")> 0 || row["DocLink"].ToString().IndexOf(".xlsx")> 0)
                {
            %>
            <a href="<%= row["WebURL"]%>/_layouts/xlviewer.aspx?Id=<%=row["DocLink"].ToString()%>&">
                <%-- <a target="_blank" href='<%= row["DocLink"].ToString() %>'>--%>
                <%=row["Docname"].ToString()%>
            </a>
            <%
                }
                
                else
                {
            %>
            
            <a href="<%= row["WebURL"] %>/_layouts/WordViewer.aspx?id=<%=row["DocLink"].ToString()%>&">
                <%-- <a target="_blank" href='<%= row["DocLink"].ToString() %>'>--%>
                <%=row["Docname"].ToString()%>
            </a>
            <%
                }

            %>
        </div>
        <%-- <div class="right"><%= row["Sitename"].ToString() %></div>--%>
        <div class="clear">
        </div>
    </div>
    <% } %>
</div>
<% }
        else
        { %>
<div class="container">
    <ul>
        <li>There is no documents available. </li>
    </ul>
</div>
<% }
    }
    catch (Exception)
    {
    }
%>
<asp:Label ID="Label1" runat="server"></asp:Label>