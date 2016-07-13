using EnterpriseSystems.Data.DAO;
using EnterpriseSystems.Data.Hydraters;
using EnterpriseSystems.Data.Mappers;
using EnterpriseSystems.Data.Model.Constants;
using EnterpriseSystems.Data.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data;
using System.Threading;

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
            Target = new CommentMapper(MockDatabase.Object, MockHydrater.Object, MockCustomerRequestMapper.Object, MockAppointmentMapper.Object);
        }

        private void InitializeMocks()
        {
            MockDatabase = new Mock<IDatabase>();
            MockQuery = new Mock<IQuery>();
            MockHydrater = new Mock<IHydrater<CommentVO>>();
            TestHydratedComments = new List<CommentVO>();
            MockAppointmentMapper = new Mock<AppointmentMapper>();
            MockCustomerRequestMapper = new Mock<CustomerRequestMapper>();


            MockDatabase.Setup(m => m.CreateQuery(It.IsAny<string>())).Returns(MockQuery.Object);
            MockHydrater.Setup(m => m.Hydrate(It.IsAny<DataTable>())).Returns(TestHydratedComments);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Target = null;
            MockDatabase = null;
            MockQuery = null;
            MockHydrater = null;
            TestHydratedComments = null;
            MockAppointmentMapper = null;
            MockCustomerRequestMapper = null;
        }

        [TestMethod]
        public void GetCommentsByCustomerRequest_OnAny_SetsQueryParameter()
        {
            var customerRequestArguement = new CustomerRequestVO() {BusinessEntityKey = "BusinessName"};
            customerRequestArguement.Comments.Add(new CommentVO { CommentType = "CommentType"});
            var actual =
                Target.GetCommentsByCustomerRequest(customerRequestArguement);

            MockQuery.Verify(m => m.AddParameter("BusinessName", CustomerRequestQueryParameters.BusinessName), Times.Once);
            MockQuery.Verify(m => m.AddParameter("Identity", CustomerRequestQueryParameters.Identity), Times.Once);
        }

        [TestMethod]
        public void GetCommentsByCustomerRequest_HasQueryMatches_ReturnsHydratedEntities()
        {
            var customerRequestArgument = new CustomerRequestVO();
            customerRequestArgument.Comments.Add(new CommentVO());

            var actual = Target.GetCommentsByCustomerRequest(customerRequestArgument);

            Assert.AreEqual(TestHydratedComments, actual);
        }
    }
}