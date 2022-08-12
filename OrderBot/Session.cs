using System;

namespace OrderBot
{
    public class Session
    {
        private enum State
        {
            WELCOMING, BURGER_TYPE, DRINK, PHONE, NAME, THANKYOU, PAYMENT, CVV, ORDERCONFIRM
        }

        private State nCur = State.WELCOMING;
        private Order oOrder;
        public string PhoneNumber {get; set;}

        public Session(string sPhone)
        {
            this.oOrder = new Order();
            this.oOrder.Phone = sPhone;
        }

        public List<String> OnMessage(String sInMessage)
        {
            List<String> aMessages = new List<string>();
            switch (this.nCur)
            {
                case State.WELCOMING:
                    aMessages.Add("Hi! I'm Olivia, the online ordering assistant for Penny's Hot Chicken!");
                    aMessages.Add("To get started, can you please provide me with your first and last name?");
                    this.nCur = State.NAME;
                    break;
                case State.NAME:
                    this.oOrder.Name = sInMessage;
                    aMessages.Add($"Thank you {this.oOrder.Name}. What's the best mobile phone number to contact you?");
                    this.nCur = State.PHONE;
                    break;
                case State.PHONE:
                    PhoneNumber = sInMessage;
                    aMessages.Add($"Great! The phone number {PhoneNumber} is successfully added!\n");
                    aMessages.Add($"What type of burger would you like?");
                    this.nCur = State.BURGER_TYPE;
                    break;
                case State.BURGER_TYPE:
                    this.oOrder.BurgerType = sInMessage;
                    this.oOrder.Save();
                    aMessages.Add("What side item would you like on this  " + this.oOrder.BurgerType + " burger?");
                    this.nCur = State.DRINK;
                    break;
                case State.DRINK:
                    this.oOrder.SideItem = sInMessage;
                    this.oOrder.Size = "";
                    aMessages.Add("What drink would you like on this  " + this.oOrder.Size + " burger with " + this.oOrder.SideItem + "?");
                    this.nCur = State.THANKYOU;
                    break;
                case State.THANKYOU:
                    this.oOrder.Drink = sInMessage;
                    aMessages.Add("Please confirm the order items and type \"confirm\":");
                    aMessages.Add($"1. {this.oOrder.BurgerType} Burger");
                    aMessages.Add($"2. Side item: {this.oOrder.SideItem}");
                    aMessages.Add($"3. Drink: {this.oOrder.Drink}");
                    this.nCur = State.ORDERCONFIRM;
                    this.oOrder.Save();
                    break;
                case State.ORDERCONFIRM:
                    DateTime now = DateTime.Now;
                    DateTime orderConfirmTime = now.AddMinutes(30);
                    this.oOrder.Save();
                    aMessages.Add("Sit back and relax!");
                    aMessages.Add("Thank you for ordering with Penny's Hot Chicken!");
                    aMessages.Add($"Your order will be delivered at {orderConfirmTime.ToString("F")}");
                    break;
            }
            aMessages.ForEach(delegate (String sMessage)
            {
                System.Diagnostics.Debug.WriteLine(sMessage);
            });
            return aMessages;
        }

    }
}
