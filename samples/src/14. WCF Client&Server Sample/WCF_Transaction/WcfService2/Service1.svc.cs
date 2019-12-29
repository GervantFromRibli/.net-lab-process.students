﻿using System;
using System.Data.SqlClient;
using System.ServiceModel;

namespace WcfService2
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in Web.config and in the associated .svc file.
    public class Service1 : IService1
    {
        private string strConnection = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=WCF_TRAN;Integrated Security=True";

        [OperationBehavior(TransactionScopeRequired = true)]
        public void UpdateData()
        {
            using (var objConnection = new SqlConnection(strConnection))
            {
                objConnection.Open();
                var objCommand = new SqlCommand("insert into DummyValues values('value1','value2')", objConnection);
                objCommand.ExecuteNonQuery();
            }
        }
        
        public string GetData(int value)
        {
            return $"You entered: {value}";
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
