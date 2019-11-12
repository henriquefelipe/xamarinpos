using Android.App;
using Android.Content;
using Android.Graphics;
using BR.Com.Gertec.Gedi;
using BR.Com.Gertec.Gedi.Enums;
using BR.Com.Gertec.Gedi.Exceptions;
using BR.Com.Gertec.Gedi.Interfaces;
using BR.Com.Gertec.Gedi.Structs;
using Plugin.DeviceInfo;
using System;
using System.Threading;
using ZXing;
using ZXing.Common;

namespace XamarinPOS.Droid
{
    public class GertecPrinter : IGertecPrinter
    {
        // Defines
        private const string IMPRESSORA_ERRO = "Impressora com erro.";

        private string modelo = CrossDeviceInfo.Current.Model;

        // Statics
        private static bool isPrintInit = false;

        private IGEDI iGedi;
        private IPRNTR iPrintr;
        private GEDI_PRNTR_st_StringConfig stringConfig;
        private GEDI_PRNTR_st_PictureConfig pictureConfig;
        private GEDI_PRNTR_e_Status status;

        private ConfigPrint configPrint;
        private Typeface typeface;

        // Thread starGedi;

        private Activity mainActivity;
        private Context mainContext;


        public GertecPrinter(Activity act, Context ctx)
        {
            this.mainActivity = act;
            this.mainContext = ctx;
            startGedi();
        }

        public void startGedi()
        {
            new Thread(new ThreadStart(() =>
            {
                this.iGedi = GEDI.GetInstance(this.mainContext);
                this.iPrintr = iGedi.PRNTR;
                ImpressoraInit();
            })).Start();
        }

        private void ImpressoraInit()
        {
            try
            {
                if (this.iPrintr != null && !isPrintInit)
                {
                    this.iPrintr.Init();
                    isPrintInit = true;
                    getStatusImpressora();
                }
            }
            catch (GediException e)
            {
                throw new GediException(e.ErrorCode);
            }
        }

        public void ImpressoraOutput()
        {
            try
            {
                if (this.iPrintr != null && isPrintInit)
                {
                    this.iPrintr.Output();
                    isPrintInit = false;
                }
            }
            catch (GediException e)
            {
                throw new GediException(e.ErrorCode);
            }
        }

        public void AvancaLinha(int linhas)
        {
            try
            {
                this.iPrintr.DrawBlankLine(linhas);
            }
            catch (GediException e)
            {
                throw new GediException(e.ErrorCode);
            }
        }

        public string getStatusImpressora()
        {
            try
            {
                this.status = this.iPrintr.Status();
            }
            catch (GediException e)
            {
                throw new GediException(e.ErrorCode);
            }
            return traduzStatusImpressora(this.status);
        }

        public bool ImprimeBarCode(string texto, int height, int width, string barcodeFormat)
        {
            try
            {

                BarcodeWriter writer = new BarcodeWriter();

                if (barcodeFormat.Equals("CODE_128"))
                {
                    writer.Format = BarcodeFormat.CODE_128;
                }
                else if (barcodeFormat.Equals("EAN_8"))
                {
                    writer.Format = BarcodeFormat.EAN_8;
                }
                else if (barcodeFormat.Equals("EAN_13"))
                {
                    writer.Format = BarcodeFormat.EAN_13;
                }
                else if (barcodeFormat.Equals("PDF_417"))
                {
                    writer.Format = BarcodeFormat.PDF_417;
                }
                else if (barcodeFormat.Equals("QR_CODE"))
                {
                    writer.Format = BarcodeFormat.QR_CODE;
                }

                writer.Options = new EncodingOptions()
                {
                    Width = width,
                    Height = height,
                    Margin = 2
                };

                var bitmap = writer.Write(texto);

                this.pictureConfig = new GEDI_PRNTR_st_PictureConfig();
                this.pictureConfig.Alignment = GEDI_PRNTR_e_Alignment.ValueOf(this.configPrint.Alinhamento);

                this.pictureConfig.Height = bitmap.Height;
                this.pictureConfig.Width = bitmap.Width;

                this.ImpressoraInit();
                this.iPrintr.DrawPictureExt(pictureConfig, bitmap);
                AvancaLinha(100);

                return true;
            }
            catch (GediException e)
            {
                throw new GediException(e.ErrorCode);
            }
        }

        public bool ImprimeImagem(string imagem)
        {
            int id;
            Bitmap bitmap;
            try
            {
                this.pictureConfig = new GEDI_PRNTR_st_PictureConfig();
                this.pictureConfig.Alignment = GEDI_PRNTR_e_Alignment.ValueOf(this.configPrint.Alinhamento);

                id = this.mainContext.Resources.GetIdentifier(imagem, "drawable", this.mainContext.PackageName);
                bitmap = BitmapFactory.DecodeResource(this.mainContext.Resources, id);
                if (modelo.Equals("Smart G800"))
                {
                    this.pictureConfig.Height = bitmap.Height;
                    this.pictureConfig.Width = bitmap.Width;
                }
                else
                {
                    this.pictureConfig.Height = configPrint.IHeight;
                    this.pictureConfig.Width = configPrint.IWidth;
                }
                ImpressoraInit();
                this.iPrintr.DrawPictureExt(pictureConfig, bitmap);
                AvancaLinha(100);


            }
            catch (GediException e)
            {
                throw new GediException(e.ErrorCode);
            }

            return true;
        }

