using Microsoft.EntityFrameworkCore;

namespace TodoList.DataAccess.TodoContext
{
    public class TodoContext : DbContext
    {
        private static bool _recreate;

        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
            // TODO: сделать нормальное создание бд
            if (!_recreate)
            {
                var onDelete = Database.EnsureDeleted();
                var onCreate = Database.EnsureCreated();
                _recreate = true;
            }
        }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<EntryEntity> Entries { get; set; }

        /// Many-to-many на данный момент не нужно, но понадобится в дальнейшем
        //public DbSet<UserEntryEntity> UsersEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .HasKey(k => k.UserId);

            modelBuilder.Entity<EntryEntity>()
                .HasKey(k => k.EntryId);

            /// Many-to-many на данный момент не нужно, но понадобится в дальнейшем
            //modelBuilder.Entity<UserEntryEntity>()
            //    .HasKey(k => new { UserId = k.UserId, EntryId = k.EntryId });
            //modelBuilder.Entity<UserEntryEntity>()
            //    .HasOne(o => o.UserBind)
            //    .WithMany(m => m.UserEntryBind)
            //    .HasForeignKey(fk => fk.UserId);
            //modelBuilder.Entity<UserEntryEntity>()
            //    .HasOne(o => o.EntryBind)
            //    .WithMany(m => m.UserEntryBind)
            //    .HasForeignKey(fk => fk.EntryId);
        }
    }
}