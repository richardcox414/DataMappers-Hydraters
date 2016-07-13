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
        private AppointmentMapper Target { get; set; }
        private Mock<IDatabase> MockDatabase { get; set; }
        private Mock<IQuery> MockQuery { get; set; }
        private Mock<IHydrater<AppointmentVO>> MockHydrater { get; set; }
        private List<AppointmentVO> TestHydratedAppoints { get; set; }
        private Mock<CommentMapper> MockCommentMapper { get; set; }
        private Mock<CustomerRequestMapper> MockCustomerRequestMapper { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeMocks();
            Target = new AppointmentMapper(MockDatabase.Object, MockHydrater.Object, MockCustomerRequestMapper.Object, MockCommentMapper.Object);
        }

        private void InitializeMocks()
        {
            MockDatabase = new Mock<IDatabase>();
            MockQuery = new Mock<IQuery>();
            MockHydrater = new Mock<IHydrater<AppointmentVO>>();
            TestHydratedAppoints = new List<AppointmentVO>();
            MockCustomerRequestMapper = new Mock<CustomerRequestMapper>();
            MockCommentMapper = new Mock<CommentMapper>();

            MockDatabase.Setup(m => m.CreateQuery(It.IsAny<string>())).Returns(MockQuery.Object);
            MockHydrater.Setup(m => m.Hydrate(It.IsAny<DataTable>())).Returns(TestHydratedAppoints);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Target = null;
            MockDatabase = null;
            MockQuery = null;
            MockHydrater = null;
            TestHydratedAppoints = null;
            MockCustomerRequestMapper = null;
            MockCommentMapper = null;
        }

        [TestMethod]
        public void GetAppointmentsByCustomerRequest_OnAny_SetsQueryParameter()
        {
            var customerRequestArgument = new CustomerRequestVO { BusinessEntityKey = "BusinessName" };
            customerRequestArgument.Appointments.Add(new AppointmentVO { Identity = 1 });

            var actual = Target.GetAppointmentsByCustomerRequest(customerRequestArgument);

            MockQuery.Verify(m => m.AddParameter("BusinessName", CustomerRequestQueryParameters.BusinessName), Times.Once);
            MockQuery.Verify(m => m.AddParameter("Identity", StopQueryParameters.Identity), Times.Once);
        }

        [TestMethod]
        public void GetAppointmentsByCustomerRequest_HasQueryMatches_ReturnsHydratedEntities()
        {

            var customerRequestArgument = new CustomerRequestVO();
            customerRequestArgument.Appointments.Add(new AppointmentVO());

            var actual = Target.GetAppointmentsByCustomerRequest(customerRequestArgument);

            Assert.AreEqual(TestHydratedAppoints, actual);
        }
    }
}
