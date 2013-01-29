using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using Microsoft.Office.Interop.Outlook;
using System.Runtime.InteropServices;
using DLExt.Builder;
using DLExt.Builder.Model;
using DLExt.Builder.Retrievers;

namespace DLExt.Outlook2010AddIn
{
     public partial class MainWindow : INotifyPropertyChanged
    {
        private readonly BackgroundWorker loadingWorker;
        private readonly BackgroundWorker composeWorker;
        private readonly BackgroundWorker filterWorker;
        private AddressBuilder builder;
        private IList<Location> locationsList;
        private IList<Person> personsList;
        private IList<Person> localPersonsList;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Server { get; private set; }

        public string LocationsRootPath { get; private set; }

        public string PersonsRootPath { get; private set; }

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

        public MainWindow(string server, string locationsRootPath, string personsRootPath)
        {
            Server = server;
            LocationsRootPath = locationsRootPath;
            PersonsRootPath = personsRootPath;

            loadingWorker = new BackgroundWorker();
            loadingWorker.DoWork += (o, args) =>
            {
                IsProcessing = true;
                locationsList = new LocationsRetriever(Server).Retrieve(LocationsRootPath);
                personsList = new PersonsRetriever(Server).Retrieve(PersonsRootPath);
            };

            loadingWorker.RunWorkerCompleted += (sender, args) =>
            {
                locations.ItemsSource = locationsList;
                persons.ItemsSource = personsList;
                IsProcessing = false;
            };

            filterWorker = new BackgroundWorker();
            filterWorker.DoWork += (sender, args) =>
            {
                IsProcessing = true;
                builder = new AddressBuilder(
                    Server,
                    locations.Items.OfType<Location>().Where(loc => loc.IsSelected),
                    personsToExclude.Items.OfType<Person>());
                localPersonsList = builder.ExtractPersons();
            };

            filterWorker.RunWorkerCompleted += (sender, args) =>
            {
                persons.ItemsSource = localPersonsList;
                IsProcessing = false;
            };

            composeWorker = new BackgroundWorker();
            composeWorker.DoWork += (sender, args) =>
            {
                IsProcessing = true;
                builder = new AddressBuilder(
                    Server,
                    locations.Items.OfType<Location>().Where(loc => loc.IsSelected),
                    personsToExclude.Items.OfType<Person>());
                builder.Build();
            };

            composeWorker.RunWorkerCompleted += (sender, args) =>
            {
                IsProcessing = false;
                try
                {
                    var app = new Microsoft.Office.Interop.Outlook.Application();
                    var mailItem = (MailItem)app.CreateItem(OlItemType.olMailItem);

                    mailItem.To = builder.ResultAddress;
                    mailItem.Display(true);

                }
                catch (COMException)
                {
                }
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
            composeWorker.RunWorkerAsync();
        }

        private void ExcludePerson(object sender, RoutedEventArgs e)
        {
            if (!personsToExclude.Items.Contains(persons.SelectedItem) && persons.SelectedItem != null)
            {
                personsToExclude.Items.Add(persons.SelectedItem);
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
            personsToExclude.Items.Remove(personsToExclude.SelectedItem);
        }

        private void CheckBoxClick(object sender, RoutedEventArgs e)
        {
            filterWorker.RunWorkerAsync();
        }
    }
}

