using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Data;
using Zolilo.Accounts;
using Zolilo.Web;
using Zolilo.Security;
using System.Text.RegularExpressions;

namespace Zolilo.Pages
{
    public partial class Page_Register : ZoliloPage
        {
        protected override void OnInit(EventArgs e)
        {
            ButtonSubmit.Click += new EventHandler(ButtonSubmit_Click);
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
                Register();
        }

        private void Register()
        {
            AccountRegistrationFormParameters parameters;
            FunctionResponse response;

            parameters = 
                new AccountRegistrationFormParameters(
                TextBoxUserName.Text.ToLower(),
                TextBoxPassword.Text,
                TextBoxConfirmPassword.Text,
                TextBoxEmail.Text);

            response = RegisterAccount(parameters);
            if (response.FunctionSucceeded)
            {
                TextBoxResult.Text = "Account " + parameters.Username + " registered successfully.";
            }
            else
            {
                TextBoxResult.Text = response.ResponseText;
            }
        }

        //Parameters should equal all items on the register form
        /// <summary>
        /// Registers an account and returns a copy of the account
        /// </summary>
        private FunctionResponse RegisterAccount(AccountRegistrationFormParameters accountParameters)
        {
            FunctionResponse response = new FunctionResponse();
            DR_Accounts user;

            user = GetUser(accountParameters.Username);
            if (user.Cells["ID"].Data != null)
            {
                response.FunctionSucceeded = false;
                response.ResponseText = "User " + accountParameters.Username + " already exists!  Please choose a different username.";
                return response;
            }

            //INSERT INTO TABLE
            DR_Accounts record = (DR_Accounts)accountParameters.GenerateNewDataRecord();
            record.SaveChanges();
            response.FunctionSucceeded = true;

            return response;
        }

        private DR_Accounts GetUser(string name)
        {
            DDOQuery<DR_Accounts> query = new DDOQuery<DR_Accounts>();
            query.Object._Username = name;
            return query.PerformQuery();
        }
        /// <summary>
        /// DEFINES the parameters to be used WHEN CREATING A NEW ACCOUNT.
        /// MODIFYING THESE PARAMETERS AND ITS CONSTRUCTOR WILL FORCE PARAMETER UPDATES IN REST OF PROJECT 
        /// (this is a feature, not a bug)
        /// </summary>
        internal class AccountRegistrationFormParameters : FormParameters
        {
            string username;

            public string Username
            {
                get { return username; }
            }
            string password;

            public string Password
            {
                get { return password; }
            }
            string confirmpassword;

            public string Confirmpassword
            {
                get { return confirmpassword; }
            }
            string email;

            public string Email
            {
                get { return email; }
            }

            DR_Accounts newRecord;

            /// <summary>
            /// Keep this private to force parameters to be passed on construction
            /// </summary>
            private AccountRegistrationFormParameters()
                : base()
            {
            }

            /// <summary>
            /// Use this constructor to set up the parameters and the validation
            /// </summary>
            internal AccountRegistrationFormParameters(
                string username,
                string password,
                string confirmpassword,
                string email)
                : base()
            {
                this.username = username;
                this.password = password;
                this.confirmpassword = confirmpassword;
                this.email = email;

                Regex regexusername = new Regex(@"^[0-9a-zA-Z]{3,30}$");
                Regex regexpassword = new Regex(@"^.{8,30}$");
                Regex regexemail = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");

                if (password != confirmpassword)
                    throw new SecurityValidationException("");

                if (regexusername.Matches(username).Count == 0)
                    throw new SecurityValidationException("");

                if (regexpassword.Matches(password).Count == 0)
                    throw new SecurityValidationException("");

                if (regexemail.Matches(email).Count == 0)
                    throw new SecurityValidationException("");

                newRecord = (DR_Accounts)GenerateNewDataRecord();
            }

            /// <summary>
            /// OVERRIDDEN -- Translates Form Abstraction Layer to Data Abstraction Layer
            /// </summary>
            /// <returns></returns>
            internal override DataRecord GenerateNewDataRecord()
            {
                DR_Accounts record = new DR_Accounts();
                record._Username = username;
                record._PCode = SecurityEncryption.ComputeHash(password);
                record._Email = email;
                return record;
            }

        }   

    }
}