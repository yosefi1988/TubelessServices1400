using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace TubelessServices.Classes.Utility
{
    public class Date
    {

        public static String convertToPersianDate(DateTime enDate)
        {
            DateTime d = DateTime.Parse(enDate.ToString());
            PersianCalendar pc = new PersianCalendar();
            return string.Format("{3} - {0}/{1}/{2}", pc.GetYear(d), pc.GetMonth(d), pc.GetDayOfMonth(d), convertDayOfWeek(pc.GetDayOfWeek(d)));
        }

        public static String convertToPersianDate(DateTime? enDate)
        {
            return convertToPersianDate((DateTime)enDate);
        }

        private static string convertDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "یک شنبه";
                case DayOfWeek.Monday:
                    return "دو شنبه";
                case DayOfWeek.Tuesday:
                    return "سه شنبه";
                case DayOfWeek.Wednesday:
                    return "چهارشنبه";
                case DayOfWeek.Thursday:
                    return "پنج شنبه";
                case DayOfWeek.Friday:
                    return "جمعه";
                case DayOfWeek.Saturday:
                    return "شنبه";
                default:
                    return "";
            }
        }
    }
}