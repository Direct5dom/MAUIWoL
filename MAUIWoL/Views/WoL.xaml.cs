using System.Net.Mail;
using System.Net;
using System.Net.Sockets;

namespace MAUIWoL.Views;

public partial class WoL : ContentPage
{
    public WoL()
    {
        InitializeComponent();
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

    // ���������ļ������÷���MagicPacket
    //private void WoLPC(string ConfigIDNum)
    private void WoLPC()
    {
        // ��ȡlocalSettings�е��ַ���
        //string configInner = localSettings.Values["ConfigID" + ConfigIDNum] as string;
        string configInner = "configName.Text" + "," + macAddress.Text + "," + ipAddress.Text + "," + ipPort.Text;
        // ����ַ����ǿ�
        if (configInner != null)
        {
            // �ָ��ַ���
            string[] configInnerSplit = configInner.Split(',');
            // configName.Text + "," + macAddress.Text + "," + ipAddress.Text + "," + ipPort.Text;
            string macAddress = configInnerSplit[1];
            string ipAddress = configInnerSplit[2];
            string ipPort = configInnerSplit[3];

            // ���Է���Magic Packet���ɹ����ѷ��͵���
            try
            {
                sendMagicPacket(macAddress, ipAddress, int.Parse(ipPort));
                //MagicPacketIsSendTips.IsOpen = true;
            }
            // ʧ�ܴ򿪷���ʧ�ܵ���
            catch
            {
                //MagicPacketNotSendTips.IsOpen = true;
            }
        }
    }
    // ��UDPЭ�鷢��MagicPacket
    public void sendMagicPacket(string macAddress, string domain, int port)
    {
        // �������Mac��ַ�ַ����ָ�Ϊʮ�������ַ�������
        // hexStrings = {"11", "22", "33", "44", "55", "66"}
        string s = macAddress;
        string[] hexStrings = s.Split(':');

        // ����һ��byte����
        byte[] bytes = new byte[hexStrings.Length];
        // �����ַ������飬��ÿ���ַ���ת��Ϊbyteֵ�����洢��byte������
        for (int i = 0; i < hexStrings.Length; i++)
        {
            // ʹ��16��Ϊ������ʾʮ�����Ƹ�ʽ
            bytes[i] = Convert.ToByte(hexStrings[i], 16);
        }
        // ��MAC��ַת��Ϊ�ֽ����飺byte[] mac = new byte[] { 0x11, 0x22, 0x33, 0x44, 0x55, 0x66 };
        byte[] mac = bytes;

        // ����һ��UDP Socket����
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        // ������Ҫ�㲥����
        socket.EnableBroadcast = true;

        // ����һ��ħ����
        byte[] packet = new byte[17 * 6];
        // ���ǰ6���ֽ�Ϊ0xFF
        for (int i = 0; i < 6; i++)
            packet[i] = 0xFF;
        // ������16���ظ���MAC��ַ�ֽ�
        for (int i = 1; i <= 16; i++)
            for (int j = 0; j < 6; j++)
                packet[i * 6 + j] = mac[j];

        // ��ȡIP��ַ
        IPAddress ip = domain2ip(domain);

        // ��η��ͣ����ⶪ��
        for (int i = 0; i < 10; i++)
        {
            // ��������
            socket.SendTo(packet, new IPEndPoint(ip, port));
            Test.Text = macAddress.ToString() + ',' + domain.ToString() + ',' + port.ToString();
        }

        // �ر�Socket����
        socket.Close();
    }
    static IPAddress domain2ip(string domain)
    {
        // �˺���������Դ����ַǷ�IP�����磺266.266.266.266��
        // ��Щ�Ƿ�IP�ᱻ��������������
        IPAddress ipAddress;
        if (IPAddress.TryParse(domain, out ipAddress))
        {
            // ��IP
            return IPAddress.Parse(domain);
        }
        else
        {
            // ����������������
            return Dns.GetHostEntry(domain).AddressList[0];
        }
    }
}