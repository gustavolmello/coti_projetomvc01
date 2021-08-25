using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAspNetMVC01.Repository.Interfaces
{
    public interface IBaseRepository<T>
    {
        void Inserir(T obj);
        void Alterar(T obj);
        void Excluir(T obj);

        List<T> Consultar();
        T ObterPorId(Guid id);
    }
}
