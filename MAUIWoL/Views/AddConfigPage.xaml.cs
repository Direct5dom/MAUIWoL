using MAUIWoL.Data;
using MAUIWoL.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Sockets;
using System.Net;

namespace MAUIWoL.Views;

[QueryProperty("Item", "Item")]

public partial class AddConfigPage : ContentPage
{
    WoLConfig item;
    public WoLConfig Item
    {
        get => BindingContext as WoLConfig;
        set => BindingContext = value;
    }
    WoLConfigDatabase database;
    public AddConfigPage(WoLConfigDatabase wolConfigDatabase)
	{
		InitializeComponent();
        database = wolConfigDatabase;
    }
    async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Item.MACAddress))
        {
            await DisplayAlert("配置内容有缺失", "请填写所有必填选项。", "确定");
            return;
        }

        await database.SaveItemAsync(Item);
        await Shell.Current.GoToAsync("..");
    }

    async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (Item.ID == 0)
            return;
        await database.DeleteItemAsync(Item);
        await Shell.Current.GoToAsync("..");
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
    void macAddressOnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
    }

    void ipAddressOnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
    }

    void ipPortOnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
    }

    void macAddressOnEntryCompleted(object sender, EventArgs e)
    {
    }

    void ipAddressOnEntryCompleted(object sender, EventArgs e)
    {
    }

    void ipPortOnEntryCompleted(object sender, EventArgs e)
    {
    }

    void WoLOnButtonClicked(object sender, EventArgs args)
    {
        WoLPC();
    }

    // 根据配置文件，调用发送MagicPacket
    //private void WoLPC(string ConfigIDNum)
    private void WoLPC()
    {
        // 读取localSettings中的字符串
        //string configInner = localSettings.Values["ConfigID" + ConfigIDNum] as string;
        string configInner = "configName.Text" + "," + macAddress.Text + "," + ipAddress.Text + "," + ipPort.Text;
        // 如果字符串非空
        if (configInner != null)
        {
            // 分割字符串
            string[] configInnerSplit = configInner.Split(',');
            // configName.Text + "," + macAddress.Text + "," + ipAddress.Text + "," + ipPort.Text;
            string macAddress = configInnerSplit[1];
            string ipAddress = configInnerSplit[2];
            string ipPort = configInnerSplit[3];

            // 尝试发送Magic Packet，成功打开已发送弹窗
            try
            {
                sendMagicPacket(macAddress, ipAddress, int.Parse(ipPort));
                //MagicPacketIsSendTips.IsOpen = true;
            }
            // 失败打开发送失败弹窗
            catch
            {
                //MagicPacketNotSendTips.IsOpen = true;
            }
        }
    }
    // 以UDP协议发送MagicPacket
    public void sendMagicPacket(string macAddress, string domain, int port)
    {
        // 将传入的Mac地址字符串分割为十六进制字符串数组
        // hexStrings = {"11", "22", "33", "44", "55", "66"}
        string s = macAddress;
        string[] hexStrings = s.Split(':');

        // 创建一个byte数组
        byte[] bytes = new byte[hexStrings.Length];
        // 遍历字符串数组，将每个字符串转换为byte值，并存储到byte数组中
        for (int i = 0; i < hexStrings.Length; i++)
        {
            // 使用16作为基数表示十六进制格式
            bytes[i] = Convert.ToByte(hexStrings[i], 16);
        }
        // 将MAC地址转换为字节数组：byte[] mac = new byte[] { 0x11, 0x22, 0x33, 0x44, 0x55, 0x66 };
        byte[] mac = bytes;

        // 创建一个UDP Socket对象
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        // 设置需要广播数据
        socket.EnableBroadcast = true;

        // 创建一个魔术包
        byte[] packet = new byte[17 * 6];
        // 填充前6个字节为0xFF
        for (int i = 0; i < 6; i++)
            packet[i] = 0xFF;
        // 填充后面16个重复的MAC地址字节
        for (int i = 1; i <= 16; i++)
            for (int j = 0; j < 6; j++)
                packet[i * 6 + j] = mac[j];

        // 获取IP地址
        IPAddress ip = domain2ip(domain);

        // 多次发送，避免丢包
        for (int i = 0; i < 10; i++)
        {
            // 发送数据
            socket.SendTo(packet, new IPEndPoint(ip, port));
            Test.Text = macAddress.ToString() + ',' + domain.ToString() + ',' + port.ToString();
        }

        // 关闭Socket对象
        socket.Close();
    }
    static IPAddress domain2ip(string domain)
    {
        // 此函数本身可以处理部分非法IP（例如：266.266.266.266）
        // 这些非法IP会被算作域名来处理
        IPAddress ipAddress;
        if (IPAddress.TryParse(domain, out ipAddress))
        {
            // 是IP
            return IPAddress.Parse(domain);
        }
        else
        {
            // 是域名或其他输入
            return Dns.GetHostEntry(domain).AddressList[0];
        }
    }
}