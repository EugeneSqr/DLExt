using System;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Office.Core;
using log4net;

namespace DLExt.OutlookAddin
{
    public partial class ThisAddIn
    {
        static readonly Timer Timer = new Timer();

        private static readonly ILog Logger = LogManager.GetLogger(typeof(ThisAddIn));
        
        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("DLExt.OutlookAddin.log4net.config"));
            
            Logger.Info("Startup: plugin startup logic has been triggered.");
            // HACK: looks like it is the only way to deal with minimized outlook startup
            Timer.Interval = 100;
            Timer.Tick += (o, args) =>
                              {
                                  Logger.Info("Startup: getting appropriate ActiveExplorer");
                                  var activeExplorer = Application.ActiveExplorer();
                                  if (activeExplorer != null)
                                  {
                                      Logger.Info("Startup: explorer found.");
                                      Timer.Stop();
                                      Logger.Info("Startup: creation toolbar...");
                                      CreateToolbar();
                                      Logger.Info("Startup: creating toolbar. Done.");
                                  }
                              };
            Timer.Start();
        }

        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {
            Logger.Info("Shutdown: plugin deactivation. Removing toolbar.");
            RemoveToolbar();
            Logger.Info("Shutdown: plugin deactivation. Done.");
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

        private const string MenuToolbarTag = "Distribution List Addin";
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
                        "domain.corp");
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
