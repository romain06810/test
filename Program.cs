using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;


namespace ConsoleApp8
{

    [TestFixture]
    class TestReportExample
    {
        public static void Main() { }
        //Declarations extent - rapport de test
        private ExtentReports extent;
        private ExtentHtmlReporter htmlReporter;
        ExtentTest test;

        //Autres decl
        IWebDriver window;
        private WebDriverWait wait;
        private static string emailCreation = "duponjojo@yahoo.fr";

        //setUp une seule fois pour l'execution de tous les tests - SetUp le fichier HTML de rapport
        [OneTimeSetUp]
        public void StartReport()
        {
            htmlReporter = new ExtentHtmlReporter(@"C:\Users\formation\source\repos\ConsoleApp8\ConsoleApp8\TestReport.html");
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }

        [SetUp]
        public void Init()
        {
            // allez à la page de google
            window = new ChromeDriver();
            window.Navigate().GoToUrl("https://www.etal-des-epices.com/a-d/186-5-baies.html");

        }

        [Test]
        public void OpenAccount()
        {
            test = extent.CreateTest("Créer un compte");
            window.Url = "https://www.etal-des-epices.com/authentification?back=my-account";
            window.FindElement(By.Id("email_create")).SendKeys(emailCreation);
            window.FindElement(By.Id("SubmitCreate")).Click();
            wait = new WebDriverWait(window, new TimeSpan(0, 0, 10));
            var elementCreate = wait.Until(condition =>
            {
                try
                {
                    var elementToBeDisplayed = window.FindElement(By.Id("id_gender1"));
                    return elementToBeDisplayed.Displayed;
                }
                catch (StaleElementReferenceException e)
                {
                    return false;
                }
                catch (NoSuchElementException e)
                {
                    return false;
                }
            });
            try
            {
                Assert.IsTrue(window.FindElement(By.Id("id_gender1")).Displayed);
                //Assert si gender (M., Melle. Mr. ) sont affichés. Pour voir si la page se charge bien
            }
            catch
            {
                test.Fail("probleme de chargement de page 10sec....");
            }
            test.Pass("Element trouvé, donc ma page est chargée");
            window.FindElement(By.Id("id_gender1")).Click();
            window.FindElement(By.Id("customer_firstname")).SendKeys("Jojo");
            window.FindElement(By.Id("customer_lastname")).SendKeys("DUPON");
            window.FindElement(By.Id("passwd")).SendKeys("Dupon2018");
            new SelectElement(window.FindElement(By.Name("days"))).SelectByValue("12");
            new SelectElement(window.FindElement(By.Name("months"))).SelectByValue("9");
            new SelectElement(window.FindElement(By.Name("years"))).SelectByValue("2016");
            window.FindElement(By.Id("newsletter")).Click();
            window.FindElement(By.Id("company")).SendKeys("Dupon&fils");
            window.FindElement(By.Name("vat_number")).SendKeys("FR40123456824");
            window.FindElement(By.Id("address1")).SendKeys("96 route badine");
            window.FindElement(By.Id("postcode")).SendKeys("06600");
            window.FindElement(By.Id("city")).SendKeys("Antibes");
            window.FindElement(By.Id("id_country")).SendKeys("France");
            window.FindElement(By.Id("other")).SendKeys("SEXY");
            window.FindElement(By.Id("phone")).SendKeys("0419263465");
            window.FindElement(By.Id("phone_mobile")).SendKeys("0658239678");
            window.FindElement(By.Id("alias")).Clear();
            window.FindElement(By.Id("alias")).SendKeys("jojoépicé");
            window.FindElement(By.Id("submitAccount")).Click();
        }

        [Test]
        public void AddProduct()
        {
            //Créer un test dans mon rapport HTML - Ajouter au panier
            test = extent.CreateTest("Ajouter au panier");
            window.FindElement(By.Id("add_to_cart")).Click();
            var wait = new WebDriverWait(window, new TimeSpan(0, 0, 10));
            var element = wait.Until(condition =>
            {
                try
                {
                    var elementToBeDisplayed = window.FindElement(By.XPath("//*[@id='layer_cart']/div[1]/div[2]/span/span[2]"));
                    return elementToBeDisplayed.Displayed;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            //Logger des informations
            test.Log(test.Status, "----> " + window.FindElement(By.XPath("//*[@id='layer_cart']/div[1]/div[2]/span/span[2]")).Text);
            try
            {
                Assert.AreEqual(1, 0);
            }
            // Le test fail, 
            catch
            {
                test.Fail("Problem ... 1 and 0 are not equal ");
            }
            //Ajouter une capture d'ecran à votre rapport HTML
            test.AddScreenCaptureFromPath(@"C:\Users\formation\source\repos\ConsoleApp10\ConsoleApp10\EDE.jpg");
        }

        // Nettoyer extent et Fermer la fenetre après chaque test
        [TearDown]
        public void CleanUp()
        {
            extent.Flush();
            window.Close();
        }
    }
}