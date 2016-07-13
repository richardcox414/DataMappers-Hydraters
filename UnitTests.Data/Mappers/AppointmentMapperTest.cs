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
    public class AppointmentMapperTest
    {
        private CustomerRequestMapper Target { get; set; }
        private Mock<IDatabase> MockDatabase { get; set; }
        private Mock<IQuery> MockQuery { get; set; }
        private Mock<IHydrater<CustomerRequestVO>> MockHydrater { get; set; }
        private Mock<IHydrater<AppointmentVO>> MockHydrater1 { get; set; }
        private List<CustomerRequestVO> TestHydratedCustomerRequests { get; set; }
        private Mock<AppointmentMapper> MockAppointmentMapper { get; set; }
        private Mock<CommentMapper> MockCommentMapper { get; set; }
        private AppointmentMapper Target1 { get; set; }
        private Mock<CustomerRequestMapper> MockCustomerRequestMapper { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeMocks();
            Target = new CustomerRequestMapper(MockDatabase.Object, MockHydrater.Object, MockAppointmentMapper.Object, MockCommentMapper.Object);
            Target1 = new AppointmentMapper(MockDatabase.Object, MockHydrater1.Object, MockCustomerRequestMapper.Object);
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
        public void GetAppointmentsByCustomerRequest_OnAny_SetsQueryParameter()
        {
            var customerRequestArgument = new CustomerRequestVO();
            customerRequestArgument.Appointments.Add(new AppointmentVO {Identity = 1});

            var actual = Target.GetCustomerRequestByIdentity(new CustomerRequestVO {Appointme})
        }

        [TestMethod]
        public void GetAppointmentsByCustomerRequest_HasQueryMatches_ReturnsHydratedEntities()
        {
            var customerRequestArgument = new CustomerRequestVO();
            customerRequestArgument.Appointments.Add(new AppointmentVO());

         //   var actual = Target.GetAppointmentsByCustomerRequest
        }
    }
}
