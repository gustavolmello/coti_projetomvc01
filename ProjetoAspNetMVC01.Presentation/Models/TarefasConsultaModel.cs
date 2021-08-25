using ProjetoAspNetMVC01.Repository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAspNetMVC01.Presentation.Models
{
    public class TarefasConsultaModel
    {
        [Required(ErrorMessage = "Por favor, informe a data de inicio.")]
        public string DataMin { get; set; }

        [Required(ErrorMessage = "Por favor, informe a data de término.")]
        public string DataMax { get; set; }

        /*
         * Propriedade para exibir na página a listagem
         * das tarefas obtidas na consulta do banco de dados
         */
        public List<Tarefa> Tarefas { get; set; }
    }
}
