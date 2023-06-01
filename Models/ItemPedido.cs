using Gamificacao3;

namespace Gamificacao3
{
    public class ItemPedido
    {
        private int _id;
        private Produto? _produto;
        private int _quantidade;
        private decimal _precoUnitario;
        private Pedido? _pedido;

        public int Id{
            get { return _id; }
            set { _id = value; }
        }

        public Produto? Produto{
            get { return _produto; }
            set { _produto = value; }
        }

        public int Quantidade{
            get { return _quantidade; }
            set { _quantidade = value;}
        }
        public decimal PrecoUnitario{
            get { return _precoUnitario; }
            set { _precoUnitario = value;}
        }

        public Pedido? Pedido{
            get { return _pedido; }
            set { _pedido = value; }
        }

        public ItemPedido(int id, Produto? produto, int quantidade, decimal precoUnitario, Pedido? pedido)
        {
            _id = id;
            _produto = produto;
            _quantidade = quantidade;
            _precoUnitario = precoUnitario;
            _pedido = pedido;
        }
    }
}