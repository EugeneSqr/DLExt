using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using DLExt.Infrastructure;
using DLExt.Model;
using Microsoft.Office.Interop.Outlook;
using System.Runtime.InteropServices;
using log4net;

namespace DLExt.Outlook2010AddIn
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MainWindow));

        private readonly BackgroundWorker loadingWorker;
        private IList<Person> allPersons;
        private IList<Location> locations;

        public event PropertyChangedEventHandler PropertyChanged;

        private bool isProcessing;

        public bool IsProcessing
        {
            get { return isProcessing; }
            set
            {
                isProcessing = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsProcessing"));
            }
        }

        public MainWindow(string server)
        {
            var repository = new Repository(server);

            Logger.InfoFormat(
                "GUI: main window has been created. Parameters: {0}",
                server);

            loadingWorker = new BackgroundWorker();
            loadingWorker.DoWork += (o, args) =>
            {
                IsProcessing = true;
                Logger.Info("WorkerThread: retrieving locations.");
                locations = repository
                    .GetLocations()
                    .Select(item => new Location { Name = item })
                    .ToList();
                Logger.Info("WorkerThread: retrieving persons.");
                allPersons = repository.GetPersons();
            };

            loadingWorker.RunWorkerCompleted += (sender, args) =>
            {
                Logger.Info("GUI: loading of locations and persons completed.");
                lbLocations.ItemsSource = locations;
                Logger.Info("GUI: persons and locations data has been saved.");
                IsProcessing = false;
            };

            loadingWorker.WorkerSupportsCancellation = true;
            DataContext = this;
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            loadingWorker.RunWorkerAsync();
        }

        private void ComposeEmail(object sender, RoutedEventArgs e)
        {
            Logger.Info("GUI: creating result email");
            IsProcessing = false;
            try
            {
                var app = new Microsoft.Office.Interop.Outlook.Application();
                var mailItem = (MailItem)app.CreateItem(OlItemType.olMailItem);
                var builder = new AddressBuilder(cbPersons.Items.OfType<Person>().ToList());
                builder.Build();
                mailItem.To = builder.ResultAddress;
                mailItem.Display(true);

            }
            catch (COMException exception)
            {
                Logger.Error("Error creating email: ", exception);
            }

            Logger.Info("GUI: email has been created.");
        }

        private void ExcludePerson(object sender, RoutedEventArgs e)
        {
            var personToExclude = cbPersons.SelectedItem as Person;
            if (personToExclude != null && !lbPersonsToExclude.Items.Contains(personToExclude))
            {
                lbPersonsToExclude.Items.Add(personToExclude);
                cbPersons.Items.Remove(cbPersons.SelectedItem);
            }
        }

        private void CloseForm(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            if (loadingWorker.IsBusy)
            {
                loadingWorker.CancelAsync();
            }
        }

        private void LabelMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            lbPersonsToExclude.Items.Remove(lbPersonsToExclude.SelectedItem);
            cbPersons.Items.Clear();
            foreach (Location location in lbLocations.Items)
            {
                if (location.IsSelected)
                {
                    foreach (var person in allPersons)
                    {
                        if (person.Location == location.Name && !lbPersonsToExclude.Items.Contains(person))
                        {
                            cbPersons.Items.Add(person);
                        }
                    }
                }
            }
        }

        private void CheckBoxClick(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null && checkBox.IsChecked.HasValue)
            {
                if (checkBox.IsChecked.Value)
                {
                    foreach (var person in allPersons)
                    {
                        if (person.Location == checkBox.Content.ToString() && !lbPersonsToExclude.Items.Contains(person))
                        {
                            cbPersons.Items.Add(person);
                        }
                    }
                }
                else
                {
                    foreach (var person in allPersons)
                    {
                        if (person.Location == checkBox.Content.ToString())
                        {
                            cbPersons.Items.Remove(person);
                        }
                    }
                }
            }
        }
    }
}

