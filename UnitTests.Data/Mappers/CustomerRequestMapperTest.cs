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
    public class CustomerRequestMapperTest
    {
        private CustomerRequestMapper Target { get; set; }
        private Mock<IDatabase> MockDatabase { get; set; }
        private Mock<IQuery> MockQuery { get; set; }
        private Mock<IHydrater<CustomerRequestVO>> MockHydrater { get; set; }
        private List<CustomerRequestVO> TestHydratedCustomerRequests { get; set; }
        private Mock<AppointmentMapper> MockAppointmentMapper { get; set; }
        private Mock<CommentMapper> MockCommentMapper { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeMocks();
            Target = new CustomerRequestMapper(MockDatabase.Object, MockHydrater.Object, MockAppointmentMapper.Object, MockCommentMapper.Object);
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
        public void GetCustomerRequestByIdentity_NoQueryMatch_ReturnsNull()
        {
            var actual = Target.GetCustomerRequestByIdentity(new CustomerRequestVO());

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void GetCustomerRequestByIdentity_OnAny_SetsQueryParameter()
        {
            var actual = Target.GetCustomerRequestByIdentity(new CustomerRequestVO { Identity = 1 });

            MockQuery.Verify(m => m.AddParameter(1, CustomerRequestQueryParameters.Identity), Times.Once);
        }

        [TestMethod]
        public void GetCustomerRequestByIdentity_HasQueryMatch_ReturnsHydratedEntity()
        {
            var hydratedEntity = new CustomerRequestVO();
            TestHydratedCustomerRequests.Add(hydratedEntity);

            var actual = Target.GetCustomerRequestByIdentity(new CustomerRequestVO());

            Assert.AreEqual(hydratedEntity, actual);
        }

        [TestMethod]
        public void GetCustomerRequestByIdentity_HasQueryMatch_FillsRelatedEntities()
        {
            TestHydratedCustomerRequests.Add(new CustomerRequestVO());

            var actual = Target.GetCustomerRequestByIdentity(new CustomerRequestVO());

            MockAppointmentMapper.Verify(m => m.GetAppointmentsByCustomerRequest(It.IsAny<CustomerRequestVO>()), Times.Once);
            MockCommentMapper.Verify(m => m.GetCommentsByCustomerRequest(It.IsAny<CustomerRequestVO>()), Times.Once);
        }


        [TestMethod]
        public void GetCustomerRequestsByReferenceNumberAndBusinessName_OnAny_SetsQueryParameters()
        {
            var customerRequestArgument = new CustomerRequestVO { BusinessEntityKey = "BusinessName" };
            customerRequestArgument.ReferenceNumbers.Add(new ReferenceNumberVO { ReferenceNumber = "ReferenceNumber" });

            var actual = Target.GetCustomerRequestsByReferenceNumberAndBusinessName(customerRequestArgument);

            MockQuery.Verify(m => m.AddParameter("BusinessName", CustomerRequestQueryParameters.BusinessName), Times.Once);
            MockQuery.Verify(m => m.AddParameter("ReferenceNumber", ReferenceNumberQueryParameters.ReferenceNumber), Times.Once);
        }

        [TestMethod]
        public void GetCustomerRequestsByReferenceNumberAndBusinessName_HasQueryMatches_ReturnsHydratedEntities()
        {
            var customerRequestArgument = new CustomerRequestVO();
            customerRequestArgument.ReferenceNumbers.Add(new ReferenceNumberVO());

            var actual = Target.GetCustomerRequestsByReferenceNumberAndBusinessName(customerRequestArgument);

            Assert.AreEqual(TestHydratedCustomerRequests, actual);
        }

        [TestMethod]
        public void GetCustomerRequestsByReferenceNumberAndBusinessName_HasQueryMatches_FillsRelatedEntities()
        {
            var customerRequestArgument = new CustomerRequestVO();
            customerRequestArgument.ReferenceNumbers.Add(new ReferenceNumberVO());
            TestHydratedCustomerRequests.Add(new CustomerRequestVO());
            TestHydratedCustomerRequests.Add(new CustomerRequestVO());

            var actual = Target.GetCustomerRequestsByReferenceNumberAndBusinessName(customerRequestArgument);

            MockAppointmentMapper.Verify(m => m.GetAppointmentsByCustomerRequest(It.IsAny<CustomerRequestVO>()), Times.Exactly(2));
            MockCommentMapper.Verify(m => m.GetCommentsByCustomerRequest(It.IsAny<CustomerRequestVO>()), Times.Exactly(2));
        }
    }
}
