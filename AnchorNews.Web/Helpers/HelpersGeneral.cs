namespace AnchorNews.Web.Helpers
{
    public static class HelpersGeneral
    {

        public static string HumanizeTime(DateTime dateTime)
        {
            DateTime currentDateTime = DateTime.UtcNow;
            TimeSpan timeSpan = currentDateTime - dateTime;

            if (timeSpan.TotalMilliseconds < 1000)
            {
                return "Less than a second";
            }
            else if (timeSpan.TotalMinutes < 1)
            {
                return $"{(int)timeSpan.TotalSeconds} seconds ago";
            }
            else if (timeSpan.TotalHours < 1)
            {
                return $"{(int)timeSpan.TotalMinutes} minutes ago";
            }
            else if (timeSpan.TotalDays < 1)
            {
                return $"{(int)timeSpan.TotalHours} hours ago";
            }
            else if (timeSpan.TotalDays < 7)
            {
                return $"{(int)timeSpan.TotalDays} days ago";
            }
            else if (timeSpan.TotalDays < 30)
            {
                int weeks = (int)(timeSpan.TotalDays / 7);
                return $"{weeks} {(weeks == 1 ? "week ago" : "weeks ago")}";
            }
            else if (timeSpan.TotalDays < 365)
            {
                int months = (int)(timeSpan.TotalDays / 30);
                return $"{months} {(months == 1 ? "month ago" : "months ago")}";
            }
            else
            {
                int years = (int)(timeSpan.TotalDays / 365);
                return $"{years} {(years == 1 ? "year" : "years")}";
            }
        }
    }
}
