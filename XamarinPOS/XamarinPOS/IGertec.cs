using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XamarinPOS
{
    public interface IGertec
    {
        string Pagamento(int id, decimal valor, PagamentoOperacao operacao, byte parcelas = 0);
        string MFe();
    }
}
