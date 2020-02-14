using TodoList.Domain.SharedKernel;

namespace TodoList.Domain.TarefaModel.Interfaces
{
    public interface ITarefaRepository : IRepository<Tarefa>
    {
        Tarefa GetById(int Id);
    }
}
