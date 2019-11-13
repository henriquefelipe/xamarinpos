using BR.Com.Gertec.Gedi.Exceptions;
using Xamarin.Forms;
using XamarinPOS.Droid.Gertec;

[assembly: Dependency(typeof(PrinterGertec))]
namespace XamarinPOS.Droid.Gertec
{
    public class PrinterGertec : IPrinterGerter
    {
        HelperPrinterGertec printer;
        ConfigPrinterGertec configPrint;

        const string _linha = "------------------------------------------";

        public PrinterGertec()
        {
            var currentContext = Android.App.Application.Context;
            printer = new HelperPrinterGertec(currentContext);
            configPrint = new ConfigPrinterGertec();
            printer.setConfigImpressao(configPrint);
        }

        public string MFe()
        {            
            try
            {                                
                configPrint.Tamanho = 23;
                configPrint.Alinhamento = "CENTER";
                configPrint.Negrito = true;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("NOME FANTASIA");
               

                configPrint.Tamanho = 15;
                configPrint.Alinhamento = "LEFT";
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("CNPJ 00.000.000/0000-00 I.E. 000.000.000.00");
                printer.ImprimeTexto("Rua Afonso Arinos, 1277 - Centro - Fone(00) 1234-456789");

                configPrint.IHeight = 10;
                printer.setConfigImpressao(configPrint);
                printer.sPrintLine(_linha);

                configPrint.Tamanho = 17;
                configPrint.Alinhamento = "CENTER";
                configPrint.Negrito = false;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("Extrato 0004243");

                configPrint.Tamanho = 23;
                configPrint.Alinhamento = "CENTER";
                configPrint.Negrito = true;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("CUPOM FISCAL ELETRONICO - SAT");
                printer.sPrintLine(_linha);

                configPrint.Tamanho = 17;
                configPrint.Alinhamento = "LEFT";
                configPrint.Negrito = false;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("CPF/CNPJ do Consumidor: 222.222.222-99");

                configPrint.Tamanho = 17;
                configPrint.Alinhamento = "CENTER";
                configPrint.Negrito = false;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("Detalhe Venda");

                configPrint.Negrito = true;
                configPrint.Tamanho = 23;
                printer.setConfigImpressao(configPrint);
                printer.sPrintLine(_linha);

                configPrint.Tamanho = 17;
                configPrint.Alinhamento = "LEFT";
                configPrint.Negrito = false;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("# COD  DESC      QTD  UN    VL UNT R$    VL TOT R$");

                configPrint.Negrito = true;
                configPrint.Tamanho = 23;
                printer.setConfigImpressao(configPrint);
                printer.sPrintLine(_linha);

                configPrint.Tamanho = 17;
                configPrint.Alinhamento = "LEFT";
                configPrint.Negrito = false;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("001 07894125 BOB. TERM. 8040X ");
                printer.ImprimeTexto("              1UN X 12,90" + "»" + "12,90");

                configPrint.Negrito = true;
                configPrint.Tamanho = 23;
                printer.setConfigImpressao(configPrint);
                printer.sPrintLine(_linha);

                configPrint.Tamanho = 17;
                configPrint.Alinhamento = "LEFT";
                configPrint.Negrito = false;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("Total bruto de itens" + "»" + "12,90");
                printer.ImprimeTexto("DESCONTO" + "»" + "---");
                printer.ImprimeTexto("OUTRAS DESPESAS" + "»" + "---");

                configPrint.Tamanho = 20;
                configPrint.Alinhamento = "LEFT";
                configPrint.Negrito = true;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("TOTAL R$" + "»" + "12,20");

                configPrint.Tamanho = 23;
                printer.setConfigImpressao(configPrint);
                printer.sPrintLine(_linha);

                configPrint.Tamanho = 17;
                configPrint.Alinhamento = "LEFT";
                configPrint.Negrito = false;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("FORMA DE PAGAMENTO" + "»" + "VALOR PAGO");
                printer.ImprimeTexto("Dinheiro" + "»" + "15,00");
                configPrint.Tamanho = 20;
                configPrint.Alinhamento = "LEFT";
                configPrint.Negrito = true;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("TROCO R$" + "»" + "2,10");

                configPrint.Negrito = true;
                configPrint.Tamanho = 23;
                printer.setConfigImpressao(configPrint);
                printer.sPrintLine(_linha);

                configPrint.Tamanho = 17;
                configPrint.Alinhamento = "LEFT";
                configPrint.Negrito = false;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("OBSERVACOES DO CONTRIBUINTE");
                printer.ImprimeTexto("Promocao de natal");
                configPrint.Tamanho = 23;
                printer.setConfigImpressao(configPrint);
                printer.AvancaLinha(1);

                configPrint.Tamanho = 17;
                configPrint.Alinhamento = "LEFT";
                configPrint.Negrito = false;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("Informacoes dos Tributos Totais");

                configPrint.Tamanho = 17;
                configPrint.Alinhamento = "LEFT";
                configPrint.Negrito = false;
                printer.ImprimeTexto("Incidentes (Lei Federal 12.741/2012" + "»" +  "R$ 4,40");

                configPrint.Negrito = true;
                configPrint.Tamanho = 23;
                printer.setConfigImpressao(configPrint);
                printer.sPrintLine(_linha);

                configPrint.Tamanho = 17;
                configPrint.Alinhamento = "CENTER";
                configPrint.Negrito = true;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("SAT No 230.024.129");

                configPrint.Tamanho = 16;
                configPrint.Alinhamento = "CENTER";
                configPrint.Negrito = true;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("12/06/2019 - 17:49:35");

                configPrint.Tamanho = 14;
                configPrint.Alinhamento = "CENTER";
                configPrint.Negrito = false;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("9999 9999 9999 9999 9999 9999 9999 9999 9999 9999 9999");

                printer.AvancaLinha(1);

                configPrint.Tamanho = 12;
                configPrint.Negrito = false;
                configPrint.Alinhamento = "CENTER";
                printer.setConfigImpressao(configPrint);
                printer.ImprimeBarCode(
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

                configPrint.Tamanho = 10;
                configPrint.Negrito = false;
                configPrint.Alinhamento = "CENTER";
                printer.setConfigImpressao(configPrint);
                printer.ImprimeBarCode(
                            "9999999999999999999999999999999999999999999",
                            320,
                            320,
                            "QR_CODE");

                printer.AvancaLinha(2);                

                printer.ImpressoraOutput();                
                return "";
            }
            catch (GediException e)
            {
                return e.Message;
            }
        }

       
    }
}