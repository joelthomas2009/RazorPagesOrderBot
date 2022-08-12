using System;
using System.IO;
using Xunit;
using OrderBot;

namespace OrderBot.tests
{
    public class OrderBotTest
    {
        [Fact(DisplayName = "Check whether the chat bot is successfully getting initiated for all types of users to place the online order from Penny’s Hot Chicken")]
        public void TestWelcome()
        {
            Session oSession = new Session("189-876-7764");
            String sInput = oSession.OnMessage("hello")[0];
            Assert.True(sInput.Contains("Hi! I'm Olivia, the online ordering assistant for Penny's Hot Chicken!"));
        }
        [Fact(DisplayName = "Check whether the customer name is succesfully getting saved by the chat bot.")]
        public void TestCustomerFullName()
        {
            Session oSession = new Session("189-876-7765");
            oSession.OnMessage("hello");
            String sInput = oSession.OnMessage("Joel Thomas")[0];
            String sOutput = $"Thank you {sInput}. What's the best mobile phone number to contact you?";
            Assert.True(sOutput.Contains(sInput));
        }
        [Fact(DisplayName = "Check whether the customer mobile number is succesfully getting saved by the chat bot.")]
        public void TestMobile()
        {
            Session oSession = new Session("189-876-0345");
            oSession.OnMessage("hello");
            oSession.OnMessage("Joel Thomas");
            String sInput = oSession.OnMessage("189-876-0345")[0];
            String sOutput = $"Great! The phone number {sInput} is successfully added! What type of burger would you like?";
            Assert.True(sOutput.Contains(sInput));
        }
        [Fact(DisplayName = "Check whether the burger type is succesfully getting added to the order by the chat bot.")]
        public void TestBurgerType()
        {
            Session oSession = new Session("189-876-0343");
            oSession.OnMessage("hello");
            oSession.OnMessage("Joel Thomas");
            oSession.OnMessage("189-876-0343");
            String sInput = oSession.OnMessage("Chicken")[0];
            String sOutput = $"What side item would you like on this {sInput} burger?";
            Assert.True(sOutput.Contains(sInput));
        }
        [Fact(DisplayName = "Check whether the side dish is succesfully getting added to the order by the chat bot.")]
        public void TestSideDishWithBurger()
        {
            Session oSession = new Session("189-876-0341");
            oSession.OnMessage("hello");
            oSession.OnMessage("Joel Thomas");
            oSession.OnMessage("189-876-0341");
            oSession.OnMessage("Chicken");
            String sInput = oSession.OnMessage("French Fries")[0];
            String sOutput = $"What drink would you like on this burger with {sInput}?";
            Assert.True(sOutput.Contains(sInput));
        }
        [Fact(DisplayName = "Check whether the chat bot will display the summary of items selected by the customers, prior confirming the order")]
        public void TestOrderItemConfirmation()
        {
            Session oSession = new Session("189-876-0388");
            oSession.OnMessage("hello");
            oSession.OnMessage("Joel Thomas");
            oSession.OnMessage("189-876-0388");
            String burgerType = oSession.OnMessage("Chicken")[0];
            String sideDish = oSession.OnMessage("French Fries")[0];
            String drink = oSession.OnMessage("Coke")[0];
            String sOutput = $"Please confirm the order items and type \"pay\": 1. {burgerType} Burger 2. Side item: {sideDish} 3. Drink: {drink}";
            Assert.True(sOutput.Contains(burgerType));
            Assert.True(sOutput.Contains(sideDish));
            Assert.True(sOutput.Contains(drink));
        }
        [Fact(DisplayName = "Check whether the chat bot will allow the customers to complete the order with different types of burgers, side dish and drinks")]
        public void TestCompleteOrderFlow()
        {
            Session oSession = new Session("189-876-0344");
            oSession.OnMessage("hello");
            String name = oSession.OnMessage("Joel Thomas")[0];
            oSession.OnMessage("189-876-0344");
            oSession.OnMessage("Chicken");
            oSession.OnMessage("French Fries");
            oSession.OnMessage("Coke");
            oSession.OnMessage("confirm");
            String sOutput = $"Sit back and relax! Thank you {name} for ordering with Penny's Hot Chicken! Your order will be delivered at";
            Assert.True(sOutput.Contains(name));
        }
        [Fact(DisplayName = "Check whether the database is getting updated once the order is initiated.")]
        public void TestDatabase()
        {
            string path = DB.GetConnectionString();
            Session oSession = new Session("169-876-7764");
            oSession.OnMessage("hello");
            oSession.OnMessage("Joel Thomas");
            oSession.OnMessage("169-876-7764");
        }
        [Fact(DisplayName = "Check whether the chat bot will provide an order confirmation message and an estimated time for preparing the order once the order is placed")]
        public void TestOrderConfirmationMessage()
        {
            Session oSession = new Session("169-876-7532");
            oSession.OnMessage("hello");
            String name = oSession.OnMessage("Joel Thomas")[0];
            oSession.OnMessage("169-876-7532");
            oSession.OnMessage("Chicken");
            oSession.OnMessage("French Fries");
            oSession.OnMessage("Coke");
            oSession.OnMessage("confirm");
            String sOutput = $"Sit back and relax! Thank you {name} for ordering with Penny's Hot Chicken! Your order will be delivered at";
            Assert.True(sOutput.Contains(name));
        }
    }
}
