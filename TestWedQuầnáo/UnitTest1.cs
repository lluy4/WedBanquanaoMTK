using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTests
{
    [TestFixture]
    public class LoginTests
    {
        private IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://localhost:44381/Home/DangKy");
        }

        [Test]
        public void TestLogin()
        {
            // Điền thông tin đăng nhập
            IWebElement usernameField = driver.FindElement(By.Id("username"));
            usernameField.SendKeys("testuser");

            IWebElement passwordField = driver.FindElement(By.Id("password"));
            passwordField.SendKeys("Password123");

            // Nhấn nút đăng nhập
            IWebElement submitButton = driver.FindElement(By.Id("submitButton"));
            submitButton.Click();

            // Kiểm tra xem đăng nhập thành công hay không
            IWebElement welcomeMessage = driver.FindElement(By.Id("welcomeMessage"));
            Assert.Equals("Welcome, testuser!", welcomeMessage.Text); // Sử dụng Assert.AreEqual của NUnit
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}