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
        public IPrinterGerter _printerGertec { get; }
        public MainPage()
        {
            InitializeComponent();
            _printerGertec = DependencyService.Get<IPrinterGerter>();
        }

        public void Button_OnClicked(object sender, EventArgs e)
        {
            _printerGertec.MFe();
        }
    }
}
