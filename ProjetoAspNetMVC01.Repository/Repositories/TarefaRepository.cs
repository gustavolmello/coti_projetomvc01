using Dapper;
using ProjetoAspNetMVC01.Repository.Entities;
using ProjetoAspNetMVC01.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAspNetMVC01.Repository.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        //atributo de valor somente leitura (readonly)
        private readonly string _connectionstring;

        //método para receber o valor da connectionstring
        //injeção de dependencia (passar como parametro o endereço da connectionstring)
        public TarefaRepository(string connectionstring)
        {
            _connectionstring = connectionstring;
        }

        public void Inserir(Tarefa obj)
        {
            var query = @"
                    INSERT INTO TAREFA(IDTAREFA, NOME, DATA, HORA, DESCRICAO, PRIORIDADE)
                    VALUES(@IdTarefa, @Nome, @Data, @Hora, @Descricao, @Prioridade)
                ";

            using (var connection = new SqlConnection(_connectionstring))
            {
                connection.Execute(query, obj);
            }
        }

        public void Alterar(Tarefa obj)
        {
            var query = @"
                    UPDATE TAREFA 
                    SET
                        NOME = @Nome,
                        DATA = @Data,
                        HORA = @Hora,
                        DESCRICAO = @Descricao,
                        PRIORIDADE = @Prioridade
                    WHERE
                        IDTAREFA = @IdTarefa
                ";

            using (var connection = new SqlConnection(_connectionstring))
            {
                connection.Execute(query, obj);
            }
        }

        public void Excluir(Tarefa obj)
        {
            var query = @"
                    DELETE FROM TAREFA 
                    WHERE IDTAREFA = @IdTarefa
                ";

            using (var connection = new SqlConnection(_connectionstring))
            {
                connection.Execute(query, obj);
            }
        }

        public List<Tarefa> Consultar()
        {
            var query = @"
                    SELECT * FROM TAREFA
                    ORDER BY DATA DESC, HORA DESC
                ";

            using (var connection = new SqlConnection(_connectionstring))
            {
                return connection
                    .Query<Tarefa>(query)
                    .ToList();
            }
        }

        public Tarefa ObterPorId(Guid id)
        {
            var query = @"
                    SELECT * FROM TAREFA
                    WHERE IDTAREFA = @id
                ";

            using (var connection = new SqlConnection(_connectionstring))
            {
                return connection
                    .Query<Tarefa>(query, new { id })
                    .FirstOrDefault();
            }
        }

        public List<Tarefa> ConsultarPorDatas(DateTime dataMin, DateTime dataMax)
        {
            var query = @"
                    SELECT * FROM TAREFA
                    WHERE DATA BETWEEN @dataMin AND @dataMax
                    ORDER BY DATA DESC, HORA DESC
                ";

            using (var connection = new SqlConnection(_connectionstring))
            {
                return connection
                    .Query<Tarefa>(query, new { dataMin, dataMax })
                    .ToList();
            }
        }
    }
}
