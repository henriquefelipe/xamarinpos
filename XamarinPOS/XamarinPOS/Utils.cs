using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinPOS
{
    public class Utils
    {
        public static double GeraValor()
        {
            Random numAleatorio = new Random();
            return Math.Round(numAleatorio.NextDouble() * 10, 2);
        }
    }
}
