using LinqToDB;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Linq;
using System.Threading.Tasks;

namespace Linqdb.Uwp
{
    public class MainViewModel : ObservableObject
    {
        private string _text;

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public IAsyncRelayCommand AddCommand => new AsyncRelayCommand(() => Add());

        public IAsyncRelayCommand Single1Command => new AsyncRelayCommand(() => Single1());

        public IAsyncRelayCommand Single2Command => new AsyncRelayCommand(() => Single2());

        public IRelayCommand Single3Command => new RelayCommand(() => Single3());

        private async Task Add()
        {
            var user = new User
            {
                Id = 1,
                Username = "username"
            };

            using (var db = AppDataConnection.New)
            {
                var response = await db.InsertAsync(user);
            }

            var notebook = new Notebook
            {
                Id = 1,
                Name = "aaa",
                UserId = 1
            };

            using (var db = AppDataConnection.New)
            {
                var response = await db.InsertAsync(notebook);
            }
        }

        private async Task Single1()
        {
            using (var db = AppDataConnection.New)
            {
                var response = await db
                                .User
                                .LoadWith(x => x.Notebooks)
                                .FirstOrDefaultAsync();
                Text = response.Username;
            }
        }

        private async Task Single2()
        {
            using (var db = AppDataConnection.New)
            {
                var response = await db
                                .User
                                .LoadWithAsTable(x => x.Notebooks)
                                .FirstOrDefaultAsync();
                Text = response.Username;
            }
        }

        private void Single3()
        {
            using (var db = AppDataConnection.New)
            {
                var query = from u in db.User.LoadWith(x => x.Notebooks)
                            select u;

                var response = query.FirstOrDefault();

                Text = response.Username;
            }
        }
    }
}