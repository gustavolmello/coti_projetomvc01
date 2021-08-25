using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAspNetMVC01.Repository.Entities
{
    public class Tarefa
    {
        public Guid IdTarefa { get; set; }
        public string Nome { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public string Descricao { get; set; }
        public string Prioridade { get; set; }
    }
}
