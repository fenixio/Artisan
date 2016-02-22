using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository;

namespace Artisan.Tools.Logger
{
    public class LogConfig
    {

        private PatternLayout patternLayout;
        private ManagedColoredConsoleAppender consoleAppender;
        private RollingFileAppender rollingAppender;
        private ILoggerRepository repository;
        private IAppender[] appenders;
        private log4net.Core.Level rootLevel;


        public LogConfig()
        {
            rootLevel = log4net.Core.Level.Debug;

            patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%d %-7p [TID=%3t] %m%n";
            
            consoleAppender = new ManagedColoredConsoleAppender();
            consoleAppender.Layout = patternLayout;
            consoleAppender.AddMapping(new ManagedColoredConsoleAppender.LevelColors() { ForeColor = ConsoleColor.Red, BackColor=ConsoleColor.White, Level = log4net.Core.Level.Fatal });
            consoleAppender.AddMapping(new ManagedColoredConsoleAppender.LevelColors() { ForeColor = ConsoleColor.Red, Level = log4net.Core.Level.Error});
            consoleAppender.AddMapping(new ManagedColoredConsoleAppender.LevelColors() { ForeColor = ConsoleColor.Yellow, Level = log4net.Core.Level.Warn });
            consoleAppender.AddMapping(new ManagedColoredConsoleAppender.LevelColors() { ForeColor = ConsoleColor.Green, Level = log4net.Core.Level.Info});
            consoleAppender.AddMapping(new ManagedColoredConsoleAppender.LevelColors() { ForeColor = ConsoleColor.White, Level = log4net.Core.Level.Debug});
            consoleAppender.AddMapping(new ManagedColoredConsoleAppender.LevelColors() { ForeColor = ConsoleColor.Gray, Level = log4net.Core.Level.Verbose });


            rollingAppender = new RollingFileAppender();
            rollingAppender.Layout = patternLayout;
            rollingAppender.MaxFileSize = 10 * 1048576;
            rollingAppender.MaxSizeRollBackups = 20;
            rollingAppender.AppendToFile = true;
            rollingAppender.RollingStyle = RollingFileAppender.RollingMode.Size;
            rollingAppender.StaticLogFileName = true;
            rollingAppender.File = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Assembly.GetEntryAssembly().GetName().Name + ".log");

        }

        public LogConfig SetRootLevel(Level level)
        {
            rootLevel = Log.Get4NetLevel(level);
            return this;
        }

        public LogConfig SetPattern(string pattern)
        {
            patternLayout.ConversionPattern = pattern;
            return this;
        }

        public LogConfig AddColorMap(Level level, ConsoleColor foreColor, ConsoleColor backColor)
        {
            if (consoleAppender != null)
            {
                consoleAppender.AddMapping(new ManagedColoredConsoleAppender.LevelColors()
                {
                    ForeColor = foreColor,
                    BackColor = backColor,
                    Level = Log.Get4NetLevel(level)
                });
            }
            return this;
        }

        public LogConfig NotConsole()
        {
            consoleAppender = null;
            return this;
        }

        public LogConfig NotFile()
        {
            rollingAppender = null;
            return this;
        }

        public LogConfig SetFilePath(string fileName)
        {
            if (rollingAppender != null)
                rollingAppender.File = fileName;
            return this;
        }

        public LogConfig SetMaxFileSize(int sizeInMb)
        {
            if (rollingAppender != null)
                rollingAppender.MaxFileSize = sizeInMb * 1048576;
            return this;
        }

        public LogConfig SetMaxRollBackups(int number)
        {
            if (rollingAppender != null)
                rollingAppender.MaxSizeRollBackups = number;
            return this;
        }

        public void Apply()
        {
            patternLayout.ActivateOptions();
            int count = 0;
            if(rollingAppender!=null)
            {
                rollingAppender.ActivateOptions();
                count++;
            }

            if (consoleAppender != null)
            {
                consoleAppender.ActivateOptions();
                count++;
            }
            appenders = new IAppender[count];
            count = 0;
            if (rollingAppender != null) 
            {
                appenders[count] = rollingAppender;
                count++;
            }

            if (consoleAppender != null)
            {
                appenders[count] = consoleAppender;
                count++;
            }


            repository = LogManager.GetRepository();
            IBasicRepositoryConfigurator basicRepositoryConfigurator = repository as IBasicRepositoryConfigurator;
            if (basicRepositoryConfigurator != null)
            {
                ((log4net.Repository.Hierarchy.Hierarchy)repository).Root.Level = rootLevel;
                basicRepositoryConfigurator.Configure(appenders);
            }

            Log.Write(Level.Info, "Configuration applied. Logger ready");
        }

    }
}
