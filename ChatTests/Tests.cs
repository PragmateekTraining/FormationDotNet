using NUnit.Framework;
using System.Threading;

namespace ChatTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void CanSendMessages()
        {
            CancellationTokenSource source = new CancellationTokenSource();

            string outputServerUsername = null;
            string outputServerMessage = null;
            string outputJohnUsername = null;
            string outputJohnMessage = null;
            string outputBobUsername = null;
            string outputBobMessage = null;

            Chat.Server server = new Chat.Server();
            server.StartAsync(12345, source.Token);

            server.NewParticipant += (s, a) =>
            {
                outputServerUsername = a.Username;
            };

            server.NewMessage += (s, a) =>
            {
                outputServerUsername = a.Username;
                outputServerMessage = a.Message;
            };

            while (!server.IsStarted) Thread.Sleep(100);

            Chat.Client John = new Chat.Client();

            John.NewParticipant += (s, a) =>
            {
                outputJohnUsername = a.Username;
            };

            John.NewMessage += (s, a) =>
            {
                outputJohnUsername = a.Username;
                outputJohnMessage = a.Message;
            };

            Chat.Client Bob = new Chat.Client();

            Bob.NewParticipant += (s, a) =>
            {
                outputBobUsername = a.Username;
            };

            Bob.NewMessage += (s, a) =>
            {
                outputBobUsername = a.Username;
                outputBobMessage = a.Message;
            };

            John.Connect("localhost", 12345, "John");

            Thread.Sleep(1000);

            Assert.AreEqual("John", outputServerUsername);

            Bob.Connect("localhost", 12345, "Bob");

            Thread.Sleep(1000);

            Assert.AreEqual("Bob", outputServerUsername);
            Assert.AreEqual("Bob", outputJohnUsername);

            string message = "Hi everybody!";

            outputServerUsername = null;
            outputJohnUsername = null;
            outputBobUsername = null;            
            outputServerMessage = null;
            outputJohnMessage = null;
            outputBobMessage = null;

            John.SendMessage(message);

            Thread.Sleep(1000);

            Assert.AreEqual("John", outputServerUsername);
            Assert.AreEqual(message, outputServerMessage);
            Assert.AreEqual("John", outputBobUsername);
            Assert.AreEqual(message, outputBobMessage);

            message = "How are you?";

            John.SendMessage(message);

            Thread.Sleep(1000);

            Assert.AreEqual("John", outputServerUsername);
            Assert.AreEqual(message, outputServerMessage);
            Assert.AreEqual("John", outputBobUsername);
            Assert.AreEqual(message, outputBobMessage);

            message = "Fine and you?";
            Bob.SendMessage(message);

            Thread.Sleep(1000);

            Assert.AreEqual("Bob", outputServerUsername);
            Assert.AreEqual(message, outputServerMessage);
            Assert.AreEqual("Bob", outputJohnUsername);
            Assert.AreEqual(message, outputJohnMessage);

            source.Cancel();
        }
    }
}
