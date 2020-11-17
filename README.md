# Cron-Builder
Cron Builder object that can be used to build Cron expressions, describe them, and manipulate objects.

# Feature Overview
1. Build Cron Expression.
2. Create Cron by Expression.
3. Display Description of Cron expression or a section.
4. Expression Chaining of Cron object and Sections (Seconds, Minutes, Hours, DayMonth, Months, DayWeek, Year).
5. Set intervals or specific times on Time Sections (Seconds, Minutes, Hours).
6. Set specific date sections (DayMonth, Months, DayWeek, Year).
   
# Examples

## Create a new CronBuilder Object.
### Create Default Cron Object.
```
    schedule = new CronBuilder();
```

### Create Cron object with an existing expression.
```
    var cron = new CronBuilder(expression);
```

### Create Initial Cron object with Days.
```
    var cron = new CronBuilder
            {
                { CronDays.Thursday, CronDays.Saturday }
            };
```

### Create Initial Cron object with Months.
```
    var cron = new CronBuilder
    {
        { CronMonths.August, CronMonths.November }
    };
```

### Create Initial Cron object with Months and Day.
```
    var cron = new CronBuilder
    {
        { CronDays.Thursday, CronMonths.November }
    };
```

## Build Cron by Section.
### Add to Cron with sections.
```
    schedule.Add(time: CronTimeSections.Seconds, value: seconds, repeatEvery: true)
    schedule.Add(CronTimeSections.Minutes, 4)
    schedule.Add(CronTimeSections.Hours, 3, 5)
```

### Add a Month Restriction to the Cron Expression.
```
    schedule.Add(CronMonths.March)
```

### Add a Day of the Week Restriction to the Cron Expression.
```
    schedule.Add(CronDays.Wednesday)
```

## Chain Cron and Sections.
```
    schedule = new CronBuilder();
    schedule
        .Add(CronDays.Friday)
        .Add(CronTimeSections.DayMonth, dayMonth)
        .Add(CronTimeSections.Years, years, true)
        .Seconds.Add(5);
```

## Display Cron Description
```
    var descCron = cron.Description;
    var descSeconds = cron.Seconds.Description;
```

## Remove all or parts of a Cron Expression.
### Remove only one entry of 5 in Seconds.
```
    cron.Remove(CronTimeSections.Seconds, 5);                
```

### Remove only one entry Range of Seconds.
```
    cron.Remove(CronTimeSections.Seconds, 5, 6);
```

### Reset / Remove All of the Day of the Week section.
```
    cron.Reset(CronTimeSections.DayWeek);
```

### Reset all sections to the defaults.
```
    cron.ResetAll();
```

# Updates
## Version 1.1.11.17
Cron Object renamed to CronBuilder to simplify creation due to conflicting namespace.