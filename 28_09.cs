using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    interface IPayment
    {
        string Name { get; }
        bool StatusPayment(decimal total);
        string GetPaymentDetails();
    }
    public class CreditCardPayment : IPayment
    {
        public string Name => "Банковская карта";
        private string cardNumber;

        public CreditCardPayment(string _cardNumber)
        {
            cardNumber = _cardNumber;
        }
        public bool StatusPayment(decimal total)
        {
            Console.WriteLine($"Платёжная система: {Name}");
            Console.WriteLine($"Списано {total} руб. с карты {cardNumber}");
            Console.WriteLine("Оплата картой прошла\n");
            return true;
        }

        public string GetPaymentDetails()
        {
            return $"Карта: {cardNumber}";
        }
    }

    public class PayPalPayment : IPayment
    {
        public string Name => "PayPal";
        private string email;

        public PayPalPayment(string _email)
        {
            email = _email;
        }
        public bool StatusPayment(decimal total)
        {
            Console.WriteLine($"Платёжная система: {Name}");
            Console.WriteLine($"Списано {total} руб. с аккаунта PayPal: {email}");
            Console.WriteLine("PayPal оплата \n");
            return true;
        }

        public string GetPaymentDetails()
        {
            return $"Email PayPal: {email}";
        }
    }
    public class CryptoPayment : IPayment
    {
        public string Name => "Криптовалюта";
        private string walletAddress;
        private string cryptoType;

        public CryptoPayment(string _walletAddress, string _cryptoType = "BTC")
        {
            walletAddress = _walletAddress;
            cryptoType = _cryptoType;
        }
        public bool StatusPayment(decimal total)
        {
            Console.WriteLine($"Платёжная система: {Name}");
            Console.WriteLine($"Получено {total} руб. ({total * 0.000014m:F8} {cryptoType})");
            Console.WriteLine($"на кошелёк: {walletAddress}");
            Console.WriteLine("транзакция осуществленна");
            return true;
        }
        public string GetPaymentDetails()
        {
            return $"{cryptoType} кошелёк: {walletAddress}";
        }
    }
    public class PaymentProcessor
    {
        private List<IPayment> paymentMethods;
        public PaymentProcessor()
        {
            paymentMethods = new List<IPayment>();
        }
        public void AddPaymentMethod(IPayment payment)
        {
            paymentMethods.Add(payment);
            Console.WriteLine($"добавлен способ оплаты: {payment.Name}\n");
        }
        public bool ProcessPayment(IPayment payment, decimal amount)
        {
            Console.WriteLine("обработка платежа");
            Console.WriteLine($"детали оплаты: {payment.GetPaymentDetails()}");
            
            bool result = payment.StatusPayment(amount);        
            Console.WriteLine("платеж пройден\n");
            return result;
        }

        public void ShowAvailablePayments()
        {
            Console.WriteLine("Доступные способы оплаты:");
            foreach (var payment in paymentMethods)
            {
                Console.WriteLine($"- {payment.Name}: {payment.GetPaymentDetails()}");
            }
            Console.WriteLine();
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            PaymentProcessor processor = new PaymentProcessor();
            IPayment card = new CreditCardPayment("3222 1234");
            IPayment paypal = new PayPalPayment("user@gmail.com");
            IPayment crypto = new CryptoPayment("322GH", "BTC");
            processor.AddPaymentMethod(card);
            processor.AddPaymentMethod(paypal);
            processor.AddPaymentMethod(crypto);

            processor.ShowAvailablePayments();

            decimal orderAmount = 1500.50m;        
            processor.ProcessPayment(card, orderAmount);
            processor.ProcessPayment(paypal, orderAmount);
            processor.ProcessPayment(crypto, orderAmount);
            Console.ReadKey();
        }
    }
}
