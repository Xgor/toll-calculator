using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TollFeeCalculator
{
    public class TollCalculator
    {

        /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day
         * @return - the total toll fee for that day
         */

        public int GetTollFee(Vehicle vehicle, DateTime[] dates)
        {
            // Return zero if array is empty
            if (dates == null || dates.Length == 0) return 0;

            Array.Sort(dates);
            var dateLists = new List<DateTime[]>();
            List<DateTime> d = new List<DateTime>();
            foreach(DateTime date in dates)
            {
                if (d.Count > 0 && d.FirstOrDefault().Date != date.Date)
                {
                    dateLists.Add(d.ToArray());
                    d = new List<DateTime>();
                }
                d.Add(date);
            }
            dateLists.Add(d.ToArray());

            DateTime intervalStart = DateTime.MinValue;
            int intervallFee = 0;
            int totalFee = 0;
            foreach (DateTime[] dateList in dateLists)
            {
                int dailyFee = 0;
                foreach (DateTime date in dateList)
                {
                    int nextFee = GetTollFee(date, vehicle);

                    double minutes = 999;
                    if (intervalStart != DateTime.MinValue)
                        minutes = date.TimeOfDay.TotalMinutes - intervalStart.TimeOfDay.TotalMinutes;

                    if (date.Date == intervalStart.Date && minutes <= 60 && nextFee >= intervallFee)
                    {
                        dailyFee += nextFee - intervallFee;
                        intervallFee = nextFee;
                    }
                    else
                    {
                        dailyFee += nextFee;
                        intervallFee = nextFee;
                        intervalStart = date;
                    }
                }
                if (dailyFee > 60) dailyFee = 60; 
                totalFee += dailyFee;
            }
            return totalFee;
        }

        private bool IsTollFreeVehicle(Vehicle vehicle) // TODO: Skapa Tractor, Emergency,Diplomat ,Foreign,Military
        {
            if (vehicle == null) return false;
            String vehicleType = vehicle.GetVehicleType();
            return vehicleType.Equals(TollFreeVehicles.Motorbike.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Military.ToString());
            // Denna return är super ful, kan göra den mer elegant Kommer inte fungera nog för den checkar string med int
        }

        public int GetTollFee(DateTime date, Vehicle vehicle)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

            int hour = date.Hour;
            int minute = date.Minute;

            // Känns som man skulle kunna göra denna lite mer läsbar. Finn definift delar som inte behövs
            if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            else if (hour == 6 && minute >= 30) return 13;
            else if (hour == 7) return 18;
            else if (hour == 8 && minute <= 29) return 13;
            else if (hour <= 14) return 8;
            else if (hour == 15 && minute <= 29) return 13;
            else if (hour <= 16 ) return 18;
            else if (hour == 17) return 13;
            else if (hour == 18 && minute <= 29) return 8;
            else return 0;
        }

        private Boolean IsTollFreeDate(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

            date = date.Date;
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            var easterSunday = EasterSunday(year);
            var midsummerEve = GetMidsummerEve(year);

            // Unsure if Christmas eve and midsummer eve should be counted here but impemented them

            if (month == 1 && (day == 1 || day == 6) || // New years eve and epiphany Eve
                month == 5 && (day == 1) || // First of may
                month == 6 && (day == 6) || // Swedish national day
                month == 12 && (day == 24 || day == 25 || day == 26) || // Christmas 
                easterSunday.AddDays(-2) == date || // Good Friday
                easterSunday.AddDays(1) == date || // Easter Monday
                easterSunday.AddDays(39) == date || // Ascension Day
                midsummerEve == date) // midsummer
            {
                return true;
            }
            return false;
        }

        private enum TollFreeVehicles
        {
            Motorbike = 0,
            Tractor = 1,
            Emergency = 2,
            Diplomat = 3,
            Foreign = 4,
            Military = 5
        }

        // From
        // https://web.archive.org/web/20120223154950/https://aa.usno.navy.mil/faq/docs/easter.php
        // https://codereview.stackexchange.com/questions/193847/find-easter-on-any-given-year
        // Gauss Easter algorithm
        public static DateTime EasterSunday(int year)
        {
            int day = 0;
            int month = 0;

            int g = year % 19;
            int c = year / 100;
            int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
            int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

            day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
            month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return new DateTime(year, month, day);
        }
        
        public DateTime GetMidsummerEve(int year)
        {
            DateTime d = new DateTime(year, 6, 19);
            while(d.DayOfWeek != DayOfWeek.Friday) d = d.AddDays(1);
            return d;
        }

    }
}

