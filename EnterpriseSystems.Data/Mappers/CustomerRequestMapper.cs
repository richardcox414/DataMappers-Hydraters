using EnterpriseSystems.Data.DAO;
using EnterpriseSystems.Data.Hydraters;
using EnterpriseSystems.Data.Model.Constants;
using EnterpriseSystems.Data.Model.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EnterpriseSystems.Data.Mappers
{
    public class CustomerRequestMapper
    {
        private IDatabase Database { get; set; }
        private IHydrater<CustomerRequestVO> Hydrater { get; set; }
        private AppointmentMapper AppointmentMapper { get; set; }
        private CommentMapper CommentMapper { get; set; }

        public CustomerRequestMapper(IDatabase database, IHydrater<CustomerRequestVO> hydrater, AppointmentMapper appointmentMapper, CommentMapper commentMapper)
        {
            Database = database;
            Hydrater = hydrater;
            AppointmentMapper = appointmentMapper;
            CommentMapper = commentMapper;
        }

        public CustomerRequestVO GetCustomerRequestByIdentity(CustomerRequestVO customerRequest)
        {
            const string selectQueryStatement = "SELECT * FROM CUS_REQ WHERE CUS_REQ_I = @CUS_REQ_I";

            var query = Database.CreateQuery(selectQueryStatement);
            query.AddParameter(customerRequest.Identity, CustomerRequestQueryParameters.Identity);
            var result = Database.RunSelect(query);
            var entity = Hydrater.Hydrate(result).FirstOrDefault();
            FillRelatedEntities(entity);

            return entity;
        }

        public IEnumerable<CustomerRequestVO> GetCustomerRequestsByReferenceNumberAndBusinessName(CustomerRequestVO customerRequest)
        {
            const string selectQueryStatement = "SELECT A.* FROM CUS_REQ A, REQ_ETY_REF_NBR B WHERE "
                        + "A.BUS_UNT_KEY_CH = @BUS_UNT_KEY_CH AND B.ETY_NM = 'CUS_REQ' "
                        + "AND B.ETY_KEY_I = A.CUS_REQ_I AND B.REF_NBR = @REF_NBR";

            var referenceNumber = customerRequest.ReferenceNumbers.First();
            var query = Database.CreateQuery(selectQueryStatement);
            query.AddParameter(customerRequest.BusinessEntityKey, CustomerRequestQueryParameters.BusinessName);
            query.AddParameter(referenceNumber.ReferenceNumber, ReferenceNumberQueryParameters.ReferenceNumber);
            var result = Database.RunSelect(query);
            var entities = Hydrater.Hydrate(result);
            FillRelatedEntities(entities);

            return entities;
        }

        private void FillRelatedEntities(IEnumerable<CustomerRequestVO> customerRequests)
        {
            foreach (var customerRequest in customerRequests)
            {
                FillRelatedEntities(customerRequest);
            }
        }

        private void FillRelatedEntities(CustomerRequestVO customerRequest)
        {
            if (customerRequest != null)
            {
                customerRequest.Appointments = AppointmentMapper.GetAppointmentsByCustomerRequest(customerRequest).ToList();
                customerRequest.Comments = CommentMapper.GetCommentsByCustomerRequest(customerRequest).ToList();
            }
        }
    }
}
