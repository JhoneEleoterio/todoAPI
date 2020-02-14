using TodoList.Domain.TarefaModel;
using TodoList.Domain.TarefaModel.Interfaces;
using TodoList.Infrastructure.Context;
using TodoList.Infrastructure.Repositories;

namespace TodoList.Infrastructure
{
    public class TarefaRepository : Repository<Tarefa>, ITarefaRepository
    {
        private DataContext _context;

        public TarefaRepository(DataContext context) 
            : base(context)
        {
            _context = context;
        }

        public Tarefa GetById(int Id)
        {
            return _context.Tarefas.Find(Id);
        }
    }
}
