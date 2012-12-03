using System;
using System.Windows.Forms;
using Microsoft.Office.Core;

namespace DLExt.OutlookAddin
{
    public partial class ThisAddIn
    {
        static readonly Timer Timer = new Timer();
        
        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            // HACK: looks like it is the only way to deal with minimized outlook startup
            Timer.Interval = 100;
            Timer.Tick += (o, args) =>
                              {
                                  var activeExplorer = Application.ActiveExplorer();
                                  if (activeExplorer != null)
                                  {
                                      Timer.Stop();
                                      CreateToolbar();
                                  }
                              };
            Timer.Start();
        }

        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {
            RemoveToolbar();
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion

        private const string MenuToolbarTag = "DLExtBar";
        private CommandBar toolBar;
        private CommandBarButton toolBarButton;

        private CommandBar FindBar()
        {
            var activeExplorer = Application.ActiveExplorer();
            return activeExplorer != null
                       ? (CommandBar) activeExplorer.CommandBars.FindControl(missing, missing, MenuToolbarTag, true)
                       : null;
        }

        private void RemoveToolbar()
        {
            var commandBar = FindBar();
            if (commandBar != null)
            {
                commandBar.Delete();
            }
        }

        private void CreateToolbar()
        {
            try
            {
                toolBar = FindBar() 
                    ?? Application.ActiveExplorer().CommandBars.Add(MenuToolbarTag, MsoBarPosition.msoBarTop, false, true);

                toolBarButton = (CommandBarButton)toolBar.Controls.Add(MsoControlType.msoControlButton, missing, missing, 1, true);
                toolBarButton.Style = MsoButtonStyle.msoButtonIconAndCaption;
                toolBarButton.Caption = "Generate Distribution List";
                toolBarButton.FaceId = 65;
                toolBarButton.Tag = MenuToolbarTag;
                toolBarButton.Click += (CommandBarButton ctrl, ref bool @default) =>
                {
                    MainWindow window = new MainWindow(
                        "NNVDC01",
                        "OU=Sites,OU=Company,DC=domain,DC=corp",
                        "OU=Sites,OU=Company,DC=domain,DC=corp");
                    window.Show();
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error Message");
            }
        } 
    }
}
