using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCF_Pipe
{
    [ServiceContract]
    public interface ICallback
    {
        [OperationContract(IsOneWay = true)]
        void Callback(string data);
    }
}
