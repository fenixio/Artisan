using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artisan.Tools.Configurator
{
    public class ConfigHandler : IList<IConfigSection>
    {
        public List<IConfigSection> sections;
        public Dictionary<string, int> sectionIdx;

        public ConfigHandler()
        {
            sections = new List<IConfigSection>();
            sectionIdx = new Dictionary<string, int>();
        }

         public IConfigSection this[int index]
        {
            get
            {
                return sections[index];
            }

            set
            {
                sections[index] = value;
                sectionIdx[value.SectionName] = index;
            }
        }

        public IConfigSection this[string name]
        {
            get
            {
                int index;
                if (!sectionIdx.TryGetValue(name, out index))
                    return sections[sectionIdx[name]];
                else
                    return null;
            }

            set
            {
                int index;
                if (!sectionIdx.TryGetValue(name, out index))
                {
                    index = sections.Count;
                    sections.Add(value);
                }
                else
                {
                    sections[index] = value;
                }
                sectionIdx[value.SectionName] = index;

            }
        }

        public int Count
        {
            get
            {
                return sections.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(IConfigSection item)
        {
            int index = sections.Count;
            sections.Add(item);
            sectionIdx[item.SectionName] = index;
        }

        public void Clear()
        {
            sections.Clear();
            sectionIdx.Clear();
        }

        public bool Contains(IConfigSection item)
        {
            return sections.Contains(item);
        }

        public bool Contains(string name)
        {
            return sectionIdx.ContainsKey(name);
        }

        public void CopyTo(IConfigSection[] array, int arrayIndex)
        {
            sections.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IConfigSection> GetEnumerator()
        {
            return sections.GetEnumerator();
        }

        public int IndexOf(IConfigSection item)
        {
            return sections.IndexOf(item);
        }

        public int IndexOf(string name )
        {
            if (sectionIdx.ContainsKey(name))
                return sectionIdx[name];
            else
                return -1;
        }

        public void Insert(int index, IConfigSection item)
        {
            sections.Insert(index, item);
            ReIndex();
        }

        public bool Remove(IConfigSection item)
        {
            bool retVal =  sections.Remove(item);
            ReIndex();
            return retVal;
        }

        public void RemoveAt(int index)
        {
            sections.RemoveAt(index);
            ReIndex();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator(); 
        }

        private void ReIndex()
        {
            sectionIdx.Clear();
            int i = 0;
            foreach (var section in sections)
            {
                sectionIdx[section.SectionName] = i++;
            }
        }
    }
}
