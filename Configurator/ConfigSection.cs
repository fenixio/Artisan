﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artisan.Tools.Configurator
{
    public abstract class ConfigSection : IConfigSection
    {
        public virtual string SectionName { get; set; }
    }
}
