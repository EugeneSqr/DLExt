using System;
using System.Collections.Generic;
using System.Text;
using DLExt.Model;
using log4net;

namespace DLExt.Infrastructure
{
    public class AddressBuilder
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AddressBuilder));

        public string ResultAddress { get; private set; }

        private readonly IList<Person> persons;

        public AddressBuilder(IList<Person> persons)
        {
            this.persons = persons;
            ResultAddress = string.Empty;
        }

        public void Build()
        {
            try
            {
                BuildDistributionList();
            }
            catch(Exception exception)
            {
                Logger.Error("Error during address string building", exception);
            }
        }

        protected virtual void BuildDistributionList()
        {
            Logger.Info("Building distribution list");
            var builder = new StringBuilder();
            foreach (Person person in persons)
            {
                builder.Append(person.Email).Append(';');
            }

            ResultAddress = builder.ToString();
        }
    }
}
