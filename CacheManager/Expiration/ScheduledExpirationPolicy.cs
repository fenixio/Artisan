using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artisan.Tools.CacheManager
{
    public enum SchedulePeriod
    { 
        Second,
        Hour,
        Day
    }
    
    public class ScheduledExpirationPolicy : ExpirationPolicyBase
    {
        public SchedulePeriod Period { get; set; }
        private int days;
        private int hours;
        private int minutes;
        private int seconds;

        public ScheduledExpirationPolicy(int dateBetweenEvents, DateTime onTime)
        {
            this.Period  = SchedulePeriod.Day;
            this.days    = dateBetweenEvents;
            this.hours   = onTime.Hour;
            this.minutes = onTime.Minute;
            this.seconds = onTime.Second;
        }

        public ScheduledExpirationPolicy(int hoursBetweenEvents, int onMinute)
        {
            this.Period  = SchedulePeriod.Hour;
            this.hours   = hoursBetweenEvents;
            this.minutes = onMinute;
            this.seconds = 0;
        }

        public ScheduledExpirationPolicy(int secondsBetweenEvents)
        {
            this.Period  = SchedulePeriod.Second;
            this.hours   = 0;
            this.minutes = 0;
            this.seconds = secondsBetweenEvents;
        }

        public override void Store<TKey, TValue>(CacheItem<TKey, TValue> item)
        {
            DateTime next;
            switch(Period)
            {
                case SchedulePeriod.Day:
                    next = item.CreationDate.Date.
                                        AddDays(this.days).
                                        AddHours(this.hours).
                                        AddMinutes(this.minutes).
                                        AddSeconds(this.seconds); 
                    item.ExpirationDate = next;
                    break;
                case SchedulePeriod.Hour:
                    next = new DateTime( item.CreationDate.Year, 
                                                item.CreationDate.Month,
                                                item.CreationDate.Day,
                                                item.CreationDate.Hour, 0, 0).
                                        AddHours(this.hours).
                                        AddMinutes(this.minutes);
                    item.ExpirationDate = next;
                    break;
                case SchedulePeriod.Second:
                    next = item.CreationDate.AddSeconds(this.seconds);
                    item.ExpirationDate = next;
                    break;
            }
            
        }

    }
}
