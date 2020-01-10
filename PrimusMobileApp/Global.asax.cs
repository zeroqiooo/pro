using GAIA.Platform.AppDomainManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace PrimusMobileApp
{
    public class Global : System.Web.HttpApplication
    {
        private System.ComponentModel.IContainer components = null;
        public static Hashtable ModuleList = new Hashtable();

        public Global()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            LSAManager.InitDomain();
            //定义暂时保存文化设置的变量
            string CultureName = Thread.CurrentThread.CurrentCulture.Name;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CultureName);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(CultureName);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //定义暂时保存文化设置的变量
            string CultureName = Thread.CurrentThread.CurrentCulture.Name;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CultureName);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(CultureName);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //定义暂时保存文化设置的变量
            string CultureName = Thread.CurrentThread.CurrentCulture.Name;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CultureName);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(CultureName);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}