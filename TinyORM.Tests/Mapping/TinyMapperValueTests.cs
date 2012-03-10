using System.Data;
using NUnit.Framework;
using SpecsFor;
using TinyORM.Mapping;
using Should;

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
        public class when_testing_single_value_objects : given.a_dataset_with_a_table_and_ten_rows
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
        }

    }
}