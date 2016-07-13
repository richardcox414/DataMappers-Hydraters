using System;
using EnterpriseSystems.Data.DAO;
using EnterpriseSystems.Data.Hydraters;
using EnterpriseSystems.Data.Model.Constants;
using EnterpriseSystems.Data.Model.Entities;
using System.Collections.Generic;

namespace EnterpriseSystems.Data.Mappers
{
    public class AppointmentMapper
    {
        private IDatabase Database { get; set; }
        private IHydrater<AppointmentVO> Hydrater { get; set; }
        private CustomerRequestMapper CustomerRequestMapper { get; set; }
        private CommentMapper CommentMapper { get; set; }

        public AppointmentMapper(IDatabase database, IHydrater<AppointmentVO> hydrater, CustomerRequestMapper customerRequestMapper, CommentMapper commentMapper)
        {
            Database = database;
            Hydrater = hydrater;
            CustomerRequestMapper = customerRequestMapper;
            CommentMapper = commentMapper;
        }
        public virtual IEnumerable<AppointmentVO> GetAppointmentsByCustomerRequest(CustomerRequestVO customerRequest)
        {
            const string selectQueryStatement = "SELECT * FROM REQ_ETY_SCH WHERE ETY_NM = 'CUS_REQ' AND ETY_KEY_I = @CUS_REQ_I";

            
            var query = Database.CreateQuery(selectQueryStatement);
            query.AddParameter(customerRequest.Appointments, CustomerRequestQueryParameters.Identity);
            var result = Database.RunSelect(query);
            var entities = Hydrater.Hydrate(result);
            //FillRelatedEntities(entities);

            return entities;
        }
        //private void FillRelatedEntities(IEnumerable<CustomerRequestVO> customerRequests)
        //{
        //    foreach (var customerRequest in customerRequests)
        //    {
        //        FillRelatedEntities(customerRequest);
        //    }
        //}
        //private void FillRelatedEntities(CustomerRequestVO customerRequest)
        //{
        //    if (customerRequest != null)
        //    {
        //        customerRequest.Appointments = AppointmentMapper.GetAppointmentsByCustomerRequest(customerRequest).ToList();
        //        customerRequest.Comments = CommentMapper.GetCommentsByCustomerRequest(customerRequest).ToList();
        //    }
        //}
    }
}
