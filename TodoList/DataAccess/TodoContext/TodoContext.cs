using Microsoft.EntityFrameworkCore;
using System;

namespace TodoList.DataAccess.TodoContext
{
    public class TodoContext : DbContext
    {
        // EFCore SQL scalar function
        [DbFunction("ufnGetStock", "dbo")]
        public static int Abc(int id)
        {
            throw new NotImplementedException();
        }

        private static bool _recreate;

        /// не сработает - Repository не получит контекст
        //static TodoContext()
        //{
        //    var a = new TodoContext();
        //    a.Database.EnsureDeleted();
        //    a.Database.EnsureCreated();
        //}
        //public TodoContext() { }

        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
            // TODO: сделать нормальное создание бд
            if (_recreate) return;
            //var onDelete = Database.EnsureDeleted();
            //var onCreate = Database.EnsureCreated();
            //if (!(onDelete && onCreate))
            //{
            //    throw new NotImplementedException("DB Create Error");
            //}
            _recreate = true;
        }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<EntryEntity> Entries { get; set; }

        public DbSet<UserStatusEntity> UserStatus { get; set; }

        public DbSet<ProblemStatusEntity> ProblemStatus { get; set; }

        /// Many-to-many на данный момент не нужно, но понадобится в дальнейшем
        //public DbSet<UserEntryEntity> UsersEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .HasKey(k => k.UserId);

            modelBuilder.Entity<EntryEntity>()
                .HasKey(k => k.EntryId);

            modelBuilder.Entity<UserStatusEntity>()
                .HasKey(k => k.UserStatusId);

            modelBuilder.Entity<ProblemStatusEntity>()
                .HasKey(k => k.ProblemStatusId);

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