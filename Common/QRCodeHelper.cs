using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class QRCodeHelper
    {
        public static Bitmap  GetQRCodeImg(string text) {
            // 生成二维码的内容
            string strCode = text;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(strCode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrcode = new QRCode(qrCodeData);

            // qrcode.GetGraphic 方法可参考最下发“补充说明”
            Bitmap qrCodeImage = qrcode.GetGraphic(5, Color.Black, Color.White, null, 15, 6, false);
            return qrCodeImage;
        }

        public static MemoryStream GetQRCodeStream(string text)
        {
            Bitmap qrCodeImage = GetQRCodeImg(text);
            MemoryStream ms = new MemoryStream();
            qrCodeImage.Save(ms, ImageFormat.Jpeg);
            return ms;
        }

        public static byte[] GetQRCodeByteArray(string text)
        {
            using (MemoryStream ms = GetQRCodeStream(text))
            {
                return ms.ToArray();
            }
        }
    }
}
