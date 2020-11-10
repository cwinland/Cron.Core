# Cron-Builder
Cron object that can be used to build Cron expressions, describe them, and manipulate objects.

# Examples

# Create Cron Object
schedule = new Cron();

# Add Cron sections by section
schedule.Add(time: CronTimeSections.Seconds, value: seconds, repeatEvery: true)

schedule.Add(CronTimeSections.Minutes, 4)

schedule.Add(CronTimeSections.Hours, 3, 5)

# Add time sections by months
schedule.Add(CronMonths.March)

# Add time section by Day
schedule.Add(CronDays.Wednesday)

# Chain sections.
schedule = new Cron()
    .Add(CronDays.Friday)
    .Add(CronTimeSections.DayMonth, dayMonth)
    .Add(CronTimeSections.Years, years, true);

# Create Cron with an existing expression
var cron = new Cron(expression);

# Remove Seconds entry
cron.Remove(CronTimeSections.Seconds, 5);                

# Remove Range of seconds entry
 cron.Remove(CronTimeSections.Seconds, 5, 6);

# Create Initial Cron with Days
  var cron = new Cron
            {
                { CronDays.Thursday, CronDays.Saturday }
            };

# Create Initial Cron with Months
              var cron = new Cron
            {
                { CronMonths.August, CronMonths.November }
            };

# Reset Day of the Week section only.
             cron.Reset(CronTimeSections.DayWeek);

# Reset all sections to the defaults.
             cron.ResetAll();
