using NUnit.Framework;
using Blazor.ApartmentHandler.Shared;
using Blazor.ApartmentHandler.Shared.Entities;

namespace Testing
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }
        // [TestCase(arg1: new Bill(6, 4), arg2: 35.5)]
        [Test]
        [TestCaseSource("TestCases")]
        public void When_Bill_Input_Expect_Result(Bill bill, double expectedResult)
        {
            var sut = new Calculator();
            var doubleResult = sut.Calculate(bill);
            Assert.AreEqual(expectedResult, doubleResult);
        }

        public static readonly object[] TestCases =
        {
                new object[] {

                    new Bill()
                    {
                        WaterConsumption=4,
                        Apartment=new Apartment()
                        {
                            NumberOfPersons=6
                        }
                    },
                    23.5

                },
                new object[] {

                    new Bill()
                    {
                        WaterConsumption=4,
                        Apartment=new Apartment()
                        {
                            NumberOfPersons=4
                        }
                    },
                    26

                },
                 new object[] {

                    new Bill()
                    {
                        WaterConsumption=0,
                        Apartment=new Apartment()
                        {
                            NumberOfPersons=6
                        }
                    },
                    0

                },
                 new object[] {

                    new Bill()
                    {
                        WaterConsumption=0,
                        Apartment=new Apartment()
                        {
                            NumberOfPersons=4
                        }
                    },
                    0

                }
        };

    }
}
