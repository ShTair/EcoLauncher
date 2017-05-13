using mshtml;
using System.Linq;

namespace EcoLauncher.ViewModels
{
    class AccountViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IHTMLElement StartElement { get; set; }

        public AccountViewModel(HTMLTableRow tr)
        {
            var spans = tr.getElementsByTagName("span").Cast<IHTMLElement>().ToList();
            Id = spans[1].innerHTML;
            Name = spans[0].innerText;

            StartElement = tr.getElementsByTagName("img").Cast<IHTMLElement>().Skip(1).First();
        }
    }
}
