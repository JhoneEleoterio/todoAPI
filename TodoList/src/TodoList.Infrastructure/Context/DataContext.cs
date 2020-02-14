using Microsoft.EntityFrameworkCore;
using TodoList.Domain.TarefaModel;

namespace TodoList.Infrastructure.Context
{
    public class DataContext : DbContext
    {
        public DbSet<Tarefa> Tarefas { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
        :base(options){}
    }
}
