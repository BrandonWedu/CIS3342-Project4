using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;
using System.Text;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Formats.Jpeg;
using static System.Net.Mime.MediaTypeNames;
using SixLabors.ImageSharp.PixelFormats;

namespace Project4.Models
{
    public class ModifyImage
    {
        private byte[] image;
        public ModifyImage() { }

        public byte[] Image
        {
            get { return image; }
            set { image = value; }
        }
        public void AddWatermark(string companyName)
        {
            if (image == null) { return; }
			using (SixLabors.ImageSharp.Image img = SixLabors.ImageSharp.Image.Load(image))
			{
				//Font
				SixLabors.Fonts.Font font = SystemFonts.CreateFont("Arial", 40, FontStyle.Regular);

				//Color of text
				Color watermarkColor = Color.White.WithAlpha(0.8f);

				// Position for the watermark
				// Bottom Right
				PointF position = new PointF(img.Width - 200, img.Height - 60);

				// Add watermark to image
				img.Mutate(new Action<IImageProcessingContext>(delegate (IImageProcessingContext ctx)
				{
					ctx.DrawText(companyName, font, watermarkColor, position);
				}));

				using (MemoryStream memoryStream = new MemoryStream())
				{
					img.Save(memoryStream, new PngEncoder());
					image = memoryStream.ToArray(); 
				}
			}
		}
        public void Compress()
        {
            if (image == null) { return; }
			using (SixLabors.ImageSharp.Image img = SixLabors.ImageSharp.Image.Load(image))
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					PngEncoder pngEncoder = new PngEncoder()
					{
						CompressionLevel = PngCompressionLevel.BestCompression
					};
					img.Save(memoryStream, pngEncoder);
					image = memoryStream.ToArray();
				}
            }
		}
		public void Resize()
		{
			if (image == null) { return; }
			SixLabors.ImageSharp.Image img = SixLabors.ImageSharp.Image.Load(image);

			//Calc aspect
			float current = img.Height / (float)img.Width;

			//1:1 square
			float target = 9f / 9f;

			int newWidth = img.Width;
			int newHeight = img.Height;

			//Adjust based on aspect ratio
			if (current > target)
			{
				newHeight = (int)(img.Width * target);
				int yOffset = (img.Height - newHeight) / 2;

				//Black bars if image needs them 
				var finalImage = new SixLabors.ImageSharp.Image<Rgba32>(newWidth, newHeight, Color.Black);

				//Crop the image
				img.Mutate(delegate (IImageProcessingContext x)
				{
					x.Crop(new Rectangle(0, yOffset, img.Width, newHeight));
				});

				// Draw the cropped image onto the black canvas
				finalImage.Mutate(delegate (IImageProcessingContext x)
				{
					x.DrawImage(img, new Point(0, 0), 1f);
				});

				image = PngToByteArray(finalImage);
			}
			else if (current < target)
			{
				newWidth = (int)(img.Height * target);
				int xOffset = (img.Width - newWidth) / 2;

				var finalImage = new SixLabors.ImageSharp.Image<Rgba32>(newWidth, newHeight, Color.Black);

				img.Mutate(delegate (IImageProcessingContext x)
				{
					x.Crop(new Rectangle(xOffset, 0, newWidth, img.Height));
				});

				finalImage.Mutate(delegate (IImageProcessingContext x)
				{
					x.DrawImage(img, new Point(0, 0), 1f);
				});

				image = PngToByteArray(finalImage);
			}
			else
			{
				image = PngToByteArray(img);
			}
		}



		private byte[] PngToByteArray(SixLabors.ImageSharp.Image img)
        {
			using (MemoryStream memoryStream = new MemoryStream())
			{
				img.Save(memoryStream, PngFormat.Instance);
				return memoryStream.ToArray(); // Return the byte array
			}
		}
    }
}
