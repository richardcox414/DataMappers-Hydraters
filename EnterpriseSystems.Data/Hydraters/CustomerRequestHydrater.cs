using EnterpriseSystems.Data.Model.Constants;
using EnterpriseSystems.Data.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace EnterpriseSystems.Data.Hydraters
{
    public class CustomerRequestHydrater : IHydrater<CustomerRequestVO>
    {
        public IEnumerable<CustomerRequestVO> Hydrate(DataTable dataTable)
        {
            var customerRequests = new List<CustomerRequestVO>();

            if (dataTable != null)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    var customerRequest = HydrateEntity(dataRow);
                    customerRequests.Add(customerRequest);
                }
            }

            return customerRequests;
        }

        private CustomerRequestVO HydrateEntity(DataRow dataRow)
        {
            var customerRequest = new CustomerRequestVO
            {
                Identity = (int)dataRow[CustomerRequestColumnNames.SequenceNumber],
                Status = dataRow[CustomerRequestColumnNames.Status].ToString(),
                BusinessEntityKey = dataRow[CustomerRequestColumnNames.BusinessEntityName].ToString(),
                TypeCode = dataRow[CustomerRequestColumnNames.TypeCode].ToString(),
                ConsumerClassificationType = dataRow[CustomerRequestColumnNames.ConsumerClassificationType].ToString(),
                CreatedDate = (DateTime?)dataRow[CustomerRequestColumnNames.CreatedDate],
                CreatedUserId = dataRow[CustomerRequestColumnNames.CreatedUserId].ToString(),
                CreatedProgramCode = dataRow[CustomerRequestColumnNames.CreatedProgramCode].ToString(),
                LastUpdatedDate = (DateTime?)dataRow[CustomerRequestColumnNames.LastUpdatedDate],
                LastUpdatedUserId = dataRow[CustomerRequestColumnNames.LastUpdatedUserId].ToString(),
                LastUpdatedProgramCode = dataRow[CustomerRequestColumnNames.LastUpdatedProgramCode].ToString()
            };

            return customerRequest;
        }
    }
}
