using Cron.Core.Enums;

namespace Cron.Core.Extensions
{
    public static class CronBuilderExtensions
    {
        public static ICronBuilder Overwrite(this ICronBuilder obj, bool overwrite = true)
        {
            if (obj is NewCronBuilder c)
            {
                if (c.IsDirty && c.LastCronType.HasValue)
                {
                    c.GetValues(c.LastCronType.Value).Overwrite = overwrite;
                }
                else
                {
                    CronType.Day.ToNameList().ForEach(x => c.GetValues(x).Overwrite = overwrite);
                }
            }

            return obj;
        }
    }
}