using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Text.Json;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {

        Console.WriteLine("Inserire la partenza del volo: ");
        string partenza = Console.ReadLine();

        Console.WriteLine("Inserire la destinazione del volo: ");
        string destinazione = Console.ReadLine();


        ChromeOptions options = new ChromeOptions();
        options.AddArgument("start-maximized");
        options.AddUserProfilePreference("profile.default_content_setting_values.cookies", 2);
        IWebDriver driver = new ChromeDriver(options);
        driver.Navigate().GoToUrl("https://www.kayak.it/flights/" + partenza + "-" + destinazione + "/2024-05-29/2024-06-05?sort=bestflight_a");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
        IWebElement button = driver.FindElement(By.ClassName("RxNS"));
        
        button.Click();

        Thread.Sleep(2000);

          var div = driver.FindElements(By.ClassName("hJSA"));

        var innerDivs = driver.FindElements(By.ClassName("VY2U"));
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
                if (e.Text != "\u2013")
                    lista.Add(e.Text);
            }
        }

        var footer = driver.FindElements(By.ClassName("nrc6-default-footer"));
        List<IWebElement> compagnie = new List<IWebElement>();

        foreach (var innerFooter in footer)
        {
            var elementiFoooter = innerFooter.FindElements(By.ClassName("J0g6-labels-grp"));
            compagnie.AddRange(elementiFoooter);
        }

        List<string> listaCompagnie = new List<string>();

        foreach (IWebElement ele in compagnie)
        {
            var elements = ele.FindElements(By.ClassName("J0g6-operator-text"));
            foreach (var e in elements)
            {
                listaCompagnie.Add(e.Text);
            }
        }

                var divPrezzi = driver.FindElements(By.ClassName("nrc6-price-section"));
        List<IWebElement> prezzi = new List<IWebElement>();

        foreach (var innerPrezzi in divPrezzi)
        {
            var elementiPrezzi = innerPrezzi.FindElements(By.ClassName("f8F1"));
            prezzi.AddRange(elementiPrezzi);
        }

        List<string> listaPrezzi = new List<string>();

        foreach (IWebElement ele in prezzi)
        {
            var elements = ele.FindElements(By.ClassName("f8F1-price-text"));
            foreach (var e in elements)
            {
                if (e.Text.Contains("\u20AC"))
                {
                    listaPrezzi.Add(e.Text);
                }
                
            }
        }


        int temp = 0; int temp2 = 0; int temp3 = 0;
        List<string> stampa = new List<string>();
        for (int i = 0; i < lista.Count; i++)
        {
            if (lista[i] != "-")
            {
                if (temp % 4 == 0)
                {
                    //Console.WriteLine(listaCompagnie[temp2]);
                    stampa.Add(listaCompagnie[temp2]);
                    temp2++;
                    
                }
                //Console.WriteLine(lista[i]);
                stampa.Add(lista[i]);
                temp++;
                if(temp % 4 == 0)
                {
                    stampa.Add(listaPrezzi[temp3]);
                    temp3++;
                }
            } 
        }

        for (int i = 0; i < stampa.Count; i++)
        {
            Console.WriteLine(stampa[i]);
        }
        
        
        string JSON = JsonSerializer.Serialize(stampa);
        File.WriteAllText("Scraping.json", JSON);

    }
}
