using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinPOS
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public IGertec _gertec { get; }
        public MainPage()
        {
            InitializeComponent();
            _gertec = DependencyService.Get<IGertec>();
        }

        public void Button_OnClicked(object sender, EventArgs e)
        {
            var valor = Utils.GeraValor();
            Random random = new Random();
            var id = random.Next(0, 10000);
            new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                _gertec.Pagamento(id, (decimal)valor, PagamentoOperacao.Credito, 1);
            })).Start();
           
        }
    }
}
