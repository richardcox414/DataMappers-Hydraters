using EnterpriseSystems.Data.DAO;
using EnterpriseSystems.Data.Hydraters;
using EnterpriseSystems.Data.Mappers;
using EnterpriseSystems.Data.Model.Constants;
using EnterpriseSystems.Data.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data;

namespace UnitTests.Data.Mappers
{
    [TestClass]
    public class CommentMapperTest
    {
        private CommentMapper Target { get; set; }
        private Mock<IDatabase> MockDatabase { get; set; }
        private Mock<IQuery> MockQuery { get; set; }
        private Mock<IHydrater<CommentVO>> MockHydrater { get; set; }
        private List<CommentVO> TestHydratedComments { get; set; }
        private Mock<AppointmentMapper> MockAppointmentMapper { get; set; }
        private Mock<CustomerRequestMapper> MockCustomerRequestMapper { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeMocks();
            Target = new CommentMapper(MockDatabase.Object, MockHydrater.Object, MockAppointmentMapper.Object, MockCustomerRequestMapper.Object);
        }

        private void InitializeMocks()
        {
            MockDatabase = new Mock<IDatabase>();
            MockQuery = new Mock<IQuery>();
            MockHydrater = new Mock<IHydrater<CustomerRequestVO>>();
            TestHydratedCustomerRequests = new List<CustomerRequestVO>();
            MockAppointmentMapper = new Mock<AppointmentMapper>();
            MockCommentMapper = new Mock<CommentMapper>();

            MockDatabase.Setup(m => m.CreateQuery(It.IsAny<string>())).Returns(MockQuery.Object);
            MockHydrater.Setup(m => m.Hydrate(It.IsAny<DataTable>())).Returns(TestHydratedCustomerRequests);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Target = null;
            MockDatabase = null;
            MockQuery = null;
            MockHydrater = null;
            TestHydratedCustomerRequests = null;
            MockAppointmentMapper = null;
            MockCommentMapper = null;
        }

        [TestMethod]
        public void GetCommentsByCustomerRequest_OnAny_SetsQueryParameter()
        {
            var actual =
                Target.GetCustomerRequestByIdentity(new CustomerRequestVO {Appointments = new List<AppointmentVO>()});

        }

        [TestMethod]
        public void GetCommentsByCustomerRequest_HasQueryMatches_ReturnsHydratedEntities()
        {
            var hydratedEntity = new CustomerRequestVO();
            TestHydratedCustomerRequests.Add(hydratedEntity);

            var actual =
        }
    }
}