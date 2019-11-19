using Android.App;
using Android.Support.V7.App;
using BR.Com.Gertec.Gedi.Exceptions;
using Ger7Tef;
using Plugin.CurrentActivity;
using System;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinPOS.Droid.Gertec;

[assembly: Dependency(typeof(Gertec))]
namespace XamarinPOS.Droid.Gertec
{
    public class Gertec : IGertec
    {
        HelperPrinterGertec _printer;
        ConfigPrinterGertec _configPrint;
        Ger7 _tef;
        Operacao _tefOperacao;

        const string _pontilhado = "---------------------------";

        public Gertec()
        {
            var currentContext = Android.App.Application.Context;
            _printer = new HelperPrinterGertec(currentContext);
            _configPrint = new ConfigPrinterGertec();
            _printer.setConfigImpressao(_configPrint);

            _tef = new Ger7();            
        }

        public string Pagamento(int id, decimal valor, PagamentoOperacao operacao, byte parcelas = 0)
        {                 
            _tefOperacao = new Operacao();            
            _tefOperacao.type = "1";
            _tefOperacao.id = id.ToString().PadLeft(6, '0');
            _tefOperacao.amount = valor.ToString("N").Replace(".", "").Replace(",", "");
            _tefOperacao.installments = parcelas.ToString();
            _tefOperacao.receipt = "1"; // Imprimir pela API
            if (_tefOperacao.installments == "0" || _tefOperacao.installments == "1")
            {
                _tefOperacao.instmode = "0"; // SEMP_ARCELAMENTO
            }           
            else
            {
                _tefOperacao.instmode = "2"; //PARCELADO_ADM
            }

            // Valida tipo de operação
            if (operacao == PagamentoOperacao.Credito)
            {
                _tefOperacao.product = "1";
            }
            else if (operacao == PagamentoOperacao.Debito)
            {
                _tefOperacao.product = "2";
            }
            else if (operacao == PagamentoOperacao.Voucher)
            {
                _tefOperacao.product = "4";
            }                                   

            try
            {
                //AppCompatActivity activity = Forms.Context as AppCompatActivity;
                Xamarin.Forms.Platform.Android.FormsAppCompatActivity activity = Forms.Context as Xamarin.Forms.Platform.Android.FormsAppCompatActivity;
                //Xamarin.Forms.Platform.Android.FormsAppCompatActivity activity = CrossCurrentActivity.Current.AppContext as Xamarin.Forms.Platform.Android.FormsAppCompatActivity;
                //activity.OnAc += OnActivityReenter;
                //var currentContext = Android.App.Application.ActivityService;
                //var intent = new Android.Content.Intent(Forms.Context, typeof(Android.App.Activity));
                //var activity =  Forms.Context.StartActivity(intent);
                //var activity = (Android.Support.V7.App.AppCompatActivity)Forms.Context;
                //CrossCurrentActivity.Current.Activity
                _tef.Ger7Execult(activity, _tefOperacao);
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private void Current_ActivityStateChanged(object sender, ActivityEventArgs e)
        {
            var a = e;
        }

        public string Cancelar(int id)
        {            
            _tefOperacao = new Operacao();            
            _tefOperacao.type = "2";            
            _tefOperacao.id = id.ToString().PadLeft(6, '0');

            try
            {
                // _tef.Ger7Execult(this, _tefOperacao);
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string MFe()
        {            
            try
            {                                
                _configPrint.Tamanho = 21;
                _configPrint.Alinhamento = "CENTER";
                _configPrint.Negrito = true;
                _printer.setConfigImpressao(_configPrint);
                _printer.ImprimeTexto("NOME FANTASIA");


                _configPrint.Tamanho = 15;
                _configPrint.Alinhamento = "LEFT";
                _printer.setConfigImpressao(_configPrint);
                _printer.ImprimeTexto("CNPJ 00.000.000/0000-00 I.E. 000.000.000.00");
                _printer.ImprimeTexto("Rua Afonso Arinos, 1277 - Centro - Fone(00) 1234-456789");

                _configPrint.Tamanho = 21;
                _configPrint.Negrito = true;
                _printer.setConfigImpressao(_configPrint);
                _printer.sPrintLine(_pontilhado);

                _configPrint.Tamanho = 17;
                _configPrint.Alinhamento = "CENTER";
                _configPrint.Negrito = false;
                _printer.setConfigImpressao(_configPrint);
                _printer.ImprimeTexto("Extrato 0004243");

                _configPrint.Tamanho = 21;
                _configPrint.Alinhamento = "CENTER";
                _configPrint.Negrito = true;
                _printer.setConfigImpressao(_configPrint);
                _printer.ImprimeTexto("CUPOM FISCAL ELETRONICO - SAT");
                _printer.sPrintLine(_pontilhado);

                _configPrint.Tamanho = 17;
                _configPrint.Alinhamento = "LEFT";
                _configPrint.Negrito = false;
                _printer.setConfigImpressao(_configPrint);
                _printer.ImprimeTexto("CPF/CNPJ do Consumidor: 222.222.222-99");

                _configPrint.Tamanho = 17;
                _configPrint.Alinhamento = "CENTER";
                _configPrint.Negrito = false;
                _printer.setConfigImpressao(_configPrint);
                _printer.ImprimeTexto("Detalhe Venda");

                _configPrint.Negrito = true;
                _configPrint.Tamanho = 23;
                _printer.setConfigImpressao(_configPrint);
                _printer.sPrintLine(_pontilhado);

                _configPrint.Tamanho = 17;
                _configPrint.Alinhamento = "LEFT";
                _configPrint.Negrito = false;
                _printer.setConfigImpressao(_configPrint);
                _printer.ImprimeTexto("# COD  DESC     QTD  UN   VL UNT R$  VL TOT R$");

                _configPrint.Negrito = true;
                _configPrint.Tamanho = 23;
                _printer.setConfigImpressao(_configPrint);
                _printer.sPrintLine(_pontilhado);

                _configPrint.Tamanho = 17;
                _configPrint.Alinhamento = "LEFT";
                _configPrint.Negrito = false;
                _printer.setConfigImpressao(_configPrint);
                _printer.ImprimeTexto("001 07894125 BOB. TERM. 8040X ");
                _printer.ImprimeTexto("              1UN X 12,90" + "»" + "12,90");

                _configPrint.Negrito = true;
                _configPrint.Tamanho = 23;
                _printer.setConfigImpressao(_configPrint);
                _printer.sPrintLine(_pontilhado);

                _configPrint.Tamanho = 17;
                _configPrint.Alinhamento = "LEFT";
                _configPrint.Negrito = false;
                _printer.setConfigImpressao(_configPrint);
                _printer.ImprimeTexto("Total bruto de itens" + "»" + "12,90");
                _printer.ImprimeTexto("DESCONTO" + "»" + "---");
                _printer.ImprimeTexto("OUTRAS DESPESAS" + "»" + "---");

                _configPrint.Tamanho = 20;
                _configPrint.Alinhamento = "LEFT";
                _configPrint.Negrito = true;
                _printer.setConfigImpressao(_configPrint);
                _printer.ImprimeTexto("TOTAL R$" + "»" + "12,20");

                _configPrint.Tamanho = 23;
                _printer.setConfigImpressao(_configPrint);
                _printer.sPrintLine(_pontilhado);

                _configPrint.Tamanho = 17;
                _configPrint.Alinhamento = "LEFT";
                _configPrint.Negrito = false;
                _printer.setConfigImpressao(_configPrint);
                _printer.ImprimeTexto("FORMA DE PAGAMENTO" + "»" + "VALOR PAGO");
                _printer.ImprimeTexto("Dinheiro" + "»" + "15,00");
                _configPrint.Tamanho = 20;
                _configPrint.Alinhamento = "LEFT";
                _configPrint.Negrito = true;
                _printer.setConfigImpressao(_configPrint);
                _printer.ImprimeTexto("TROCO R$" + "»" + "2,10");

                _configPrint.Negrito = true;
                _configPrint.Tamanho = 23;
                _printer.setConfigImpressao(_configPrint);
                _printer.sPrintLine(_pontilhado);

                _configPrint.Tamanho = 17;
                _configPrint.Alinhamento = "LEFT";
                _configPrint.Negrito = false;
                _printer.setConfigImpressao(_configPrint);
                _printer.ImprimeTexto("OBSERVACOES DO CONTRIBUINTE");
                _printer.ImprimeTexto("Promocao de natal");
                _configPrint.Tamanho = 23;
                _printer.setConfigImpressao(_configPrint);
                _printer.AvancaLinha(1);

                _configPrint.Tamanho = 17;
                _configPrint.Alinhamento = "LEFT";
                _configPrint.Negrito = false;
                _printer.setConfigImpressao(_configPrint);
                _printer.ImprimeTexto("Informacoes dos Tributos Totais");

                _configPrint.Tamanho = 17;
                _configPrint.Alinhamento = "LEFT";
                _configPrint.Negrito = false;
                _printer.ImprimeTexto("Incidentes (Lei Federal 12.741/2012" + "»" +  "R$ 4,40");

                _configPrint.Negrito = true;
                _configPrint.Tamanho = 23;
                _printer.setConfigImpressao(_configPrint);
                _printer.sPrintLine(_pontilhado);

                _configPrint.Tamanho = 17;
                _configPrint.Alinhamento = "CENTER";
                _configPrint.Negrito = true;
                _printer.setConfigImpressao(_configPrint);
                _printer.ImprimeTexto("SAT No 230.024.129");

                _configPrint.Tamanho = 16;
                _configPrint.Alinhamento = "CENTER";
                _configPrint.Negrito = true;
                _printer.setConfigImpressao(_configPrint);
                _printer.ImprimeTexto("12/06/2019 - 17:49:35");

                _configPrint.Tamanho = 14;
                _configPrint.Alinhamento = "CENTER";
                _configPrint.Negrito = false;
                _printer.setConfigImpressao(_configPrint);
                _printer.ImprimeTexto("9999 9999 9999 9999 9999 9999 9999 9999 9999 9999 9999");

                _printer.AvancaLinha(1);

                _configPrint.Tamanho = 12;
                _configPrint.Negrito = false;
                _configPrint.Alinhamento = "CENTER";
                _printer.setConfigImpressao(_configPrint);
                _printer.ImprimeBarCode(
                            "9999999999999999999999999999999999999999999",
                            60,
                            380,
                            "CODE_128");

                //O código QR Code deverá representar as seguintes informações do CF - e - SAT:
                // Chave de Consulta do CF - e - SAT;
                // Data e hora de emissão do CF - e - SAT, no formato: AAAAMMDDHHMMSS;
                // Valor total do CF - e - SAT;
                // CPF ou CNPJ do adquirente(se existir) , sem pontuações;
                // Campo “assinaturaQRCODE” presente no leiaute do arquivo do CF - e - SAT

                _configPrint.Tamanho = 10;
                _configPrint.Negrito = false;
                _configPrint.Alinhamento = "CENTER";
                _printer.setConfigImpressao(_configPrint);
                _printer.ImprimeBarCode(
                            "9999999999999999999999999999999999999999999",
                            320,
                            320,
                            "QR_CODE");

                _printer.AvancaLinha(2);

                _printer.ImpressoraOutput();                
                return "";
            }
            catch (GediException e)
            {
                return e.Message;
            }
        }


        //protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        protected void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            //base.OnActivityResult(requestCode, resultCode, data);
            try
            {
                var retornoGer7 = _tef.TrataRetornoGer7(requestCode, resultCode, data);
                ValidRetorno(retornoGer7);
            }
            catch (GediException e)
            {
                //Toast.MakeText(ApplicationContext, e.Message, ToastLength.Long).Show();
            }
            catch (Exception e)
            {
                //Toast.MakeText(ApplicationContext, e.Message, ToastLength.Long).Show();
            }
        }

        private void ValidRetorno(RetornoGer7 retorno)
        {
            String[] cupom;
            String[] cupomEstabelecimento;
            String[] cupomCliente;

            StringBuilder builder = new StringBuilder();           
            if (retorno.response == "0" && retorno.errcode == "0")
            {

                //Transação Aprovada;
                builder.Append("Authorization: " + retorno.authorization);
                builder.Append("\nID: " + retorno.id);
                builder.Append("\nProduto: " + retorno.product);
                builder.Append("\nLabel: " + retorno.label);
                builder.Append("\nSTAN: " + retorno.stan);
                builder.Append("\nAID: " + retorno.aid);
                builder.Append("\nRRN: " + retorno.rrn);
                builder.Append("\nHorario: " + retorno.time);
                builder.Append("\nVersion: " + retorno.version);
                if (retorno.amount != null)
                {
                    builder.Append("\nValor: " + retorno.amount.Substring(0, retorno.amount.Length - 2).TrimStart('0') + "," + retorno.amount.Substring(retorno.amount.Length - 2));
                }
                //alert.SetMessage(builder.ToString());
                //alert.SetButton("Cancelar", (c, ev) => { });
                //alert.SetButton2("Imprimir", (c, ev) => {
                try
                {
                    _configPrint.Negrito = true;

                    _printer.setConfigImpressao(_configPrint);
                    cupom = retorno.print.Split('\f');
                    cupomEstabelecimento = cupom[0].Split("\n");
                    cupomCliente = cupom[1].Split("\n");

                    _printer.ImprimeTexto("************[ESTABELECIMENTO]************");
                    foreach (string linha in cupomEstabelecimento)
                    {
                        _printer.ImprimeTexto(linha);
                    }

                    _printer.AvancaLinha(20);

                    _printer.ImprimeTexto("****************[CLIENTE]****************");
                    foreach (string linha in cupomCliente)
                    {
                        _printer.ImprimeTexto(linha);
                    }

                    _printer.ImpressoraOutput();
                }
                catch (GediException e)
                {
                    // throw new GediException(e.ErrorCode);
                    //Toast.MakeText(ApplicationContext, e.Message, ToastLength.Long).Show();
                }
                catch (Exception e)
                {
                    // throw new Exception(e.Message);
                    //Toast.MakeText(ApplicationContext, e.Message, ToastLength.Long).Show();
                }
                finally
                {
                    _printer.ImpressoraOutput();
                }

                //});
            }
            else
            {
                //Transação Erro
                builder.Append("Código: " + retorno.errcode);
                builder.Append("\nDescrição: " + retorno.errmsg);
                //alert.SetMessage(builder.ToString());
                //alert.SetButton("OK", (c, ev) => { });
            }           
        }
    }

    
}