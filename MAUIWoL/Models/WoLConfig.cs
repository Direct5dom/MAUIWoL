using SQLite;

namespace MAUIWoL.Models;

public class WoLConfig
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string MACAddress { get; set; }
    public string IPAddress { get; set; }
    public string IPPort { get; set; }
    //public bool Done { get; set; }
}
