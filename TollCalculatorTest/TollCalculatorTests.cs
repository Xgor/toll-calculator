using Microsoft.VisualStudio.TestTools.UnitTesting;
using TollFeeCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Runtime.ConstrainedExecution;

namespace TollFeeCalculator.Tests
{
    [TestClass()]
    public class TollCalculatorTests
    {
        private readonly TollCalculator _tollCalculator;
        private readonly Vehicle car;
        private readonly Vehicle motorbike;
        private readonly DateTime[] emptyDateTimes;
        private readonly DateTime[] rushHourDayTimes;
        private readonly DateTime[] shuffledRushHourDayTimes;
        private readonly DateTime[] easter2024Times;
        private readonly DateTime[] easter2025Times;
        private readonly DateTime[] oneWeekNoonTimes;
        private readonly DateTime[] twoFilledDaysTimes;
        private readonly DateTime[] holidays2024DateTimes;
        public TollCalculatorTests()
        {
            _tollCalculator = new TollCalculator();
            car = new Car();
            motorbike = new Motorbike();
            emptyDateTimes = Array.Empty<DateTime>();
            rushHourDayTimes = new DateTime[] { 
                new DateTime(2024, 09, 16, 07, 00, 00),
                new DateTime(2024, 09, 16, 07, 30, 00),
                new DateTime(2024, 09, 16, 16, 00, 00),
                new DateTime(2024, 09, 16, 16, 30, 00),
            };
            shuffledRushHourDayTimes = new DateTime[] {
                new DateTime(2024, 09, 16, 16, 30, 00),
                new DateTime(2024, 09, 16, 16, 00, 00),
                new DateTime(2024, 09, 16, 07, 30, 00),
                new DateTime(2024, 09, 16, 07, 00, 00),

            };
            easter2024Times = new DateTime[] {
                new DateTime(2024, 03, 29, 07, 00, 00),
                new DateTime(2024, 03, 30, 07, 30, 00),
                new DateTime(2024, 03, 31, 16, 00, 00),
                new DateTime(2024, 04, 01, 16, 30, 00),
            };
            easter2025Times = new DateTime[] {
                new DateTime(2025, 04, 18, 07, 00, 00),
                new DateTime(2025, 04, 19, 07, 30, 00),
                new DateTime(2025, 04, 20, 16, 00, 00),
                new DateTime(2025, 04, 21, 16, 30, 00),
            };
            oneWeekNoonTimes = new DateTime[] {
                new DateTime(2024, 09, 16, 12, 00, 00),
                new DateTime(2024, 09, 17, 12, 30, 00),
                new DateTime(2024, 09, 18, 12, 00, 00),
                new DateTime(2024, 09, 19, 12, 30, 00),
                new DateTime(2024, 09, 20, 12, 30, 00),
                new DateTime(2024, 09, 21, 12, 00, 00),
                new DateTime(2024, 09, 22, 12, 30, 00),
            };

            
            twoFilledDaysTimes = new DateTime[] {
                new DateTime(2024, 09, 16, 6, 00, 00),
                new DateTime(2024, 09, 16, 7, 00, 00),
                new DateTime(2024, 09, 16, 8, 00, 00),
                new DateTime(2024, 09, 16, 9, 00, 00),
                new DateTime(2024, 09, 16, 10, 00, 00),
                new DateTime(2024, 09, 16, 11, 00, 00),
                new DateTime(2024, 09, 16, 12, 00, 00),
                new DateTime(2024, 09, 16, 13, 00, 00),
                new DateTime(2024, 09, 16, 14, 00, 00),
                new DateTime(2024, 09, 16, 15, 00, 00),
                new DateTime(2024, 09, 16, 16, 00, 00),
                new DateTime(2024, 09, 16, 17, 00, 00),
                new DateTime(2024, 09, 16, 18, 00, 00),
                new DateTime(2024, 09, 16, 19, 00, 00),
                new DateTime(2024, 09, 17, 6, 00, 00),
                new DateTime(2024, 09, 17, 7, 00, 00),
                new DateTime(2024, 09, 17, 8, 00, 00),
                new DateTime(2024, 09, 17, 9, 00, 00),
                new DateTime(2024, 09, 17, 10, 00, 00),
                new DateTime(2024, 09, 17, 11, 00, 00),
                new DateTime(2024, 09, 17, 12, 00, 00),
                new DateTime(2024, 09, 17, 13, 00, 00),
                new DateTime(2024, 09, 17, 14, 00, 00),
                new DateTime(2024, 09, 17, 15, 00, 00),
                new DateTime(2024, 09, 17, 16, 00, 00),
                new DateTime(2024, 09, 17, 17, 00, 00),
                new DateTime(2024, 09, 17, 18, 00, 00),
                new DateTime(2024, 09, 17, 19, 00, 00),
            };
            holidays2024DateTimes = new DateTime[] {
                new DateTime(2024, 01, 01, 16, 30, 00),
                new DateTime(2024, 01, 06, 16, 30, 00),
                new DateTime(2024, 03, 29, 07, 00, 00),
                new DateTime(2024, 03, 30, 07, 30, 00),
                new DateTime(2024, 03, 31, 16, 00, 00),
                new DateTime(2024, 04, 01, 16, 30, 00),
                new DateTime(2024, 05, 01, 16, 30, 00),
                new DateTime(2024, 05, 09, 16, 30, 00),
                new DateTime(2024, 05, 19, 16, 30, 00),
                new DateTime(2024, 06, 06, 16, 30, 00),
                new DateTime(2024, 12, 25, 16, 30, 00),
                new DateTime(2024, 12, 26, 16, 30, 00),
            };
        }
        [TestMethod()]
        public void TollFeeEmptyTest()
        {
            int value = _tollCalculator.GetTollFee(car, emptyDateTimes);

            Assert.AreEqual(0, value);
        }
        [TestMethod()]
        public void TollFeeNullTest()
        {
            int value = _tollCalculator.GetTollFee(null, null);

            Assert.AreEqual(0, value);
        }

        [TestMethod()]
        public void TollMotorbikeTest()
        {
            int value = _tollCalculator.GetTollFee(motorbike, rushHourDayTimes);

            Assert.AreEqual(0, value);
        }

        [TestMethod()]
        public void TollRushHourTest()
        {
            int value = _tollCalculator.GetTollFee(car, rushHourDayTimes);

            Assert.AreEqual(36, value);
        }
        [TestMethod()]
        public void TollShuffledRushHourTest()
        {
            int value = _tollCalculator.GetTollFee(car, shuffledRushHourDayTimes);

            Assert.AreEqual(36, value);
        }

        
        [TestMethod()]
        public void TollEaster2024Test()
        {
            int value = _tollCalculator.GetTollFee(car, easter2024Times);

            Assert.AreEqual(36, value);
        }

        [TestMethod()]
        public void TollEaster2025Test()
        {
            int value = _tollCalculator.GetTollFee(car, easter2025Times);

            Assert.AreEqual(36, value);
        }

        [TestMethod()]
        public void TollOneWeekNoonTest()
        {
            int value = _tollCalculator.GetTollFee(car, oneWeekNoonTimes);

            Assert.AreEqual(40, value);
        }

        [TestMethod()]
        public void TollTwoFilledDaysTest()
        {
            int value = _tollCalculator.GetTollFee(car, oneWeekNoonTimes);

            Assert.AreEqual(120, value);
        }
        [TestMethod()]
        public void TollHolidays2024Test()
        {
            int value = _tollCalculator.GetTollFee(car, holidays2024DateTimes);

            Assert.AreEqual(0, value);
        }
        

    }
}