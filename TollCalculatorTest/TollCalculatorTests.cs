using Microsoft.VisualStudio.TestTools.UnitTesting;
using TollFeeCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Runtime.ConstrainedExecution;
using Newtonsoft.Json.Linq;

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
        private readonly DateTime[] oneWeekNoonTimes;
        private readonly DateTime[] twoFilledDaysTimes;
        private readonly DateTime[] holidays2024DateTimes;
        private readonly DateTime[] holidays2025DateTimes;
        private readonly DateTime[] multiOneHourTimes;
        private readonly DateTime[] reverseMultiOneHourTimes;
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
                new DateTime(2024, 03, 31, 16, 00, 00),
                new DateTime(2024, 04, 01, 16, 30, 00),
                new DateTime(2024, 05, 01, 16, 30, 00),
                new DateTime(2024, 05, 09, 16, 30, 00),
                new DateTime(2024, 06, 06, 16, 30, 00),
                new DateTime(2024, 12, 25, 16, 30, 00),
                new DateTime(2024, 12, 26, 16, 30, 00),
            };
            holidays2025DateTimes = new DateTime[] {
                new DateTime(2025, 01, 01, 16, 30, 00),
                new DateTime(2025, 01, 06, 16, 30, 00),
                new DateTime(2025, 04, 18, 07, 00, 00),
                new DateTime(2025, 04, 20, 16, 00, 00),
                new DateTime(2025, 04, 21, 16, 30, 00),
                new DateTime(2025, 05, 01, 16, 30, 00),
                new DateTime(2025, 05, 29, 16, 30, 00),
                new DateTime(2025, 06, 06, 16, 30, 00),
                new DateTime(2025, 12, 25, 16, 30, 00),
                new DateTime(2025, 12, 26, 16, 30, 00),
            };
            multiOneHourTimes = new DateTime[] {
                new DateTime(2024, 09, 16, 06, 00, 00),
                new DateTime(2024, 09, 16, 06, 15, 00),
                new DateTime(2024, 09, 16, 06, 30, 00),
                new DateTime(2024, 09, 16, 06, 45, 00),
            };
            reverseMultiOneHourTimes = new DateTime[] {
                new DateTime(2024, 09, 16, 06, 45, 00),
                new DateTime(2024, 09, 16, 06, 30, 00),
                new DateTime(2024, 09, 16, 06, 15, 00),
                new DateTime(2024, 09, 16, 06, 00, 00),
            };
        }
        [TestMethod()]
        public void TollFeesEmptyTest()
        {
            int value = _tollCalculator.GetTollFee(car, emptyDateTimes);

            Assert.AreEqual(0, value);
        }
        [TestMethod()]
        public void TollFeesNullTest()
        {
            int value = _tollCalculator.GetTollFee(null, null);

            Assert.AreEqual(0, value);
        }

        [TestMethod()]
        public void TollFeesRushHourTest()
        {
            int value = _tollCalculator.GetTollFee(car, rushHourDayTimes);

            Assert.AreEqual(36, value);
        }
        [TestMethod()]
        public void TollFeesShuffledRushHourTest()
        {
            int value = _tollCalculator.GetTollFee(car, shuffledRushHourDayTimes);

            Assert.AreEqual(36, value);
        }

        
        [TestMethod()]
        public void TollFeesEaster2024Test()
        {
            int value = _tollCalculator.GetTollFee(car, easter2024Times);

            Assert.AreEqual(0, value);
        }


        [TestMethod()]
        public void TollFeesOneWeekNoonTest()
        {
            int value = _tollCalculator.GetTollFee(car, oneWeekNoonTimes);

            Assert.AreEqual(40, value);
        }

        [TestMethod()]
        public void TollFeesTwoFilledDaysTest()
        {
            int value = _tollCalculator.GetTollFee(car, twoFilledDaysTimes);

            Assert.AreEqual(120, value);
        }
        [TestMethod()]
        public void TollFeesHolidays2024Test()
        {
            int value = _tollCalculator.GetTollFee(car, holidays2024DateTimes);

            Assert.AreEqual(0, value);
        }
        [TestMethod()]
        public void TollFeesHolidays2025Test()
        {
            int value = _tollCalculator.GetTollFee(car, holidays2025DateTimes);

            Assert.AreEqual(0, value);
        }
        [TestMethod()]
        public void TollMultiOneHourTest()
        {
            int value = _tollCalculator.GetTollFee(car, multiOneHourTimes);

            Assert.AreEqual(0, value);
        }
        

        [TestMethod()]
        public void TollReverseMultiOneHourTest()
        {
            int value = _tollCalculator.GetTollFee(car, reverseMultiOneHourTimes);

            Assert.AreEqual(0, value);
        }
        [TestMethod()]
        public void TollFeesMotorbikeTest()
        {
            int value = _tollCalculator.GetTollFee(motorbike, rushHourDayTimes);

            Assert.AreEqual(0, value);
        }

        [TestMethod()]
        public void TollFeesTractorTest()
        {
            //int value = _tollCalculator.GetTollFee(motorbike, rushHourDayTimes);

            //    Assert.AreEqual(0, value);
            Assert.Fail();
        }
        [TestMethod()]
        public void TollFeesEmergencyTest()
        {
            //int value = _tollCalculator.GetTollFee(motorbike, rushHourDayTimes);

            //    Assert.AreEqual(0, value);
            Assert.Fail();
        }
        [TestMethod()]
        public void TollFeesDiplomatTest()
        {
            //int value = _tollCalculator.GetTollFee(motorbike, rushHourDayTimes);

            //    Assert.AreEqual(0, value);
            Assert.Fail();
        }
        [TestMethod()]
        public void TollFeesForeignTest()
        {
            //int value = _tollCalculator.GetTollFee(motorbike, rushHourDayTimes);

            //    Assert.AreEqual(0, value);
            Assert.Fail();
        }
        [TestMethod()]
        public void TollFeesMilitaryTest()
        {
            //int value = _tollCalculator.GetTollFee(motorbike, rushHourDayTimes);

            //    Assert.AreEqual(0, value);
            Assert.Fail();
        }
    }
}