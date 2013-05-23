using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace TankWar.Content
{
    /// <summary>
    /// Summary description for GetNameImage
    /// </summary>
    public class GetNameImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var name = context.Request.QueryString["text"];
            if (name == null)
            {
                name = "_";
            }

            name = name.PadRight(40, ' '); // sprite doesn't repeat

            var font = new Font("Courier New", 10);
            var image = DrawText(name, font, Color.Black, Color.White);
            var memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Gif);

            context.Response.ContentType = "image/gif";
            context.Response.BinaryWrite(memoryStream.ToArray());
        }

        private Image DrawText(String text, Font font, Color textColor, Color backColor)
        {
            //first, create a dummy bitmap just to get a graphics object
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)textSize.Width, (int)textSize.Height);

            drawing = Graphics.FromImage(img);

            //paint the background
            drawing.Clear(backColor);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, 0, 0);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            return img;

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}