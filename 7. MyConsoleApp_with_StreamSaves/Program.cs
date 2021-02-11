using System;
using System.IO;
using System.Collections.Generic;

namespace _7._MyConsoleApp_with_StreamSaves
{
    class Program
    {
        static void Main(string[] args)
        {
            usingTips();
            string[] productSaves = File.ReadAllLines(@"../../../productSaves.txt");
            var writer = File.AppendText(@"../../../productSaves.txt");
            Dictionary<string, double> productNameAndPrice = new Dictionary<string, double>();
            Dictionary<string, double[]> productNamePriceandQuantity = new Dictionary<string, double[]>();
            double totalPrice = 0;

            int counter = 0;
            string[] currentProduct = new string[productSaves.Length + 1];

            if (productSaves.Length > 0)
            {
                currentProduct = productSaves[counter].Split();
            }

            while (currentProduct[0] != "" && productSaves.Length >= counter && currentProduct[0] != null)
            {
                string productName = currentProduct[0];
                double productCost = double.Parse(currentProduct[1]);

                productNameAndPrice.Add(productName, productCost);
                counter++;

                if (counter == productSaves.Length)
                {
                    break;
                }

                currentProduct = productSaves[counter].Split();

            }

            string[] purchasedProduct = Console.ReadLine().ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string text = purchasedProduct[0];
            string product = purchasedProduct[1];

            while (text != "end")
            {
                if (purchasedProduct.Length == 2)
                {
                    if (text == "add")
                    {
                        if (!productNameAndPrice.ContainsKey(product))
                        {                           
                            Console.Write($"How much to be {product}'s price? - ");
                            double addedProductValue = double.Parse(Console.ReadLine());
                            productNameAndPrice.Add(product, addedProductValue);
                            writer.WriteLine($"{product} {addedProductValue}");
                            writer.Flush();
                            Console.WriteLine($"{product} is succesfully added with price {addedProductValue} per quantity!");
                        }
                        else
                        {
                            Console.WriteLine($"{product} is already avaliable! You dont't need to add it!");
                        }

                    }
                }

                else if (purchasedProduct.Length == 3)
                {
                    int quantity = int.Parse(purchasedProduct[2]);

                    if (text == "order")
                    {
                        if (productNameAndPrice.ContainsKey(product))
                        {
                            if (!productNamePriceandQuantity.ContainsKey(product))
                            {
                                productNamePriceandQuantity.Add(product, new double[] { quantity, quantity * productNameAndPrice[product] });
                            }
                            else
                            {
                                productNamePriceandQuantity[product][0] += quantity;
                            }

                            totalPrice += quantity * productNameAndPrice[product];
                            Console.WriteLine("Ordered!");
                        }

                        else
                        {
                            Console.Write($"{product} isn't avaliable! Do you want to add it? Y/N - ");
                            string yesOrNo = Console.ReadLine().ToUpper();

                            if (yesOrNo == "Y")
                            {
                                Console.Write($"How much to be {product}'s price? - ");
                                double addedProductValue = double.Parse(Console.ReadLine());
                                productNameAndPrice.Add(product, addedProductValue);
                                writer.WriteLine($"{product} {addedProductValue}");
                                Console.WriteLine($"{product} is succesfully added with price {addedProductValue} per quantity!");
                            }
                            else if (yesOrNo == "N")
                            {
                                Console.WriteLine($"The product {product} isn't added!");
                            }
                            else
                            {
                                Console.WriteLine("You write something different then Y or N!");
                                Console.WriteLine($"The product {product} isn't added!");
                            }

                        }
                    }
                }
                else if (text == "see")
                {
                    string first = purchasedProduct[1];
                    string second = purchasedProduct[2];
                    string third = purchasedProduct[3];


                    if (text == "see" && first == "the" && second == "price" && third == "of")
                    {
                        string productName = purchasedProduct[4];

                        if (productNameAndPrice.ContainsKey(productName))
                        {
                            Console.WriteLine($"The price of {productName} is {productNameAndPrice[productName]}$");
                        }
                        else
                        {
                            Console.Write($"The product {productName} isn't avaliable! Do you want to add it? Y/N - ");
                            string yesOrNo = Console.ReadLine().ToUpper();

                            if (yesOrNo == "Y")
                            {
                                Console.Write($"How much to be {productName}'s price? - ");
                                double addedProductValue = double.Parse(Console.ReadLine());
                                productNameAndPrice.Add(productName, addedProductValue);
                                writer.WriteLine($"{productName} {addedProductValue}");
                                Console.WriteLine($"The product {productName} is succesfully added with price {addedProductValue} per quantity!");
                            }
                            else if (yesOrNo == "N")
                            {
                                Console.WriteLine($"The product {productName} isn't added!");
                            }
                            else
                            {
                                Console.WriteLine("You write something different then Y or N!");
                                Console.WriteLine($"The product {productName} isn't added!");
                            }
                        }


                    }
                }
                else if (text == "show")
                {
                    string first = purchasedProduct[1];
                    string second = purchasedProduct[2];
                    string third = purchasedProduct[3];

                    if (text == "show" && first == "what" && second == "is" && third == "available")
                    {
                        showWhatsIsAvailable(productNameAndPrice);
                    }
                }

                purchasedProduct = Console.ReadLine().ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                text = purchasedProduct[0];
                if (purchasedProduct.Length > 1)
                {
                    product = purchasedProduct[1];
                }
            }

            Console.WriteLine($"Your bill is {totalPrice:F02}$");
            Console.Write("Do you want to see your order list? Y/N - ");
            string yesOrNot = Console.ReadLine().ToUpper();

            bool thanks = false;

            if (yesOrNot == "Y")
            {
                Queue<double> moneyQue = new Queue<double>();
                List<double> moneyShower = new List<double>();

                foreach (var pair in productNamePriceandQuantity)
                {
                    Console.WriteLine($"{pair.Value[0]}x {pair.Key}({productNameAndPrice[pair.Key]}$) = {pair.Value[1]:F02}$");
                    moneyQue.Enqueue(pair.Value[1]);
                }

                double moneyCollector = 0;

                while (moneyQue.Count > 0)
                {
                    double current = moneyQue.Dequeue();
                    moneyCollector += current;
                    if (moneyQue.Count > 0)
                    {
                        Console.WriteLine($"{moneyCollector:F02} + {(string.Join(" + ", moneyQue)):F02}");
                    }
                    else
                    {
                        Console.WriteLine($"Your bill - {moneyCollector:F02}$");
                    }

                }
            }
            else if (yesOrNot == "N")
            {
                thanks = true;
                Console.WriteLine("Thank you for choosing us!");
            }
            else
            {
                Console.WriteLine("You write something different then Y/N! It is understood like N!");
            }

            if (thanks == false)
            {
                Console.WriteLine("Thank you for choosing us!");
            }

        }

        static void usingTips()
        {
            Console.WriteLine("*** USING TIPS! ***");
            Console.WriteLine("The program has four functions (\"add\", \"order\", \"see the price of ...\"");
            Console.WriteLine("and \"show what is available\")!");
            Console.WriteLine("Functions using -------");
            Console.WriteLine("add {product Name}");
            Console.WriteLine("order {product Name} {quantity}");
            Console.WriteLine("show what is available");
            Console.WriteLine("see the price of {product Name}");
            Console.WriteLine("-------------------------------------------------");
        }

        static void showWhatsIsAvailable(Dictionary<string, double> productNameAndPrice)
        {
            Console.WriteLine("-----------------------");
            foreach (var pair in productNameAndPrice)
            {
                Console.WriteLine($"{pair.Key} - {pair.Value}$");              
            }
            Console.WriteLine("-----------------------");
        }
    }
}
