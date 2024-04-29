using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {

        Console.WriteLine("Inserire la partenza del volo: ");
        string partenza = Console.ReadLine();

        Console.WriteLine("Inserire la destinazione del volo: ");
        string destinazione = Console.ReadLine();
        /*
         

        

        Console.WriteLine("Inserire la data di partenza (ANNO,MESE,GIORNO): ");
        string input = Console.ReadLine();
        string dataPartenza = Regex.Replace(input, @"\D", "");

        Console.WriteLine("Inserire la data di ritorno (ANNO,MESE,GIORNO): ");
        string input2 = Console.ReadLine();
        string dataArrivo = Regex.Replace(input2, @"\D", "");
         */

        ChromeOptions options = new ChromeOptions();
        options.AddArgument("start-maximized");
        options.AddUserProfilePreference("profile.default_content_setting_values.cookies", 2);
        IWebDriver driver = new ChromeDriver(options);
        driver.Navigate().GoToUrl("https://www.kayak.it/flights/" + partenza + "-" + destinazione + "/2024-05-29/2024-06-05?sort=bestflight_a");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
        IWebElement div = driver.FindElement(By.ClassName("hJSA"));

        var innerDivs = div.FindElements(By.ClassName("VY2U"));
        List<IWebElement> orari = new List<IWebElement>();

        foreach (var innerDiv in innerDivs)
        {
            var elementiOrari = innerDiv.FindElements(By.ClassName("vmXl"));
            orari.AddRange(elementiOrari);
        }

        List<string> lista = new List<string>();

        foreach (IWebElement ele in orari)
        {
            var elements = ele.FindElements(By.TagName("span"));
            foreach (var e in elements)
            {
                lista.Add(e.Text);
            }
        }

        for (int i = 0; i < lista.Count; i++)
        {
            Console.WriteLine(lista[i]);
        }
           
    }
}
