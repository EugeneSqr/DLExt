using System;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

namespace DLExt.WebApplication
{
    public partial class Default : System.Web.UI.Page
    {
        private Controller controller;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.controller = new Controller();
            this.Load += this.OnLoad;
            this.PreRender += OnPreRender;
        }

        protected void CheckBoxListLocationsOnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            this.CreatePersonDropDownList();
        }

        protected void ButtonClearClick(object sender, EventArgs e)
        {
            this.controller.ClearExcludedPersonsList();
        }

        protected void ButtonExcludeClick(object sender, EventArgs e)
        {
            //this.controller.AddExcludedPersonByEmail(DropDownListPersons.SelectedItem.Value);
        }

        protected void OnLinkButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                this.controller.DeleteExcludedPersonByEmail(e.CommandArgument.ToString());
            }
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            controller.LoadAvailableLocations();
            //foreach (RepeaterItem item in RepeaterLocations.Items)
            //{
            //    var location = (CheckBox)item.FindControl("CheckBoxLocation");
            //    controller.SetLocationSelected(location.Text, location.Checked);
            //}

            controller.LoadPersonsList();

            controller.LoadExcludedPersonsFromCookies(ExcludedPersonsCookie);
        }

        private void OnPreRender(object sender, EventArgs eventArgs)
        {
            ExcludedPersonsCookie = controller.ConvertExcludedPersonsToCookieValue();

            //RepeaterLocations.DataSource = controller.Locations;
            //RepeaterLocations.DataBind();

            //int personsInList;
            //var emailList = this.controller.GetEmailList(out personsInList);

            //if (emailList.Equals(string.Empty))
            //{
            //    HyperLinkMailTo.Enabled = false;
            //    HyperLinkMailTo.Text = "Выберите хотя бы одно отделение";
            //}
            //else
            //{
            //    HyperLinkMailTo.Enabled = true;
            //    HyperLinkMailTo.Text = "Отправить письмо";
            //}

            //HyperLinkMailTo.NavigateUrl = "mailto:" + emailList;
            //TextBoxPersonsCount.Text = personsInList.ToString(CultureInfo.InvariantCulture);

            //RepeaterPersons.DataSource = controller.ExcludedPersons;
            //RepeaterPersons.DataBind();

            //if (DropDownListPersons.DataSource == null)
            //{
            //    CreatePersonDropDownList();
            //}
        }

        private void CreatePersonDropDownList()
        {
            //this.DropDownListPersons.DataSource = this.controller.Persons;
            //this.DropDownListPersons.DataTextField = "DisplayName";
            //this.DropDownListPersons.DataValueField = "Email";
            //this.DropDownListPersons.DataBind();
        }

        private string ExcludedPersonsCookie
        {
            get
            {
                var httpCookie = this.Request.Cookies[CookieKeys.ExcludedPersons];
                if (httpCookie == null)
                {
                    return string.Empty;
                }

                return httpCookie.Value;
            }

            set
            {
                var httpCookie = new HttpCookie(CookieKeys.ExcludedPersons, value);
                this.Response.Cookies.Set(httpCookie);
            }
        }
    }
}
