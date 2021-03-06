﻿using System;
using System.Data;
using NUnit.Framework;
using SpecsFor;
using TinyORM.Mapping;
using Should;

namespace TinyORM.Tests.Mapping
{
    public class TinyMapperBoundariesTests
    {
        public static class given
        {
            public abstract class a_dataset_with_a_table_and_a_row : SpecsFor<TinyMapper>
            {
                protected DataSet Data { get; set; }

                protected override void Given()
                {
                    base.Given();

                    Data = Common.GenerateDataSet(1, 1);
                }
            }
        }

        [TestFixture]
        public class when_testing_null_handling : given.a_dataset_with_a_table_and_a_row
        {
            [Test]
            public void then_should_handle_input_for_value_types()
            {
                SUT.Map<int>(null).ShouldEqual(default(int));
                SUT.Map<bool>(null).ShouldEqual(default(bool));
                SUT.Map<string>(null).ShouldEqual(default(string));
            }
        }

        [TestFixture]
        public class when_testing_same_type_handling : given.a_dataset_with_a_table_and_a_row
        {
            [Test]
            public void then_should_return_the_same_dataset_that_was_given()
            {
                SUT.Map<DataSet>(Data).ShouldEqual(Data);
            }
        }

        [TestFixture]
        public class when_testing_bool_boundaries : given.a_dataset_with_a_table_and_a_row
        {
            [Test]
            public void then_should_return_false_when_parsing_bool_with_value_of_zero()
            {
                Data.Tables[0].Rows[0][0] = 0;

                SUT.Map<bool>(Data).ShouldBeFalse();
            }

            [Test]
            public void then_should_return_false_when_parsing_bool_with_value_of_one()
            {
                SUT.Map<bool>(Data).ShouldBeTrue();
            }

            [Test]
            public void then_should_return_false_when_parsing_bool_with_value_of_two()
            {
                Data.Tables[0].Rows[0][0] = 2;

                SUT.Map<bool>(Data).ShouldBeFalse();
            }
        }
    }
}