        public void ImprimeTexto(string texto)
        {
            try
            {
                ImpressoraInit();

                if (!this.IsImpressoraOK())
                {
                    throw new Exception(IMPRESSORA_ERRO);
                }
                sPrintLine(texto);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void ImprimeTexto(string texto, int tamanho)
        {
            int tamanhoOld;
            try
            {
                this.getStatusImpressora();
                if (!this.IsImpressoraOK())
                {
                    throw new Exception(IMPRESSORA_ERRO);
                }
                tamanhoOld = this.configPrint.Tamanho;
                this.configPrint.Tamanho = tamanho;
                sPrintLine(texto);
                this.configPrint.Tamanho = tamanhoOld;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void ImprimeTexto(string texto, bool negrito)
        {
            bool negritoOld;
            try
            {
                this.getStatusImpressora();
                if (!this.IsImpressoraOK())
                {
                    throw new Exception(IMPRESSORA_ERRO);
                }
                negritoOld = this.configPrint.Negrito;
                this.configPrint.Negrito = negrito;
                sPrintLine(texto);
                this.configPrint.Negrito = negritoOld;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void ImprimeTexto(string texto, bool negrito, bool italico)
        {
            bool negritoOld;
            bool italicoOld;
            try
            {
                this.getStatusImpressora();
                if (!this.IsImpressoraOK())
                {
                    throw new Exception(IMPRESSORA_ERRO);
                }
                negritoOld = this.configPrint.Negrito;
                italicoOld = this.configPrint.Italico;
                this.configPrint.Negrito = negrito;
                sPrintLine(texto);
                this.configPrint.Negrito = negritoOld;
                this.configPrint.Italico = italicoOld;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void ImprimeTexto(string texto, bool negrito, bool italico, bool sublinhado)
        {
            bool negritoOld;
            bool italicoOld;
            bool sublinhadoOld;
            try
            {
                this.getStatusImpressora();
                if (!this.IsImpressoraOK())
                {
                    throw new Exception(IMPRESSORA_ERRO);
                }
                negritoOld = this.configPrint.Negrito;
                italicoOld = this.configPrint.Italico;
                sublinhadoOld = this.configPrint.SubLinhado;
                this.configPrint.Negrito = negrito;
                sPrintLine(texto);
                this.configPrint.Negrito = negritoOld;
                this.configPrint.Italico = italicoOld;
                this.configPrint.SubLinhado = sublinhadoOld;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool IsImpressoraOK()
        {
            if (this.status.Value == 0)
            {
                return true;
            }
            return false;
        }

        public void setConfigImpressao(ConfigPrint config)
        {
            this.configPrint = config;

            this.stringConfig = new GEDI_PRNTR_st_StringConfig(new Paint());
            this.stringConfig.Paint.TextSize = configPrint.Tamanho;
            this.stringConfig.Paint.TextAlign = Paint.Align.ValueOf(configPrint.Alinhamento);
            this.stringConfig.Offset = configPrint.OffSet;
            this.stringConfig.LineSpace = configPrint.LineSpace;

            switch (configPrint.Fonte)
            {
                case "NORMAL":
                    this.typeface = Typeface.Create(configPrint.Fonte, TypefaceStyle.Bold);
                    break;

                case "DEFAULT":
                    this.typeface = Typeface.Create(Typeface.Default, TypefaceStyle.Normal);
                    break;

                case "DEFAULT BOLD":
                    this.typeface = Typeface.Create(Typeface.DefaultBold, TypefaceStyle.Normal);
                    break;

                case "MONOSPACE":
                    this.typeface = Typeface.Create(Typeface.Monospace, TypefaceStyle.Normal);
                    break;

                case "SANS SERIF":
                    this.typeface = Typeface.Create(Typeface.SansSerif, TypefaceStyle.Normal);
                    break;

                case "SERIF":
                    this.typeface = Typeface.Create(Typeface.Serif, TypefaceStyle.Normal);
                    break;

                default:
                    this.typeface = Typeface.CreateFromAsset(this.mainContext.Assets, configPrint.Fonte);
                    break;
            }

            if (this.configPrint.Negrito && this.configPrint.Italico)
            {
                this.typeface = Typeface.Create(typeface, TypefaceStyle.BoldItalic);
            }
            else if (this.configPrint.Negrito)
            {
                this.typeface = Typeface.Create(typeface, TypefaceStyle.Bold);
            }
            else if (this.configPrint.Italico)
            {
                this.typeface = Typeface.Create(typeface, TypefaceStyle.Italic);
            }

            if (this.configPrint.SubLinhado)
            {
                this.stringConfig.Paint.Flags = PaintFlags.UnderlineText;
            }

            this.stringConfig.Paint.SetTypeface(typeface);

        }

        public bool sPrintLine(string texto)
        {
            try
            {
                ImpressoraInit();
                this.iPrintr.DrawStringExt(this.stringConfig, texto);
                this.AvancaLinha(configPrint.AvancaLinha);
            }
            catch (GediException e)
            {
                throw new GediException(e.ErrorCode);
            }

            return true;

        }

        private string traduzStatusImpressora(GEDI_PRNTR_e_Status status)
        {

            string retorno;

            if (status == GEDI_PRNTR_e_Status.Ok)
            {
                retorno = "IMPRESSORA OK";
            }
            else if (status == GEDI_PRNTR_e_Status.OutOfPaper)
            {
                retorno = "SEM PAPEL";
            }
            else if (status == GEDI_PRNTR_e_Status.Overheat)
            {
                retorno = "SUPER AQUECIMENTO";
            }
            else
            {
                retorno = "ERRO DESCONHECIDO";
            }

            return retorno;

        }

    }

}