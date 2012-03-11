using System.Collections.Generic;
using System.Data;
using NUnit.Framework;
using SpecsFor;
using TinyORM.Mapping;
using Should;
using SpecsFor.ShouldExtensions;

namespace TinyORM.Tests.Mapping
{
    public class TinyMapperValueTests
    {
        public static class given
        {
            public abstract class a_dataset_with_a_table_and_ten_rows : SpecsFor<TinyMapper>
            {
                protected DataSet Data { get; set; }

                protected override void Given()
                {
                    base.Given();

                    Data = Common.GenerateDataSet(1, 10);
                }
            }
        }

        [TestFixture]
        public class when_testing_single_value_int_objects : given.a_dataset_with_a_table_and_ten_rows
        {
            [Test]
            public void then_should_return_valid_int()
            {
                SUT.Map<int>(Data).ShouldEqual(1);
            }

            [Test]
            public void then_should_return_valid_bool()
            {
                SUT.Map<bool>(Data).ShouldEqual(true);
            }

            [Test]
            public void then_should_return_valid_string()
            {
                SUT.Map<string>(Data).ShouldEqual("1");
            }

            [Test]
            public void then_should_return_valid_double()
            {
                SUT.Map<double>(Data).ShouldEqual(1.0D);
            }

            [Test]
            public void then_should_return_valid_long()
            {
                SUT.Map<long>(Data).ShouldEqual(1L);
            }

            [Test]
            public void then_should_return_valid_float()
            {
                SUT.Map<float>(Data).ShouldEqual(1.0F);
            }
        }

        [TestFixture]
        public class when_testing_lists_of_value_objects_from_int_value : given.a_dataset_with_a_table_and_ten_rows
        {
            [Test]
            public void then_should_return_valid_int()
            {
                SUT.Map<List<int>>(Data).ShouldLookLike(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            }

            [Test]
            public void then_should_return_valid_bool()
            {
                Data.Tables[0].Rows[9][0] = 0;

                SUT.Map<List<bool>>(Data).ShouldLookLike(new List<bool> { true, false, false, false, false, false, false, false, false, false });
            }

            [Test]
            public void then_should_return_valid_string()
            {
                SUT.Map<List<string>>(Data).ShouldLookLike(new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
            }

            [Test]
            public void then_should_return_valid_double()
            {
                SUT.Map<List<double>>(Data).ShouldLookLike(new List<double> { 1D, 2D, 3D, 4D, 5D, 6D, 7D, 8D, 9D, 10D });
            }

            [Test]
            public void then_should_return_valid_long()
            {
                SUT.Map<List<long>>(Data).ShouldLookLike(new List<long> { 1L, 2L, 3L, 4L, 5L, 6L, 7L, 8L, 9L, 10L });
            }

            [Test]
            public void then_should_return_valid_float()
            {
                SUT.Map<List<float>>(Data).ShouldLookLike(new List<float> { 1F, 2F, 3F, 4F, 5F, 6F, 7F, 8F, 9F, 10F });
            }
        }

    }
}