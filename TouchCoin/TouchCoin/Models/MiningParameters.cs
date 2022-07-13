using System.Collections.Generic;

namespace TouchCoin.Models
{
    public class MiningParameters
    {
        public List<Parameter> Parameter { get; private set; }
        public double FactorCost { get; private set; }

        public MiningParameters()
        {
            Parameter = new List<Parameter>()
            {
                new Parameter() { Id = 0, Name = "Курсор", DefaultCost = 100, MaxQuantity = 50, Profitability = 1, ImgName = "One"},
                new Parameter() { Id = 1, Name = "Видеокарта", DefaultCost = 100, MaxQuantity = 50, Profitability = 3, ImgName = "Two" },
                new Parameter() { Id = 2, Name = "Стойка видеокарт", DefaultCost = 1000, MaxQuantity = 50, Profitability = 10, ImgName = "Three" },
                new Parameter() { Id = 3, Name = "Сервер", DefaultCost = 10000, MaxQuantity = 50, Profitability = 30, ImgName = "Four" },
                new Parameter() { Id = 4, Name = "Суперкомпьютер", DefaultCost = 50000, MaxQuantity = 50, Profitability = 100, ImgName = "Five" },
                new Parameter() { Id = 5, Name = "Датацентр", DefaultCost = 200000, MaxQuantity = 50, Profitability = 500, ImgName = "Six" },
                new Parameter() { Id = 6, Name = "Квантовый компьютер", DefaultCost = 500000, MaxQuantity = 50, Profitability = 1000, ImgName = "Seven" }
            };

            FactorCost = 1.3;
        }
    }

    public class Parameter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DefaultCost { get; set; }
        public int Profitability { get; set; }
        public byte MaxQuantity { get; set; }
        public string ImgName { get; set; }
    }
}
