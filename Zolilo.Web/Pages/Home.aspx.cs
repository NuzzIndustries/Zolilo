using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Data;
using Zolilo.Web;
using System.Text;
using Zolilo.Pages;

namespace Zolilo
{
    public partial class _Default : ZoliloPage
    {
        protected override void OnInit(EventArgs e)
        {
            //tab.Tabs.Add(new ZoliloTab("Tab1"));
            //tabs.Tabs.Add(new ZoliloTab("Tab2"));
            
            //((SiteMaster)Master).ID = "MasterHome";
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetUFrame("/browse");

            //RegisterScript("MainFrameExists", "return true;");
            string idstring = Request.QueryString["x"];
            if (idstring != null)
            {
                string cmdstring = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(idstring));
                Command(cmdstring);
            }
        }

        protected void LoginLabel_PreRender(object sender, EventArgs e)
        {
            if (zContext.Session.LoggedIn)
            {
                string name;
                if (zContext.Session.Flags.OpenIDNotLinked)
                    name = "Authenticated, but OpenID not linked.";
                else
                    name = "Logged in as " + zContext.Session.CurrentAccount._Username;

                LoginLabel.Text = name + "<br><a href=\"/account/settings\">Account Settings</a> | <a href=\"/account/logout\">Logout</a>";
            }
            else
            {
                LoginLabel.Text = "<a href=\"/account/login\">Login</a> | <a href=\"/account/register\">Register</a>";
            }
        }


        internal void Command(string command)
        {
            List<string> tokens = new List<string>(command.Split(' '));
            if (tokens.Count > 0)
            {
                string cmd = tokens[0];
                tokens.Remove(tokens[0]);
                switch (cmd)
                {
                    case "loadpage":
                        LoadPage(tokens);
                        break;
                    default:
                        break;
                }
            }

            ClientScriptManager script = ClientScript;

            String csname1 = "PopupScript";
            Type cstype = GetType();

            System.Text.StringBuilder cstext1 = new System.Text.StringBuilder();
            script.RegisterStartupScript(cstype, csname1, cstext1.ToString());
        }

        private void LoadPage(List<string> tokens)
        {
            if (tokens.Count > 0)
                LoadPage(tokens[0]);
        }

        private void LoadPage(string token)
        {
            //Add supervisor id to url
            token = WebDirector.Instance.AddQueryString(token, "supervisor", SupervisorID.ToString());

            Body.Attributes["src"] = token;
            SetUFrame(token);
            currentURL.QueryString = "";
            currentURL.PageURL = token;
        }

        /// <summary>
        /// Sets the location which the UFrame will use as the src tag.  This must be done before the page loads
        /// </summary>
        /// <param name="pagelocation"></param>
        private void SetUFrame(string pagelocation)
        {
            //tabs.SelectedTab.SetURL(pagelocation);
            LiteralBody.Text = "<div id=\"Body\" runat=\"server\" src=\"" + pagelocation + "\" style=\"position: relative; top: 0px;\"></div>";
        }

        /// <summary>
        /// Sets the location which will be used to modify UFrame source reference
        /// </summary>
        /// <param name="pagelocation"></param>
        internal void SetUFrameLOC(string pagelocation)
        {

        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            Command("TEST");
        }

        protected void TabContainer1_Load(object sender, EventArgs e)
        {
            //tc.Visible = false;
        }
    }
}
