using System;
using System.Data;
using Moq;
using NUnit.Framework;
using Should;
using SpecsFor;
using SpecsFor.ShouldExtensions;
using TinyORM.Connection;
using TinyORM.Exceptions;
using TinyORM.Mapping;

namespace TinyORM.Tests.DbTests
{
    public class DbExecuteTests
    {
        public class given
        {
            public abstract class a_mocked_db_for_datasets : SpecsFor<Db>
            {
                protected DataSet InputValue { get; set; }
                protected string SqlString { get; set; }
                protected Mock<IMapper> MockedMapper { get; set; }
                protected Mock<ITinyConnection> MockedConnection { get; set; }

                protected override void Given()
                {
                    SqlString = "usp_TestSproc";

                    InputValue = new DataSet();
                    InputValue.Tables.Add(new DataTable());
                    InputValue.Tables[0].Columns.Add("TestingColumn");
                    InputValue.Tables[0].Rows.Add(InputValue.Tables[0].NewRow());
                    
                    GenerateMapperMock();
                    GenerateConnectionMock();
                    
                    SUT.CommandTimeout = new TimeSpan(0, 1, 0);
                }

                private void GenerateMapperMock()
                {
                    MockedMapper = GetMockFor<IMapper>();

                    MockedMapper.Setup(i => i.Map<DataSet>(InputValue))
                        .Returns(InputValue)
                        .Verifiable();

                    SUT.Mapper = MockedMapper.Object;
                }

                private void GenerateConnectionMock()
                {
                    MockedConnection = GetMockFor<ITinyConnection>();

                    //MockedConnection
                    //    .Setup(i => i.ExecuteScalar(null, It.Is<string>(x => x == SUT.SqlConnectionTestScript), null, It.IsAny<TimeSpan>()))
                    //    .Throws(new TinyDbException())
                    //    .Verifiable();

                    MockedConnection
                        .Setup(i => i.ExecuteScalar(null, It.Is<string>(x => x == SqlString), null, It.IsAny<TimeSpan>()))
                        .Returns(InputValue)
                        .Verifiable();

                    SUT.DbConnection = MockedConnection.Object;
                }
            }
        }

        [TestFixture]
        public class when_executing_a_dataset_via_stored_procedure : given.a_mocked_db_for_datasets
        {
            private DbResultInfoRtn<DataSet> RtnVal { get; set; }
            protected override void When()
            {
                RtnVal = SUT.Execute<DataSet>(SqlString);
            }
            
            [Test]
            public void then_should_be_successfull()
            {
                RtnVal.Success.ShouldBeTrue();
            }

            [Test]
            public void then_return_value_should_match_expected_sql_value()
            {
                RtnVal.Value.ShouldLookLike(InputValue);
            }
        }
    }
}