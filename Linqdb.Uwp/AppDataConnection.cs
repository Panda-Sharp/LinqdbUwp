using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace Linqdb.Uwp
{
    public class AppDataConnection : DataConnection
    {
        public AppDataConnection(LinqToDbConnectionOptions options) : base(options)
        {
        }

        public static AppDataConnection New =>
            new AppDataConnection(
                new LinqToDbConnectionOptionsBuilder()
                    .UseSQLiteMicrosoft("Data Source=db.sqlite")
                        .Build());

        public ITable<User> User => this.GetTable<User>();

        public ITable<Notebook> Notebook => this.GetTable<Notebook>();

        public static void Initialize()
        {
            var appDataConnection = New;

            appDataConnection.CreateTable<Notebook>(tableOptions: TableOptions.CreateIfNotExists);
            appDataConnection.CreateTable<User>(tableOptions: TableOptions.CreateIfNotExists);
        }
    }

    public class User
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Username { get; set; }

        [Association(ThisKey = nameof(Id), OtherKey = nameof(Notebook.UserId), CanBeNull = true, Relationship = Relationship.OneToMany, IsBackReference = true)]
        public IEnumerable<Notebook> Notebooks { get; set; } = null;
    }

    public class Notebook
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Name { get; set; }

        [Column("User_Id")]
        public int UserId { get; set; }

        [Association(ThisKey = nameof(UserId), OtherKey = nameof(Id), CanBeNull = true, Relationship = Relationship.ManyToOne, BackReferenceName = nameof(Uwp.User.Notebooks))]
        public User User { get; set; } = null;
    }
}