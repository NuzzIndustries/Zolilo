using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Web;
using Zolilo.Accounts;
using Zolilo.Data;

namespace Zolilo.Pages
{
    public partial class Login_OpenID : ZoliloPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            //Remove ~ from URLs
            OpenIdButtonGoogle.RealmUrl = OpenIdButtonGoogle.RealmUrl.Substring(1);
            OpenIdButtonGoogle.ReturnToUrl = OpenIdButtonGoogle.ReturnToUrl.Substring(1);
            OpenIdButtonGoogle.ImageUrl = OpenIdButtonGoogle.ImageUrl.Substring(1);

            OpenIdButtonYahoo.RealmUrl = OpenIdButtonYahoo.RealmUrl.Substring(1);
            OpenIdButtonYahoo.ReturnToUrl = OpenIdButtonYahoo.ReturnToUrl.Substring(1);
            OpenIdButtonYahoo.ImageUrl = OpenIdButtonYahoo.ImageUrl.Substring(1);

            OpenIdButtonOpenID.RealmUrl = OpenIdButtonOpenID.RealmUrl.Substring(1);
            OpenIdButtonOpenID.ReturnToUrl = OpenIdButtonOpenID.ReturnToUrl.Substring(1);
            OpenIdButtonOpenID.ImageUrl = OpenIdButtonOpenID.ImageUrl.Substring(1);
        }

        protected void OpenIdButtonGoogle_LoggedIn(object sender, DotNetOpenAuth.OpenId.RelyingParty.OpenIdEventArgs e)
        {
            OnLoggedIn(sender, e);
        }

        protected void OpenIdButtonYahoo_LoggedIn(object sender, DotNetOpenAuth.OpenId.RelyingParty.OpenIdEventArgs e)
        {
            OnLoggedIn(sender, e);
        }

        protected void OpenIdButtonOpenID_LoggedIn(object sender, DotNetOpenAuth.OpenId.RelyingParty.OpenIdEventArgs e)
        {
            OnLoggedIn(sender, e);
        }

        void OnLoggedIn(object sender, DotNetOpenAuth.OpenId.RelyingParty.OpenIdEventArgs e)
        {
            if (e.Response.Status == DotNetOpenAuth.OpenId.RelyingParty.AuthenticationStatus.Authenticated && e.Response.ClaimedIdentifier != null)
            {
                AuthenticationInformation user = new AuthenticationInformation();
                user.FriendlyName = e.Response.FriendlyIdentifierForDisplay;
                user.OpenIdentifier = e.Response.ClaimedIdentifier;
                TextBox1.Text = user.FriendlyName + Environment.NewLine + user.OpenIdentifier;
                zContext.Session.OpenIDAuthenticationInformation = user;
                zContext.Session.ResetData();
                if (zContext.Session.Flags.OpenIDNotLinked)
                {
                    TextBox1.Text = "OpenIDNotLinked" + Environment.NewLine + TextBox1.Text;

                    WebDirector.Instance.Redirect("/account/idlink");
                }
                else
                    WebDirector.Instance.Redirect("/home");
            }
        }
    }

    public class AuthenticationInformation
    {
        string friendlyName;

        public string FriendlyName
        {
            get { return friendlyName; }
            set { friendlyName = value; }
        }
        string openIdentifier;

        public string OpenIdentifier
        {
            get { return openIdentifier; }
            set { openIdentifier = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            //sb.AppendLine("AuthenticationInformation");
            sb.AppendLine("OpenIdentifier: " + openIdentifier);
            //sb.AppendLine("FriendlyName: " + friendlyName);
            return sb.ToString();
        }
    }
}