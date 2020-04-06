using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace IK_SE
{
    class recv_thread
    {
        byte[] data = new byte[1024];
        string stringData;
        public TcpClient WiFly_address;
        public NetworkStream ns;
        public Form1 form;
        int num_of_bytes_recvd;

        public void recv()
        {
            ns = WiFly_address.GetStream();
            while (true)
            {
                try
                {
                    num_of_bytes_recvd = ns.Read(data, 0, data.Length);
                    stringData = Encoding.ASCII.GetString(data, 0, num_of_bytes_recvd);
                    form.PrintOut(stringData);
                }
                catch (Exception)
                {

                }
                System.Threading.Thread.Sleep(1);
            }

        }

    }
}
