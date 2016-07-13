using System;
using EnterpriseSystems.Data.DAO;
using EnterpriseSystems.Data.Hydraters;
using EnterpriseSystems.Data.Model.Constants;
using EnterpriseSystems.Data.Model.Entities;
using System.Collections.Generic;

namespace EnterpriseSystems.Data.Mappers
{
    public class CommentMapper
    {
        private IDatabase Database { get; set; }
        private IHydrater<CommentVO> Hydrater { get; set; }
        private CustomerRequestMapper CustomerRequestMapper { get; set; }
        private AppointmentMapper AppointmentMapper { get; set; }

        public CommentMapper(IDatabase database, IHydrater<CommentVO> hydrater, CustomerRequestMapper customerRequestMapper, AppointmentMapper appointmentMapper)
        {
            Database = database;
            Hydrater = hydrater;
            CustomerRequestMapper = customerRequestMapper;
            AppointmentMapper = appointmentMapper;
        }
        public virtual IEnumerable<CommentVO> GetCommentsByCustomerRequest(CustomerRequestVO customerRequest)
        {
            const string selectQueryStatement = "SELECT * FROM REQ_ETY_CMM WHERE ETY_NM = 'CUS_REQ' AND ETY_KEY_I = @CUS_REQ_I";


            var query = Database.CreateQuery(selectQueryStatement);
            query.AddParameter(customerRequest.Comments, CustomerRequestQueryParameters.Identity);
            var result = Database.RunSelect(query);
            var entities = Hydrater.Hydrate(result);

            return entities;
        }
        
    }
}
