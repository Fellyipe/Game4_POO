using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamificacao3;

namespace Gamificacao3
{
    public class Pedido
{
    private int _id;
    private DateTime _data;
    private Cliente? _cliente;
    private string _status;
    //private List<ItemPedido> _itens;

    public int Id{
        get { return _id; }
        set { _id = value; }
    }

    public DateTime Data{
        get { return _data; }
        set { _data = value;}
    }

    public Cliente? Cliente{
        get { return _cliente; }
        set { _cliente = value;}
    }

    public string Status {
        get { return _status; }
        set { _status = value; }
    }

    /*public List<ItemPedido> Itens {
        get { return _itens; }
    }*/

    public Pedido(int id, DateTime Data, Cliente? cliente, string status)
    {
        _id = id;
        _data = Data;
        _cliente = cliente;
        _status = status;
        //_itens = new List<ItemPedido>();
    }
}

    
}