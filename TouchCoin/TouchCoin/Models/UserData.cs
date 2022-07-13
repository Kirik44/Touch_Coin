namespace TouchCoin.Models
{
    public class UserData
    {
        public int Id { get; set; }
        public string NikName { get; set; }
        public string Pass { get; set; }
        public ulong Coin { get; set; }
        public byte LvlClick { get; set; }
        public byte LvlMining1 { get; set; }
        public byte LvlMining2 { get; set; }
        public byte LvlMining3 { get; set; }
        public byte LvlMining4 { get; set; }
        public byte LvlMining5 { get; set; }
        public byte LvlMining6 { get; set; }
    }
}
