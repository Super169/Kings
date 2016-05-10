using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface
{
    public partial class auto
    {
        public const int DAY_START_HOUR = 5;
        public const int DAY_START_ADJ_MIN = 5;

        public enum FreqType
        {
            DAILY, ELAPSE, WEEKLY
        }

        public class TaskInfo
        {
            public string id { get; set; }
            public List<int> dow { get; set; } = new List<int>();
            public TimeSpan? startTime { get; set; } = null;
            public TimeSpan? endTime { get; set; } = null;
            public int elapseMin { get; set; } = -1;
            public List<TimeSpan> executionTimes { get; set; } = new List<TimeSpan>();
            public DateTime? lastExecutionTime = null;
            public DateTime? nextExecutionTime = null;

            public DateTime getDailyRefTime(DateTime baseTime, TimeSpan refTS)
            {
                DateTime refTime = new DateTime(baseTime.Year, baseTime.Month, baseTime.Day) + refTS;
                if (baseTime.Hour < 5)
                {
                    if (refTS.Hours >= 5) refTime = refTime.AddDays(-1);
                }
                else
                {
                    if (refTS.Hours < 5) refTime = refTime.AddDays(1);
                }
                return refTime;
            }

            public DateTime getDayStartTime(DateTime baseTime)
            {
                TimeSpan refTS = (startTime == null ? new TimeSpan(DAY_START_HOUR, DAY_START_ADJ_MIN, 0) : startTime.GetValueOrDefault());
                return getDailyRefTime(baseTime, refTS);
            }

            public DateTime getDayEndTime(DateTime baseTime)
            {
                TimeSpan refTS = (endTime == null ? new TimeSpan(DAY_START_HOUR, 0, 0).Subtract(new TimeSpan(0, DAY_START_ADJ_MIN, 0)) : endTime.GetValueOrDefault());
                return getDailyRefTime(baseTime, refTS);
            }

            public bool startTimeOK(DateTime chkTime)
            {
                return (startTimeOK(chkTime, chkTime));
            }

            public bool startTimeOK(DateTime baseTime, DateTime chkTime)
            {
                return (chkTime > getDayStartTime(baseTime));
            }

            public bool endTimeOK(DateTime chkTime)
            {
                return (endTimeOK(chkTime, chkTime));
            }

            public bool endTimeOK(DateTime baseTime, DateTime chkTime)
            {
                return (chkTime < getDayEndTime(baseTime));
            }

            public bool matchDOW(DateTime chkTime)
            {
                if (dow.Count == 0) return true;
                int currDow = (int)getDayStartTime(chkTime).DayOfWeek;
                return (dow.FindIndex(x => x == currDow) >= 0);
            }

            public bool matchTime(DateTime chkTime)
            {
                return (startTimeOK(chkTime) && endTimeOK(chkTime));
            }

            public bool validActionTime(DateTime chkTime)
            {
                return (matchDOW(chkTime) && matchTime(chkTime));
            }

            public bool readyToGo()
            {
                DateTime now = DateTime.Now;
                if (now < this.nextExecutionTime) return false;
                return (validActionTime(now));
            }

            public DateTime getNextByElapseTime(DateTime baseTime)
            {
                DateTime nextTime = baseTime.AddMinutes(this.elapseMin);
                if (!endTimeOK(baseTime, nextTime) || !matchDOW(nextTime))
                {
                    nextTime = getDayStartTime(baseTime).AddDays(1);
                    while (!matchDOW(nextTime))
                    {
                        nextTime = nextTime.AddDays(1);
                    }
                }
                return nextTime;
            }

            public DateTime getNextByFixStartTime(DateTime baseTime)
            {
                bool findOnBase = false;
                bool findMinTime = false;

                // Dummy initialize to due with the checking for usage before assignment
                DateTime nextTime = baseTime;
                DateTime minTime = baseTime;

                foreach (TimeSpan ts in executionTimes)
                {
                    DateTime newTime = getDailyRefTime(baseTime, ts);
                    if (validActionTime(newTime) && (newTime > baseTime))
                    {
                        if (findOnBase)
                        {
                            if (newTime < nextTime) nextTime = newTime;
                        }
                        else
                        {
                            findOnBase = true;
                            nextTime = newTime;
                        }
                    }
                    if (matchTime(newTime))
                    {
                        if (!findMinTime || (newTime < minTime))
                        {
                            findMinTime = true;
                            minTime = newTime;
                        }
                    }
                }
                if (!findOnBase)
                {
                    nextTime = minTime.AddDays(1);
                    while (!matchDOW(nextTime))
                    {
                        nextTime = nextTime.AddDays(1);
                    }
                }
                return nextTime;
            }

            public void setNextExecutionTime()
            {
                DateTime now = DateTime.Now;

                if (lastExecutionTime == null)
                {
                    // set lastExecuteTime on today if it cannot be execute on today,
                    // so that it will search for next time as normal
                    if (!matchDOW(now) || !endTimeOK(now)) lastExecutionTime = now;
                }
                if (lastExecutionTime == null)
                {
                    if (this.elapseMin > 0)
                    {
                        // Set to execute immedicately
                        this.nextExecutionTime = now;
                    }
                    else if (executionTimes.Count() > 0)
                    {
                        DateTime dayStart = getDayStartTime(now);
                        this.nextExecutionTime = getNextByFixStartTime(dayStart);
                    }
                    else
                    {
                        // Daily, so set to execute immedicately
                        this.nextExecutionTime = now;
                    }
                }
                else
                {
                    if (this.elapseMin > 0)
                    {
                        this.nextExecutionTime = getNextByElapseTime(this.lastExecutionTime.GetValueOrDefault());
                    }
                    else if (executionTimes.Count() > 0)
                    {
                        this.nextExecutionTime = getNextByFixStartTime(this.lastExecutionTime.GetValueOrDefault());
                    }
                    else
                    {
                        DateTime nextTime = getDayStartTime(this.lastExecutionTime.GetValueOrDefault()).AddDays(1);
                        while (!matchDOW(nextTime)) nextTime = nextTime.AddDays(1);
                        this.nextExecutionTime = nextTime;
                    }
                }
            }

        }

    }
}
