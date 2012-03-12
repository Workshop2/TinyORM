using System;
using System.Collections.Generic;
using NUnit.Framework;
using TinyORM.Mapping;
using SpecsFor;
using SpecsFor.ShouldExtensions;

namespace TinyORM.Tests.Mapping
{
    public class TinyMapperClassTests
    {
        
        [TestFixture]
        public class when_mapping_single_instances_of_classes : SpecsFor<TinyMapper>
        {
            [Test]
            public void then_should_map_correctly()
            {
                var timeStamp = DateTime.Now;
                var expected = Common.GenerateExpectedDummyObject(1);
                Common.PrintTime(timeStamp, DateTime.Now);

                timeStamp = DateTime.Now;
                var data = Common.GenerateDataSet(1, 1);
                Common.PrintTime(timeStamp, DateTime.Now);

                timeStamp = DateTime.Now;
                var mapped = SUT.Map<Common.DummyClass>(data);
                Common.PrintTime(timeStamp, DateTime.Now);

                timeStamp = DateTime.Now;
                mapped.ShouldLookLike(expected[0]);
                Common.PrintTime(timeStamp, DateTime.Now);
            }
        }
        
        [TestFixture]
        public class when_mapping_lists_of_classes : SpecsFor<TinyMapper>
        {
            private void RunTest(int numberToTest)
            {
                var timeStamp = DateTime.Now;
                var expected = Common.GenerateExpectedDummyObject(numberToTest);
                Common.PrintTime(timeStamp, DateTime.Now);

                timeStamp = DateTime.Now;
                var data = Common.GenerateDataSet(1, numberToTest);
                Common.PrintTime(timeStamp, DateTime.Now);

                timeStamp = DateTime.Now;
                var mapped = SUT.Map<List<Common.DummyClass>>(data);
                Common.PrintTime(timeStamp, DateTime.Now);

                timeStamp = DateTime.Now;
                mapped.ShouldLookLike(expected);
                Common.PrintTime(timeStamp, DateTime.Now);
            }

            [Test]
            public void then_ten_should_map_correctly()
            {
                RunTest(10);
            }

            [Test]
            public void then_one_hundred_should_map_correctly()
            {
                RunTest(100);
            }

            [Test]
            public void then_one_thousand_should_map_correctly()
            {
                RunTest(1000);
            }

            [Test]
            public void then_ten_thousand_should_map_correctly()
            {
                RunTest(10000);
            }

            //Stress tests
            [Test]
            public void then_one_hundred_thousand_should_map_correctly()
            {
                RunTest(100000);
            }

            //[Test]
            //public void then_one_million_should_map_correctly()
            //{
            //    RunTest(1000000);
            //}
        }

        [TestFixture]
        public class when_mapping_lists_of_wrong_classes : SpecsFor<TinyMapper>
        {
            private void RunTest(int numberToTest)
            {
                var timeStamp = DateTime.Now;
                var expected = Common.GenerateExpectedWrongDummyObject(numberToTest);
                Common.PrintTime(timeStamp, DateTime.Now);

                timeStamp = DateTime.Now;
                var data = Common.GenerateDataSet(1, numberToTest);
                Common.PrintTime(timeStamp, DateTime.Now);

                timeStamp = DateTime.Now;
                var mapped = SUT.Map<List<Common.DummyClassWrong>>(data);
                Common.PrintTime(timeStamp, DateTime.Now);

                timeStamp = DateTime.Now;
                mapped.ShouldLookLike(expected);
                Common.PrintTime(timeStamp, DateTime.Now);
            }

            [Test]
            public void then_ten_should_map_correctly()
            {
                RunTest(10);
            }
        }
    }
}