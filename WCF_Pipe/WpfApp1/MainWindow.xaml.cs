using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WCF_Pipe;
using static WCF_Pipe.Common;

namespace WpfApp1
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window, IService, ICallback
    {
        private ServiceHost _ServiceHost;

        public MainWindow()
        {
            InitializeComponent();
        }
        protected override void OnContentRendered(EventArgs e)
        {
            txtLog.Text = txtSend.Text = "";
            OpenHost();

            base.OnContentRendered(e);
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            var host = DuplexChannelFactory<IService>.CreateChannel(this, binding, new EndpointAddress(Addr2));
            host.Send(txtSend.Text);
        }

        public void OpenHost()
        {
            var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            _ServiceHost = new ServiceHost(this);
            _ServiceHost.AddServiceEndpoint(typeof(IService), binding, Addr1);
            _ServiceHost.Open();
        }

        public void CloseHost()
        {
            _ServiceHost.Close();
        }

        public void Send(string data)
        {
            txtLog.Text += $"データを受信した => {data}\n";

            var context = OperationContext.Current;
            var callback = context.GetCallbackChannel<ICallback>();

            callback.Callback(string.Format("OK", data));
        }

        public void Callback(string data)
        {
            txtLog.Text += $"応答を受信した => {data}\n";
        }
    }
}
