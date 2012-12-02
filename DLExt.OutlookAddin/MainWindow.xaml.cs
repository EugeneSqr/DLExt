using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using DLExt.Builder;
using DLExt.Builder.Model;
using DLExt.Builder.Retrievers;
using Microsoft.Office.Interop.Outlook;

namespace DLExt.OutlookAddin
{
    public partial class MainWindow
    {
        private readonly BackgroundWorker worker;
        private IList<Location> locationsList;
        private IList<Person> personsList;

        public string Server { get; private set; }

        public string LocationsRootPath { get; private set; }

        public string PersonsRootPath { get; private set; }

        public MainWindow(string server, string locationsRootPath, string personsRootPath)
        {
            Server = server;
            LocationsRootPath = locationsRootPath;
            PersonsRootPath = personsRootPath;

            worker = new BackgroundWorker();
            worker.DoWork += (o, args) =>
            {
                locationsList = new LocationsRetriever(Server).Retrieve(LocationsRootPath);
                personsList = new PersonsRetriever(Server).Retrieve(PersonsRootPath);
            };

            worker.RunWorkerCompleted += (sender, args) =>
            {
                locations.ItemsSource = locationsList;
                persons.ItemsSource = personsList;
            };

            worker.WorkerSupportsCancellation = true;
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            worker.RunWorkerAsync();
        }

        private void ComposeEmail(object sender, RoutedEventArgs e)
        {
            var addressBuilder = new AddressBuilder(
                Server,
                locations.Items.OfType<Location>().Where(loc => loc.IsSelected),
                personsToExclude.Items.OfType<Person>());

            addressBuilder.Build();
            var app = new Microsoft.Office.Interop.Outlook.Application();
            var mailItem = (MailItem)app.CreateItem(OlItemType.olMailItem);

            mailItem.To = addressBuilder.ResultAddress;
            mailItem.Display(true);
        }

        private void ExcludePerson(object sender, RoutedEventArgs e)
        {
            personsToExclude.Items.Add(persons.SelectedItem);
        }

        private void CloseForm(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            if (worker.IsBusy)
            {
                worker.CancelAsync();
            }
        }
    }
}
