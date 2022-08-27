using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.ViewModel
{

    partial class Roupa : ObservableObject
    {
        [ObservableProperty]
        string nome;

        [ObservableProperty]
        int peso;

        [ObservableProperty]
        int quantidade = 0;

        public static Roupa FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            Roupa roupa = new Roupa();
            roupa.nome = values[0];
            roupa.peso = int.Parse(values[1]);
            return roupa;
        }
    }
    partial class MyViewModel : ObservableObject
    {
        public double PesoTotal
        {
            get
            {
                return (
                    comuns.Select(x => x.Peso * x.Quantidade).Sum() +
                    jeans.Select(x => x.Peso * x.Quantidade).Sum() +
                    camaMesaBanho.Select(x => x.Peso * x.Quantidade).Sum()) / 1000.0;
            }
        }

        [ObservableProperty]
        BindingList<Roupa> roupas;

        
        [ObservableProperty]
        BindingList<Roupa> comuns, jeans, camaMesaBanho;

        public MyViewModel()
        {

            LerCsv(ref comuns, "Comum.csv");
            LerCsv(ref jeans, "Jeans.csv");
            LerCsv(ref camaMesaBanho, "CamaMesaBanho.csv");
            comuns.ListChanged += Roupas_ListChanged;
            jeans.ListChanged += Roupas_ListChanged;
            camaMesaBanho.ListChanged += Roupas_ListChanged;
        }

        public IEnumerable<string> ReadLines(Func<Stream> streamProvider,
                                     Encoding encoding)
        {
            using (var stream = streamProvider())
            using (var reader = new StreamReader(stream, encoding))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
        private void LerCsv(ref BindingList<Roupa> roupas, string arquivoCsv)
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
            
            var stream = FileSystem.OpenAppPackageFileAsync(arquivoCsv);
            stream.Wait();
            

            roupas = new BindingList<Roupa>(this.ReadLines(() => stream.Result, Encoding.UTF8)
                                          .Select(Roupa.FromCsv)
                                          .ToList());
        }

        private void Roupas_ListChanged(object sender, ListChangedEventArgs e)
        {
            OnPropertyChanged(nameof(PesoTotal));
        }
    }
}
