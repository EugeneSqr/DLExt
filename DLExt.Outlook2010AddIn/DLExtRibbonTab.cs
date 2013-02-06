using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Office = Microsoft.Office.Core;
using System.Windows.Forms;

namespace DLExt.Outlook2010AddIn
{
    [ComVisible(true)]
    public class DLExtRibbonTab : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;

        public DLExtRibbonTab()
        {
        }

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText("DLExt.Outlook2010AddIn.DLExtRibbonTab.xml");
        }

        #endregion

        #region Ribbon Callbacks

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
            this.ribbon.ActivateTab("DLEext");
        }

        public void OnDLExtClick(Office.IRibbonControl control)
        {
            MainWindow window = new MainWindow(
                "domain.corp",
                "OU=Sites,OU=Company,DC=domain,DC=corp",
                "OU=Sites,OU=Company,DC=domain,DC=corp");
            window.Show();
        }

        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
