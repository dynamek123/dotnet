namespace ProjektSklepGryWideo.Models
{


    public class OrderItem
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } // Assuming Price is a decimal in Game model

        public Game Game { get; set; } // Changed from object? to Game
    }

}
