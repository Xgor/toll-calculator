using System;
using System.Globalization;
using TollFeeCalculator;

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

            //

            // Om jag ska behålla intervalStart så behövs sortera dates i tidsordning. Man hoppas det är i tidsordning men man vet aldrig
            DateTime intervalStart = dates[0];
            int totalFee = 0;
            foreach (DateTime date in dates)
            {
                int nextFee = GetTollFee(date, vehicle);
                int tempFee = GetTollFee(intervalStart, vehicle); // Funderar man kan ha denna utanför loopen så man inte behöver hämta fees två gånger per loop

                long diffInMillies = date.Millisecond - intervalStart.Millisecond;
                long minutes = diffInMillies / 1000 / 60; // Varför inte bara göra date.minutes ?

                // Denna kodsnutten är mest förvirrande och tror inte den ens funkar. 
                // Tror den är för att kolla så det inte blir för flera gånger men den kollar bara mot ett datum och kollar om det är större inte mindre
                // Tror idéen är att uppdatera interval start så kollar med den närmaste men det händer inte just nu.
                if (minutes <= 60)
                {
                    if (totalFee > 0) totalFee -= tempFee;
                    if (nextFee >= tempFee) tempFee = nextFee;
                    totalFee += tempFee;
                }
                else
                {
                    totalFee += nextFee;
                }
            }
            if (totalFee > 60) totalFee = 60; // Felplacerad. Detta gör så att det bara går att max få 60 kr på fakturan Behövs nog en loop för varje dag.
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
            else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
            else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
            else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
            else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
            else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            else return 0;
        }

        private Boolean IsTollFreeDate(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

            if (year == 2013) // Detta behöver modiferas för att kunna hantera högtider. Specifikt Midsommar, Kristi himmelsfärd och Påsk behöver jag kolla in.
            {
                if (month == 1 && (day == 1 || day == 6) || // La till Trettondag
                    month == 3 && (day == 28 || day == 29) || // Modifera för påsk med DateUtils.EasterSunday
                    month == 4 && (day == 1 || day == 30) || // Första april är inte högtid... inte heller valborg
                    month == 5 && (day == 1 || day == 8 || day == 9) ||
                    month == 6 && (day == 5 || day == 6 || day == 21) || // 5 juni högtid?
                    month == 7 || // Är hela juli en högtid?
                    month == 11 && day == 1 || // Behövs Alla helgons dag hanteras? Är på en lördag
                    month == 12 && (day == 24 || day == 25 || day == 26 || day == 31)) // Räknas julafton och nyårsdagen?
                {
                    return true;
                }
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


        static void Main(string[] args)
        {
            // Display the number of command line arguments.
            Console.WriteLine(args.Length);
        }
    }
}

/*
public class TollTester
{
    public void runUnitTest()
    {
        TollCalculator toll = new TollCalculator();
        Car testCar = new Car();
        DateTime[] testChristmas = new DateTime[new DateTime(2013, 12, 25, 10, 30, 50),new DateTime(2013, 12, 25, 10, 50, 50)]
        if 0 ==toll.GetTollFee(testCar,testChristmas):
            Console.Write("testChristmas SUCCESS");
        else:
            Console.Write("testChristmas FAIL");
        DateTime[] testNormal = new DateTime[new DateTime(2013, 12, 22, 10, 30, 50),new DateTime(2013, 12, 22, 10, 50, 50)]
        
        if 0 ==toll.GetTollFee(testCar,testNormal):
            Console.Write("testChristmas SUCCESS");
        else:
            Console.Write("testChristmas FAIL");
    }
}
*/


// Från
// https://web.archive.org/web/20120223154950/https://aa.usno.navy.mil/faq/docs/easter.php
//  https://codereview.stackexchange.com/questions/193847/find-easter-on-any-given-year
// Gauss Easter algorithm

/*
public static DateTime EasterSunday(int year)
{
    int day = 0;
    int month = 0;

    int g = year % 19;
    int c = year / 100;
    int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
    int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

    day   = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
    month = 3;

    if (day > 31)
    {
        month++;
        day -= 31;
    }

    return new DateTime(year, month, day);
}
*/