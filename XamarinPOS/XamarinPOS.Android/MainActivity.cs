using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Ger7Tef;
using BR.Com.Gertec.Gedi.Exceptions;
using Android.Content;
using Plugin.CurrentActivity;
using System.Text;

namespace XamarinPOS.Droid
{
    [Activity(Label = "XamarinPOS", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            global::ZXing.Net.Mobile.Forms.Android.Platform.Init();
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            //global::ZXing.Net.Mobile.Forms.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            try
            {
                var ger7 = new Ger7();
                var retornoGer7 = ger7.TrataRetornoGer7(requestCode, resultCode, data);
                Ger7ValidRetorno(retornoGer7);
            }
            catch (GediException e)
            {
                Toast.MakeText(ApplicationContext, e.Message, ToastLength.Long).Show();
            }
            catch (Exception e)
            {
                Toast.MakeText(ApplicationContext, e.Message, ToastLength.Long).Show();
            }

            //CrossCurrentActivity.Current.Activity = 
        }

        private void Ger7ValidRetorno(RetornoGer7 retorno)
        {

            String[] cupom;
            //String[] cupomEstabelecimento;
            //String[] cupomCliente;

            StringBuilder builder = new StringBuilder();
            //Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
            //AlertDialog alert = dialog.Create();
            if (retorno.response == "0" && retorno.errcode == "0")
            {

                //    alert.SetTitle("Transação Aprovada");
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
                //    alert.SetMessage(builder.ToString());
                //    alert.SetButton("Cancelar", (c, ev) => { });
                //    alert.SetButton2("Imprimir", (c, ev) => {
                //        try
                //        {
                //            configPrint = new ConfigPrint();
                //            configPrint.Negrito = true;

                //            if (printer == null)
                //            {
                //                printer = new GertecPrinter(this.iGedi, this.iPrint);
                //            }

                //            printer.setConfigImpressao(configPrint);
                //            cupom = retorno.print.Split('\f');
                //            cupomEstabelecimento = cupom[0].Split("\n");
                //            cupomCliente = cupom[1].Split("\n");

                //            printer.ImprimeTexto("************[ESTABELECIMENTO]************");
                //            foreach (string linha in cupomEstabelecimento)
                //            {
                //                printer.ImprimeTexto(linha);
                //            }

                //            printer.AvancaLinha(20);

                //            printer.ImprimeTexto("****************[CLIENTE]****************");
                //            foreach (string linha in cupomCliente)
                //            {
                //                printer.ImprimeTexto(linha);
                //            }

                //            printer.ImpressoraOutput();
                //        }
                //        catch (GediException e)
                //        {
                //            // throw new GediException(e.ErrorCode);
                //            Toast.MakeText(ApplicationContext, e.Message, ToastLength.Long).Show();
                //        }
                //        catch (Exception e)
                //        {
                //            // throw new Exception(e.Message);
                //            Toast.MakeText(ApplicationContext, e.Message, ToastLength.Long).Show();
                //        }
                //        finally
                //        {
                //            printer.ImpressoraOutput();
                //        }

                //    });
                }
                else
                {
                //    alert.SetTitle("Transação Erro");
                    builder.Append("Código: " + retorno.errcode);
                    builder.Append("\nDescrição: " + retorno.errmsg);
                //    alert.SetMessage(builder.ToString());
                //    alert.SetButton("OK", (c, ev) => { });
                }

                //alert.Show();
            }
    }
}