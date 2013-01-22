using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;

namespace RecentDoc.RecentDoc
{
    [ToolboxItem(false)]
    public partial class RecentDoc : System.Web.UI.WebControls.WebParts.WebPart
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int errorIndex = 0;
                try
                {
                    var siteContext = SPContext.Current.Site;

                    //var impersonationUser = siteContext.RootWeb.AllUsers[0];
                    var site = siteContext;

                    var query = new SPSiteDataQuery
                    {
                        Query = "",
                        ViewFields =
                            "<FieldRef Name=\"LinkFilename\"/>" +
                            "<FieldRef Name=\"Created\"/>" +
                            "<FieldRef Name=\"Modified\"/>" +
                            "<FieldRef Name=\"Editor\"/>" +
                            "<FieldRef Name=\"FileLeafRef\"/>",
                        Lists =
                            "<Lists ServerTemplate=\"101\"/>",
                        RowLimit = 500,
                        Webs =
                            "<Webs Scope=\"SiteCollection\"/>"
                    };
                    //var productiondate = new DateTime(2012, 09, 12);
                    //string proddatestring =
                    //    SPUtility.CreateISO8601DateTimeFromSystemDateTime(
                    //        productiondate.ToUniversalTime());

                    //var date = Convert.ToDateTime(proddatestring);
                    //date.AddMonths(-1).ToUniversalTime();

                    
                    string convertedTime = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now.AddMonths(-1).ToUniversalTime());

                    query.Query =
                        @"<Where>
                                                             <And>
                                                                <Geq>
                                                                    <FieldRef Name='Modified' />
                                                                    <Value IncludeTimeValue='TRUE' Type='DateTime'>" +
                        convertedTime +
                        @"</Value>
                                                                </Geq>
                                                                <Eq>
                                                                    <FieldRef Name='FSObjType'/><Value Type='Integer'>0</Value>
                                                                </Eq>
                                                             </And>
                                                        </Where>";
                    var resultTable = new DataTable();
                    resultTable.Columns.Add("DocLink");
                    resultTable.Columns.Add("Docname");
                    resultTable.Columns.Add("Sitename");
                    resultTable.Columns.Add("WebURL");
                    errorIndex = 1;
                    foreach (DataRow row in site.RootWeb.GetSiteData(query).Rows)
                    {
                        try
                        {
                            string filename = row["LinkFilename"].ToString();
                            string extension = Path.GetExtension(filename);
                            if (extension != null)
                                switch (extension.ToLower())
                                {
                                    case ".jpg":
                                    case ".xsl":
                                    case ".css":
                                    case ".png":
                                    case ".xaml":
                                    case ".gif":
                                    case ".js":
                                        //case ".dotm":
                                        {
                                            continue;
                                        }
                                    default:
                                        {
                                            var newRow = resultTable.NewRow();
                                            using (SPWeb web = site.OpenWeb(new Guid(row["WebId"].ToString())))
                                            {
                                                var item =
                                                    web.Lists[new Guid(row["listId"].ToString())].GetItemById(
                                                        Convert.ToInt32(row["ID"]));
                                                var url = (string) item[SPBuiltInFieldId.EncodedAbsUrl];
                                                newRow["DocLink"] = Uri.EscapeUriString(url);
                                                newRow["Docname"] = filename;
                                                newRow["Sitename"] = web.Name;
                                                newRow["WebURL"] = web.Url;
                                                if (string.IsNullOrEmpty(newRow["Sitename"].ToString()))
                                                    newRow["Sitename"] = "Root Site";
                                                resultTable.Rows.Add(newRow);
                                                break;
                                            }
                                        }
                                }

                            ViewState["Result"] = resultTable;

                        }
                        catch (Exception ex)
                        {
                            Label1.Text += ex.StackTrace;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Label1.Text = "<br />" + ex.Message;
                   // Label1.Text += ex.StackTrace;
                    Label1.Text += "<br />" + "Error message is" + errorIndex;
                }
            }
        }
    }
}