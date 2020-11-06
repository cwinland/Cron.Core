<a name='assembly'></a>
# Cron

## Contents

- [Cron](#T-Cron-Cron 'Cron.Cron')
  - [#ctor()](#M-Cron-Cron-#ctor 'Cron.Cron.#ctor')
  - [#ctor()](#M-Cron-Cron-#ctor-System-String- 'Cron.Cron.#ctor(System.String)')
  - [DayMonth](#P-Cron-Cron-DayMonth 'Cron.Cron.DayMonth')
  - [DayWeek](#P-Cron-Cron-DayWeek 'Cron.Cron.DayWeek')
  - [Description](#P-Cron-Cron-Description 'Cron.Cron.Description')
  - [Hours](#P-Cron-Cron-Hours 'Cron.Cron.Hours')
  - [Minutes](#P-Cron-Cron-Minutes 'Cron.Cron.Minutes')
  - [Months](#P-Cron-Cron-Months 'Cron.Cron.Months')
  - [Seconds](#P-Cron-Cron-Seconds 'Cron.Cron.Seconds')
  - [Value](#P-Cron-Cron-Value 'Cron.Cron.Value')
  - [Years](#P-Cron-Cron-Years 'Cron.Cron.Years')
  - [Add()](#M-Cron-Cron-Add-Cron-Enums-CronTimeSections,System-Int32,System-Boolean- 'Cron.Cron.Add(Cron.Enums.CronTimeSections,System.Int32,System.Boolean)')
  - [Add()](#M-Cron-Cron-Add-Cron-Enums-CronTimeSections,System-Int32,System-Int32- 'Cron.Cron.Add(Cron.Enums.CronTimeSections,System.Int32,System.Int32)')
  - [Add()](#M-Cron-Cron-Add-Cron-Enums-CronDays,System-Boolean- 'Cron.Cron.Add(Cron.Enums.CronDays,System.Boolean)')
  - [Add()](#M-Cron-Cron-Add-Cron-Enums-CronMonths,System-Boolean- 'Cron.Cron.Add(Cron.Enums.CronMonths,System.Boolean)')
  - [Add()](#M-Cron-Cron-Add-Cron-Enums-CronDays,Cron-Enums-CronDays- 'Cron.Cron.Add(Cron.Enums.CronDays,Cron.Enums.CronDays)')
  - [Add()](#M-Cron-Cron-Add-Cron-Enums-CronMonths,Cron-Enums-CronMonths- 'Cron.Cron.Add(Cron.Enums.CronMonths,Cron.Enums.CronMonths)')
  - [Remove()](#M-Cron-Cron-Remove-Cron-Enums-CronTimeSections,System-Int32- 'Cron.Cron.Remove(Cron.Enums.CronTimeSections,System.Int32)')
  - [Remove()](#M-Cron-Cron-Remove-Cron-Enums-CronTimeSections,System-Int32,System-Int32- 'Cron.Cron.Remove(Cron.Enums.CronTimeSections,System.Int32,System.Int32)')
  - [Reset()](#M-Cron-Cron-Reset-Cron-Enums-CronTimeSections- 'Cron.Cron.Reset(Cron.Enums.CronTimeSections)')
  - [ResetAll()](#M-Cron-Cron-ResetAll 'Cron.Cron.ResetAll')
  - [Set()](#M-Cron-Cron-Set-Cron-Enums-CronTimeSections,Cron-Interfaces-ISection- 'Cron.Cron.Set(Cron.Enums.CronTimeSections,Cron.Interfaces.ISection)')
  - [System#Collections#IEnumerable#GetEnumerator()](#M-Cron-Cron-System#Collections#IEnumerable#GetEnumerator 'Cron.Cron.System#Collections#IEnumerable#GetEnumerator')
  - [ToString()](#M-Cron-Cron-ToString 'Cron.Cron.ToString')
- [CronDays](#T-Cron-Enums-CronDays 'Cron.Enums.CronDays')
  - [Friday](#F-Cron-Enums-CronDays-Friday 'Cron.Enums.CronDays.Friday')
  - [Monday](#F-Cron-Enums-CronDays-Monday 'Cron.Enums.CronDays.Monday')
  - [Saturday](#F-Cron-Enums-CronDays-Saturday 'Cron.Enums.CronDays.Saturday')
  - [Sunday](#F-Cron-Enums-CronDays-Sunday 'Cron.Enums.CronDays.Sunday')
  - [Thursday](#F-Cron-Enums-CronDays-Thursday 'Cron.Enums.CronDays.Thursday')
  - [Tuesday](#F-Cron-Enums-CronDays-Tuesday 'Cron.Enums.CronDays.Tuesday')
  - [Wednesday](#F-Cron-Enums-CronDays-Wednesday 'Cron.Enums.CronDays.Wednesday')
- [CronMonths](#T-Cron-Enums-CronMonths 'Cron.Enums.CronMonths')
  - [April](#F-Cron-Enums-CronMonths-April 'Cron.Enums.CronMonths.April')
  - [August](#F-Cron-Enums-CronMonths-August 'Cron.Enums.CronMonths.August')
  - [December](#F-Cron-Enums-CronMonths-December 'Cron.Enums.CronMonths.December')
  - [February](#F-Cron-Enums-CronMonths-February 'Cron.Enums.CronMonths.February')
  - [January](#F-Cron-Enums-CronMonths-January 'Cron.Enums.CronMonths.January')
  - [July](#F-Cron-Enums-CronMonths-July 'Cron.Enums.CronMonths.July')
  - [June](#F-Cron-Enums-CronMonths-June 'Cron.Enums.CronMonths.June')
  - [March](#F-Cron-Enums-CronMonths-March 'Cron.Enums.CronMonths.March')
  - [May](#F-Cron-Enums-CronMonths-May 'Cron.Enums.CronMonths.May')
  - [November](#F-Cron-Enums-CronMonths-November 'Cron.Enums.CronMonths.November')
  - [October](#F-Cron-Enums-CronMonths-October 'Cron.Enums.CronMonths.October')
  - [September](#F-Cron-Enums-CronMonths-September 'Cron.Enums.CronMonths.September')
- [CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections')
- [ICron](#T-Cron-Interfaces-ICron 'Cron.Interfaces.ICron')
  - [DayMonth](#P-Cron-Interfaces-ICron-DayMonth 'Cron.Interfaces.ICron.DayMonth')
  - [DayWeek](#P-Cron-Interfaces-ICron-DayWeek 'Cron.Interfaces.ICron.DayWeek')
  - [Description](#P-Cron-Interfaces-ICron-Description 'Cron.Interfaces.ICron.Description')
  - [Hours](#P-Cron-Interfaces-ICron-Hours 'Cron.Interfaces.ICron.Hours')
  - [Minutes](#P-Cron-Interfaces-ICron-Minutes 'Cron.Interfaces.ICron.Minutes')
  - [Months](#P-Cron-Interfaces-ICron-Months 'Cron.Interfaces.ICron.Months')
  - [Seconds](#P-Cron-Interfaces-ICron-Seconds 'Cron.Interfaces.ICron.Seconds')
  - [Value](#P-Cron-Interfaces-ICron-Value 'Cron.Interfaces.ICron.Value')
  - [Years](#P-Cron-Interfaces-ICron-Years 'Cron.Interfaces.ICron.Years')
  - [Add(time,value,repeatEvery)](#M-Cron-Interfaces-ICron-Add-Cron-Enums-CronTimeSections,System-Int32,System-Boolean- 'Cron.Interfaces.ICron.Add(Cron.Enums.CronTimeSections,System.Int32,System.Boolean)')
  - [Add(time,minValue,maxValue)](#M-Cron-Interfaces-ICron-Add-Cron-Enums-CronTimeSections,System-Int32,System-Int32- 'Cron.Interfaces.ICron.Add(Cron.Enums.CronTimeSections,System.Int32,System.Int32)')
  - [Add(value,repeatEvery)](#M-Cron-Interfaces-ICron-Add-Cron-Enums-CronDays,System-Boolean- 'Cron.Interfaces.ICron.Add(Cron.Enums.CronDays,System.Boolean)')
  - [Add(value,repeatEvery)](#M-Cron-Interfaces-ICron-Add-Cron-Enums-CronMonths,System-Boolean- 'Cron.Interfaces.ICron.Add(Cron.Enums.CronMonths,System.Boolean)')
  - [Add(minValue,maxValue)](#M-Cron-Interfaces-ICron-Add-Cron-Enums-CronDays,Cron-Enums-CronDays- 'Cron.Interfaces.ICron.Add(Cron.Enums.CronDays,Cron.Enums.CronDays)')
  - [Add(minValue,maxValue)](#M-Cron-Interfaces-ICron-Add-Cron-Enums-CronMonths,Cron-Enums-CronMonths- 'Cron.Interfaces.ICron.Add(Cron.Enums.CronMonths,Cron.Enums.CronMonths)')
  - [Remove(time,value)](#M-Cron-Interfaces-ICron-Remove-Cron-Enums-CronTimeSections,System-Int32- 'Cron.Interfaces.ICron.Remove(Cron.Enums.CronTimeSections,System.Int32)')
  - [Remove(time,minValue,maxValue)](#M-Cron-Interfaces-ICron-Remove-Cron-Enums-CronTimeSections,System-Int32,System-Int32- 'Cron.Interfaces.ICron.Remove(Cron.Enums.CronTimeSections,System.Int32,System.Int32)')
  - [Reset(time)](#M-Cron-Interfaces-ICron-Reset-Cron-Enums-CronTimeSections- 'Cron.Interfaces.ICron.Reset(Cron.Enums.CronTimeSections)')
  - [ResetAll()](#M-Cron-Interfaces-ICron-ResetAll 'Cron.Interfaces.ICron.ResetAll')
  - [Set(time,value)](#M-Cron-Interfaces-ICron-Set-Cron-Enums-CronTimeSections,Cron-Interfaces-ISection- 'Cron.Interfaces.ICron.Set(Cron.Enums.CronTimeSections,Cron.Interfaces.ISection)')
- [ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection')
  - [Any](#P-Cron-Interfaces-ISection-Any 'Cron.Interfaces.ISection.Any')
  - [Enabled](#P-Cron-Interfaces-ISection-Enabled 'Cron.Interfaces.ISection.Enabled')
  - [Every](#P-Cron-Interfaces-ISection-Every 'Cron.Interfaces.ISection.Every')
  - [Values](#P-Cron-Interfaces-ISection-Values 'Cron.Interfaces.ISection.Values')
  - [Add(value)](#M-Cron-Interfaces-ISection-Add-System-Int32- 'Cron.Interfaces.ISection.Add(System.Int32)')
  - [Add(minVal,maxVal)](#M-Cron-Interfaces-ISection-Add-System-Int32,System-Int32- 'Cron.Interfaces.ISection.Add(System.Int32,System.Int32)')
  - [Clear()](#M-Cron-Interfaces-ISection-Clear 'Cron.Interfaces.ISection.Clear')
  - [IsValidRange(values)](#M-Cron-Interfaces-ISection-IsValidRange-Cron-Interfaces-ISectionValues- 'Cron.Interfaces.ISection.IsValidRange(Cron.Interfaces.ISectionValues)')
  - [IsValidRange(value)](#M-Cron-Interfaces-ISection-IsValidRange-System-Int32- 'Cron.Interfaces.ISection.IsValidRange(System.Int32)')
  - [Remove(value)](#M-Cron-Interfaces-ISection-Remove-System-Int32- 'Cron.Interfaces.ISection.Remove(System.Int32)')
  - [Remove(minVal,maxVal)](#M-Cron-Interfaces-ISection-Remove-System-Int32,System-Int32- 'Cron.Interfaces.ISection.Remove(System.Int32,System.Int32)')
- [Section](#T-Cron-Section 'Cron.Section')
  - [#ctor(time,expression)](#M-Cron-Section-#ctor-Cron-Enums-CronTimeSections,System-String- 'Cron.Section.#ctor(Cron.Enums.CronTimeSections,System.String)')
  - [#ctor(time)](#M-Cron-Section-#ctor-Cron-Enums-CronTimeSections- 'Cron.Section.#ctor(Cron.Enums.CronTimeSections)')
  - [Any](#P-Cron-Section-Any 'Cron.Section.Any')
  - [Count](#P-Cron-Section-Count 'Cron.Section.Count')
  - [Enabled](#P-Cron-Section-Enabled 'Cron.Section.Enabled')
  - [Every](#P-Cron-Section-Every 'Cron.Section.Every')
  - [Item](#P-Cron-Section-Item-System-Int32- 'Cron.Section.Item(System.Int32)')
  - [Values](#P-Cron-Section-Values 'Cron.Section.Values')
  - [Add()](#M-Cron-Section-Add-System-Int32- 'Cron.Section.Add(System.Int32)')
  - [Add()](#M-Cron-Section-Add-System-Int32,System-Int32- 'Cron.Section.Add(System.Int32,System.Int32)')
  - [Clear()](#M-Cron-Section-Clear 'Cron.Section.Clear')
  - [GetEnumerator()](#M-Cron-Section-GetEnumerator 'Cron.Section.GetEnumerator')
  - [IsValidRange()](#M-Cron-Section-IsValidRange-Cron-Interfaces-ISectionValues- 'Cron.Section.IsValidRange(Cron.Interfaces.ISectionValues)')
  - [IsValidRange()](#M-Cron-Section-IsValidRange-System-Int32- 'Cron.Section.IsValidRange(System.Int32)')
  - [Remove()](#M-Cron-Section-Remove-System-Int32- 'Cron.Section.Remove(System.Int32)')
  - [Remove()](#M-Cron-Section-Remove-System-Int32,System-Int32- 'Cron.Section.Remove(System.Int32,System.Int32)')
  - [System#Collections#IEnumerable#GetEnumerator()](#M-Cron-Section-System#Collections#IEnumerable#GetEnumerator 'Cron.Section.System#Collections#IEnumerable#GetEnumerator')
  - [ToString()](#M-Cron-Section-ToString 'Cron.Section.ToString')

<a name='T-Cron-Cron'></a>
## Cron `type`

##### Namespace

Cron

##### Summary

*Inherit from parent.*

<a name='M-Cron-Cron-#ctor'></a>
### #ctor() `constructor`

##### Summary

*Inherit from parent.*

##### Parameters

This constructor has no parameters.

<a name='M-Cron-Cron-#ctor-System-String-'></a>
### #ctor() `constructor`

##### Summary

*Inherit from parent.*

##### Parameters

This constructor has no parameters.

<a name='P-Cron-Cron-DayMonth'></a>
### DayMonth `property`

##### Summary

*Inherit from parent.*

<a name='P-Cron-Cron-DayWeek'></a>
### DayWeek `property`

##### Summary

*Inherit from parent.*

<a name='P-Cron-Cron-Description'></a>
### Description `property`

##### Summary

*Inherit from parent.*

<a name='P-Cron-Cron-Hours'></a>
### Hours `property`

##### Summary

*Inherit from parent.*

<a name='P-Cron-Cron-Minutes'></a>
### Minutes `property`

##### Summary

*Inherit from parent.*

<a name='P-Cron-Cron-Months'></a>
### Months `property`

##### Summary

*Inherit from parent.*

<a name='P-Cron-Cron-Seconds'></a>
### Seconds `property`

##### Summary

*Inherit from parent.*

<a name='P-Cron-Cron-Value'></a>
### Value `property`

##### Summary

*Inherit from parent.*

<a name='P-Cron-Cron-Years'></a>
### Years `property`

##### Summary

*Inherit from parent.*

<a name='M-Cron-Cron-Add-Cron-Enums-CronTimeSections,System-Int32,System-Boolean-'></a>
### Add() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Cron-Cron-Add-Cron-Enums-CronTimeSections,System-Int32,System-Int32-'></a>
### Add() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Cron-Cron-Add-Cron-Enums-CronDays,System-Boolean-'></a>
### Add() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Cron-Cron-Add-Cron-Enums-CronMonths,System-Boolean-'></a>
### Add() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Cron-Cron-Add-Cron-Enums-CronDays,Cron-Enums-CronDays-'></a>
### Add() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Cron-Cron-Add-Cron-Enums-CronMonths,Cron-Enums-CronMonths-'></a>
### Add() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Cron-Cron-Remove-Cron-Enums-CronTimeSections,System-Int32-'></a>
### Remove() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Cron-Cron-Remove-Cron-Enums-CronTimeSections,System-Int32,System-Int32-'></a>
### Remove() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Cron-Cron-Reset-Cron-Enums-CronTimeSections-'></a>
### Reset() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Cron-Cron-ResetAll'></a>
### ResetAll() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Cron-Cron-Set-Cron-Enums-CronTimeSections,Cron-Interfaces-ISection-'></a>
### Set() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Cron-Cron-System#Collections#IEnumerable#GetEnumerator'></a>
### System#Collections#IEnumerable#GetEnumerator() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Cron-Cron-ToString'></a>
### ToString() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='T-Cron-Enums-CronDays'></a>
## CronDays `type`

##### Namespace

Cron.Enums

##### Summary

Day of the week values to build Cron expressions.

<a name='F-Cron-Enums-CronDays-Friday'></a>
### Friday `constants`

##### Summary

Friday.

<a name='F-Cron-Enums-CronDays-Monday'></a>
### Monday `constants`

##### Summary

Monday.

<a name='F-Cron-Enums-CronDays-Saturday'></a>
### Saturday `constants`

##### Summary

Saturday.

<a name='F-Cron-Enums-CronDays-Sunday'></a>
### Sunday `constants`

##### Summary

Sunday.

<a name='F-Cron-Enums-CronDays-Thursday'></a>
### Thursday `constants`

##### Summary

Thursday.

<a name='F-Cron-Enums-CronDays-Tuesday'></a>
### Tuesday `constants`

##### Summary

Tuesday.

<a name='F-Cron-Enums-CronDays-Wednesday'></a>
### Wednesday `constants`

##### Summary

Wednesday.

<a name='T-Cron-Enums-CronMonths'></a>
## CronMonths `type`

##### Namespace

Cron.Enums

##### Summary

Month values used to build Cron expressions.

<a name='F-Cron-Enums-CronMonths-April'></a>
### April `constants`

##### Summary

April.

<a name='F-Cron-Enums-CronMonths-August'></a>
### August `constants`

##### Summary

August.

<a name='F-Cron-Enums-CronMonths-December'></a>
### December `constants`

##### Summary

December.

<a name='F-Cron-Enums-CronMonths-February'></a>
### February `constants`

##### Summary

February.

<a name='F-Cron-Enums-CronMonths-January'></a>
### January `constants`

##### Summary

January.

<a name='F-Cron-Enums-CronMonths-July'></a>
### July `constants`

##### Summary

July.

<a name='F-Cron-Enums-CronMonths-June'></a>
### June `constants`

##### Summary

June.

<a name='F-Cron-Enums-CronMonths-March'></a>
### March `constants`

##### Summary

March.

<a name='F-Cron-Enums-CronMonths-May'></a>
### May `constants`

##### Summary

May.

<a name='F-Cron-Enums-CronMonths-November'></a>
### November `constants`

##### Summary

November.

<a name='F-Cron-Enums-CronMonths-October'></a>
### October `constants`

##### Summary

October.

<a name='F-Cron-Enums-CronMonths-September'></a>
### September `constants`

##### Summary

September.

<a name='T-Cron-Enums-CronTimeSections'></a>
## CronTimeSections `type`

##### Namespace

Cron.Enums

##### Summary

Sections of the Cron indicating the type of time.

<a name='T-Cron-Interfaces-ICron'></a>
## ICron `type`

##### Namespace

Cron.Interfaces

##### Summary

Cron Interface Object.

<a name='P-Cron-Interfaces-ICron-DayMonth'></a>
### DayMonth `property`

##### Summary

Day of the Month Information

##### Returns

[ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection')

<a name='P-Cron-Interfaces-ICron-DayWeek'></a>
### DayWeek `property`

##### Summary

Day of the Week Information

##### Returns

[ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection')

<a name='P-Cron-Interfaces-ICron-Description'></a>
### Description `property`

##### Summary

Human readable description.

##### Example

Every 22 seconds, every 3,4 minutes, at 03:00 AM through 05:59 AM and 07:00 AM through 11:59 AM, on day 12 of the month, only on
  Wednesday, only in March and May, every 5 years

<a name='P-Cron-Interfaces-ICron-Hours'></a>
### Hours `property`

##### Summary

Hours Information

##### Returns

[ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection')

<a name='P-Cron-Interfaces-ICron-Minutes'></a>
### Minutes `property`

##### Summary

Minutes Information

##### Returns

[ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection')

<a name='P-Cron-Interfaces-ICron-Months'></a>
### Months `property`

##### Summary

Months Information

##### Returns

[ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection')

<a name='P-Cron-Interfaces-ICron-Seconds'></a>
### Seconds `property`

##### Summary

Seconds Information

##### Returns

[ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection')

<a name='P-Cron-Interfaces-ICron-Value'></a>
### Value `property`

##### Summary

Cron Expression.

##### Example

*/22 */3,4 3-5,7-11 12 3,5 3 */5

<a name='P-Cron-Interfaces-ICron-Years'></a>
### Years `property`

##### Summary

Year Information

##### Returns

[ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection')

<a name='M-Cron-Interfaces-ICron-Add-Cron-Enums-CronTimeSections,System-Int32,System-Boolean-'></a>
### Add(time,value,repeatEvery) `method`

##### Summary

Add time value for the specified time section.

##### Returns

[ICron](#T-Cron-Interfaces-ICron 'Cron.Interfaces.ICron')

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| time | [Cron.Enums.CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections') | The type of time section such as seconds, minutes, etc. See [CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections'). |
| value | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Value for the specified time section. |
| repeatEvery | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | Indicates if the value is a repeating time or specific time. |

<a name='M-Cron-Interfaces-ICron-Add-Cron-Enums-CronTimeSections,System-Int32,System-Int32-'></a>
### Add(time,minValue,maxValue) `method`

##### Summary

Add time value for the specified time section.

##### Returns

[ICron](#T-Cron-Interfaces-ICron 'Cron.Interfaces.ICron')

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| time | [Cron.Enums.CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections') | The type of time section such as seconds, minutes, etc. See [CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections'). |
| minValue | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Starting value for the specified time section. |
| maxValue | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Ending value for the specified time section. |

<a name='M-Cron-Interfaces-ICron-Add-Cron-Enums-CronDays,System-Boolean-'></a>
### Add(value,repeatEvery) `method`

##### Summary

Add day of the week.

##### Returns

[ICron](#T-Cron-Interfaces-ICron 'Cron.Interfaces.ICron')

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [Cron.Enums.CronDays](#T-Cron-Enums-CronDays 'Cron.Enums.CronDays') | [CronDays](#T-Cron-Enums-CronDays 'Cron.Enums.CronDays') representing the day of the week. |
| repeatEvery | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | Indicates if the value is a repeating day or specific day. |

<a name='M-Cron-Interfaces-ICron-Add-Cron-Enums-CronMonths,System-Boolean-'></a>
### Add(value,repeatEvery) `method`

##### Summary

Add month.

##### Returns

[ICron](#T-Cron-Interfaces-ICron 'Cron.Interfaces.ICron')

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [Cron.Enums.CronMonths](#T-Cron-Enums-CronMonths 'Cron.Enums.CronMonths') | [CronMonths](#T-Cron-Enums-CronMonths 'Cron.Enums.CronMonths') representing the month. |
| repeatEvery | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | Indicates if the value is a repeating month or specific month. |

<a name='M-Cron-Interfaces-ICron-Add-Cron-Enums-CronDays,Cron-Enums-CronDays-'></a>
### Add(minValue,maxValue) `method`

##### Summary

Add range of days in the week.

##### Returns

[ICron](#T-Cron-Interfaces-ICron 'Cron.Interfaces.ICron')

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| minValue | [Cron.Enums.CronDays](#T-Cron-Enums-CronDays 'Cron.Enums.CronDays') | Starting [CronDays](#T-Cron-Enums-CronDays 'Cron.Enums.CronDays') representing the day of the week. |
| maxValue | [Cron.Enums.CronDays](#T-Cron-Enums-CronDays 'Cron.Enums.CronDays') | Ending [CronDays](#T-Cron-Enums-CronDays 'Cron.Enums.CronDays') representing the day of the week. |

<a name='M-Cron-Interfaces-ICron-Add-Cron-Enums-CronMonths,Cron-Enums-CronMonths-'></a>
### Add(minValue,maxValue) `method`

##### Summary

Add range of months.

##### Returns

[ICron](#T-Cron-Interfaces-ICron 'Cron.Interfaces.ICron')

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| minValue | [Cron.Enums.CronMonths](#T-Cron-Enums-CronMonths 'Cron.Enums.CronMonths') | Starting [CronMonths](#T-Cron-Enums-CronMonths 'Cron.Enums.CronMonths') representing the day of the month. |
| maxValue | [Cron.Enums.CronMonths](#T-Cron-Enums-CronMonths 'Cron.Enums.CronMonths') | Ending [CronMonths](#T-Cron-Enums-CronMonths 'Cron.Enums.CronMonths') representing the day of the month. |

<a name='M-Cron-Interfaces-ICron-Remove-Cron-Enums-CronTimeSections,System-Int32-'></a>
### Remove(time,value) `method`

##### Summary

Remove [CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections') with a specified value.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| time | [Cron.Enums.CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections') | The type of time section such as seconds, minutes, etc. See [CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections'). |
| value | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Value for the specified time section. |

<a name='M-Cron-Interfaces-ICron-Remove-Cron-Enums-CronTimeSections,System-Int32,System-Int32-'></a>
### Remove(time,minValue,maxValue) `method`

##### Summary

Remove [CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections') with a specified value.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| time | [Cron.Enums.CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections') | The type of time section such as seconds, minutes, etc. See [CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections'). |
| minValue | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Starting value for the specified time section. |
| maxValue | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Ending value for the specified time section. |

<a name='M-Cron-Interfaces-ICron-Reset-Cron-Enums-CronTimeSections-'></a>
### Reset(time) `method`

##### Summary

Clear the specific time element of the Cron object to defaults without any numerical cron representations.

##### Returns

[ICron](#T-Cron-Interfaces-ICron 'Cron.Interfaces.ICron')

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| time | [Cron.Enums.CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections') | The type of time section such as seconds, minutes, etc. See [CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections'). |

<a name='M-Cron-Interfaces-ICron-ResetAll'></a>
### ResetAll() `method`

##### Summary

Clear the entire Cron object to defaults without any numerical cron representations.

##### Returns

[ICron](#T-Cron-Interfaces-ICron 'Cron.Interfaces.ICron')

##### Parameters

This method has no parameters.

<a name='M-Cron-Interfaces-ICron-Set-Cron-Enums-CronTimeSections,Cron-Interfaces-ISection-'></a>
### Set(time,value) `method`

##### Summary

Set time with [ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection') value.

##### Returns

[ICron](#T-Cron-Interfaces-ICron 'Cron.Interfaces.ICron')

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| time | [Cron.Enums.CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections') | The type of time section such as seconds, minutes, etc. See [CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections'). |
| value | [Cron.Interfaces.ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection') | Value for the specified time section. |

<a name='T-Cron-Interfaces-ISection'></a>
## ISection `type`

##### Namespace

Cron.Interfaces

<a name='P-Cron-Interfaces-ISection-Any'></a>
### Any `property`

##### Summary

Indicates that the value should be translated using the ? any indicator.

##### Remarks

Experimental.

<a name='P-Cron-Interfaces-ISection-Enabled'></a>
### Enabled `property`

##### Summary

Indicates that the value is enabled or used. Equivalent to using ? in a cron expression.

<a name='P-Cron-Interfaces-ISection-Every'></a>
### Every `property`

##### Summary

Indicates that the value should be translated using the */ every indicator.

<a name='P-Cron-Interfaces-ISection-Values'></a>
### Values `property`

##### Summary

List of Cron value expression specific to the [ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection').

<a name='M-Cron-Interfaces-ISection-Add-System-Int32-'></a>
### Add(value) `method`

##### Summary

Add time value to this [ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection').

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Value for this [ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection'). |

<a name='M-Cron-Interfaces-ISection-Add-System-Int32,System-Int32-'></a>
### Add(minVal,maxVal) `method`

##### Summary

Add a time value range to this [ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection').

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| minVal | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Starting value for this [ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection'). |
| maxVal | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Ending value for this [ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection'). |

<a name='M-Cron-Interfaces-ISection-Clear'></a>
### Clear() `method`

##### Summary

Clear the values in the [ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection').

##### Returns



##### Parameters

This method has no parameters.

<a name='M-Cron-Interfaces-ISection-IsValidRange-Cron-Interfaces-ISectionValues-'></a>
### IsValidRange(values) `method`

##### Summary

Checks if the given values are valid for the current [ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection')'s [CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections') value.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| values | [Cron.Interfaces.ISectionValues](#T-Cron-Interfaces-ISectionValues 'Cron.Interfaces.ISectionValues') | [ISectionValues](#T-Cron-Interfaces-ISectionValues 'Cron.Interfaces.ISectionValues') values. |

<a name='M-Cron-Interfaces-ISection-IsValidRange-System-Int32-'></a>
### IsValidRange(value) `method`

##### Summary

Checks if the given value is valid for the current [ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection')'s [CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections') value.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Value for this [ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection'). |

<a name='M-Cron-Interfaces-ISection-Remove-System-Int32-'></a>
### Remove(value) `method`

##### Summary

Remove the specified value from the [ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection').

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Value for this [ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection'). |

<a name='M-Cron-Interfaces-ISection-Remove-System-Int32,System-Int32-'></a>
### Remove(minVal,maxVal) `method`

##### Summary

Remove the specified range of values from the [ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection').

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| minVal | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Starting value for this [ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection'). |
| maxVal | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Ending value for this [ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection'). |

<a name='T-Cron-Section'></a>
## Section `type`

##### Namespace

Cron

##### Summary

*Inherit from parent.*

<a name='M-Cron-Section-#ctor-Cron-Enums-CronTimeSections,System-String-'></a>
### #ctor(time,expression) `constructor`

##### Summary

Create an [ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection') based on the specified expression.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| time | [Cron.Enums.CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections') | Specific [CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections'). |
| expression | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Cron Expression |

##### Remarks

Experimental.

<a name='M-Cron-Section-#ctor-Cron-Enums-CronTimeSections-'></a>
### #ctor(time) `constructor`

##### Summary

Create [ISection](#T-Cron-Interfaces-ISection 'Cron.Interfaces.ISection') for a specific [CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections').

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| time | [Cron.Enums.CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections') | The type of time section such as seconds, minutes, etc. See [CronTimeSections](#T-Cron-Enums-CronTimeSections 'Cron.Enums.CronTimeSections'). |

<a name='P-Cron-Section-Any'></a>
### Any `property`

##### Summary

*Inherit from parent.*

<a name='P-Cron-Section-Count'></a>
### Count `property`

##### Summary

*Inherit from parent.*

<a name='P-Cron-Section-Enabled'></a>
### Enabled `property`

##### Summary

*Inherit from parent.*

<a name='P-Cron-Section-Every'></a>
### Every `property`

##### Summary

*Inherit from parent.*

<a name='P-Cron-Section-Item-System-Int32-'></a>
### Item `property`

##### Summary

*Inherit from parent.*

<a name='P-Cron-Section-Values'></a>
### Values `property`

##### Summary

*Inherit from parent.*

<a name='M-Cron-Section-Add-System-Int32-'></a>
### Add() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Cron-Section-Add-System-Int32,System-Int32-'></a>
### Add() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Cron-Section-Clear'></a>
### Clear() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Cron-Section-GetEnumerator'></a>
### GetEnumerator() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Cron-Section-IsValidRange-Cron-Interfaces-ISectionValues-'></a>
### IsValidRange() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Cron-Section-IsValidRange-System-Int32-'></a>
### IsValidRange() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Cron-Section-Remove-System-Int32-'></a>
### Remove() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Cron-Section-Remove-System-Int32,System-Int32-'></a>
### Remove() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Cron-Section-System#Collections#IEnumerable#GetEnumerator'></a>
### System#Collections#IEnumerable#GetEnumerator() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Cron-Section-ToString'></a>
### ToString() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.
