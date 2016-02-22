using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;

namespace Artisan.Tools.Configurator
{
    class ConfigSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {

            List<IConfigSection> sections = new List<IConfigSection>();

            ConcurrentDictionary<string, Type> sectionTypes = new ConcurrentDictionary<string, Type>();
            GetSectionTypes("*.dll", sectionTypes);
            GetSectionTypes("*.exe", sectionTypes);
            XmlElement sectionElement = section.FirstChild as XmlElement;
            while (sectionElement != null)
            {
                if (sectionTypes.ContainsKey(sectionElement.Name))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer( sectionTypes[sectionElement.Name]);
                    IConfigSection confSection  = (IConfigSection)xmlSerializer.Deserialize(new XmlNodeReader(sectionElement));
                    confSection.SectionName     = sectionElement.Name;
                    sections.Add(confSection );
                }


                sectionElement = sectionElement.NextSibling as XmlElement;
            }

            return sections;
        }


        private void GetSectionTypes(string fileType, ConcurrentDictionary<string, Type> sectionTypes)
        {
            
            Parallel.ForEach(Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, fileType),
            d =>
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(d);
                    if (assembly != null)
                    {
                        Parallel.ForEach(assembly.GetTypes(),
                        t =>
                        {
                            Type interfaceType = t.GetInterface("IConfigSection");
                            if ((interfaceType != null) && (!t.IsInterface))
                            {

                                sectionTypes.TryAdd(t.Name, t);
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    // Create an EventLog instance and assign its source.
                    EventLog myLog = new EventLog();
                    myLog.Source = "Application";

                    // Write an informational entry to the event log.    
                    myLog.WriteEntry(string.Format("Failed to load the assembly {0} to discover plug ins with exception {1}", d, ex.Message));
                }
            });

        }

    }
}
