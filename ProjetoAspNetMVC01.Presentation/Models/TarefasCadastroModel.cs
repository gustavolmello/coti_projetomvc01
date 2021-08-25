using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAspNetMVC01.Presentation.Models
{
    public class TarefasCadastroModel
    {
        [MinLength(6, ErrorMessage = "Por favor, informe no mínimo {1} caracteres.")]
        [MaxLength(150, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe o nome da tarefa.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Por favor, informe a data da tarefa.")]
        public string Data { get; set; }

        [Required(ErrorMessage = "Por favor, informe a hora da tarefa.")]
        public string Hora { get; set; }

        [MinLength(6, ErrorMessage = "Por favor, informe no mínimo {1} caracteres.")]
        [MaxLength(500, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe a descrição da tarefa.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Por favor, informe a prioridade da tarefa.")]
        public string Prioridade { get; set; }
    }
}
