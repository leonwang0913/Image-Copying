using System;
using System.Drawing;
using static System.Console;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace pa2
{
	class Retina
	{
		public class Tile
		{
			public const int Len = 8; //setting constant len integer
			
			public Color [,] Pixels {get; set;} //declaring new property of 2d array pixels
			
			public Tile()
			{
				Pixels = new Color [Len, Len]; //setting pixels as a 2d array of size len by len
			}
			
		}	
		//Properties
		public Tile[,] Tiles {get; set;}
		public int Height {get; private set;} //can't set height
		public int Width {get; private set;} //can't set width
		
		public Retina(string path)
		{
			Image <Rgba32> img6L = Image.Load<Rgba32>(path); //creating new image variable that stores retina picture
			
			//calculating width and height
			Height = (int) Math.Ceiling((double) img6L.Height / Tile.Len);
			Width = (int) Math.Ceiling((double) img6L.Width /  Tile.Len);
			
			Tiles = new Tile[Height, Width]; //setting tile with dimension height by width
			
			//running through two for loops to create each tile
			for (int tileRow = 0; tileRow < Width; tileRow++)
			{
				for (int tileCol = 0; tileCol < Height; tileCol++)
				{
					Tiles[tileRow, tileCol] = new Tile(); //declaring new tile
					
					for (int row = 0; row < Tile.Len; row++)
					{
						for (int col = 0; col < Tile.Len; col++)
						{
							//if tilerow multiplied by length of tile plus the row is greater or equal to image height
							//or tilecol multiplied by width of tile plus the col is greater or equal to image width
							if (tileRow* Tile.Len + row >= img6L.Height || tileCol * Tile.Len + col >= img6L.Width)
							{
								Tiles [tileRow, tileCol].Pixels[row, col] = Color.FromArgb(255, 0, 0, 0); //drawing the pixel
							}
							else 
							{
								Rgba32 p = img6L[tileRow*Tile.Len + row, tileCol * Tile.Len + col]; //setting rgba32 p variable to image's specific pixel
								Color c = Color.FromArgb(p.A, p.R, p.G, p.B); //creating new color c variable
								
								Tiles [tileRow, tileCol].Pixels[row,col] = c;  //tile's pixel gets that of p's color pattern
							}
						}
					}
				}
			}
		}
		public void SaveToFile(string path) 
		{
			Image<Rgba32> img6L = new Image<Rgba32> (Width*Tile.Len, Height*Tile.Len); //creating new img6l variable with parameters
			
			//Nested for loops to go through each pixel
			for (int x = 0; x < Width; x++) 
			{
				for (int y = 0; y < Height; y++)
				{
					Color [,] c = Tiles[x,y].Pixels; //setting new color 2d array c 
					//Nested for loop to go set color 
					for (int k = 0; k < Tile.Len; k++)
					{
						for (int l = 0; l < Tile.Len; l++)
						{
							Rgba32 p = new Rgba32 (c[k,l].R, c[k,l].G, c[k,l].B, c[k,l].A); //p variable that gets the color of c at (k,l)
							//creating var types to let compiler find out type of posY/posX
							var posY = x*Tile.Len+k; //setting new variable posY as x * length of tile added with k
							var posX = y*Tile.Len+l; //setting new variable posX as y * length of tile added with l
							img6L[posY,posX] = p; //pixel at (posY, posX) has p variable colour pattern
						}
					}
				}
			}
			img6L.Save(path); //saving image
		}
	}
    static class Program
    {
        static void Main()
        { 
			const string path = "20051020_63711_0100_PP.png";
			
			Retina retina = new Retina (path);
			retina.SaveToFile("test.png");
        }
    }
}
