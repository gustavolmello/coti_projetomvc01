using ProjetoAspNetMVC01.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAspNetMVC01.Repository.Interfaces
{
    public interface ITarefaRepository : IBaseRepository<Tarefa>
    {
        List<Tarefa> ConsultarPorDatas(DateTime dataMin, DateTime dataMax);
    }
}
