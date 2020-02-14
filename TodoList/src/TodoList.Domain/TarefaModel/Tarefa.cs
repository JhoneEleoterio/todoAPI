
using System;
using TodoList.Domain.SharedKernel;

namespace TodoList.Domain.TarefaModel
{
    public class Tarefa : Entity
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }

        public Tarefa(string titulo, string descricao)
        {
            Titulo = titulo;
            Descricao = descricao;
        }
    }
}
