﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using WowCyborg.Models;
using WowCyborg.Utilities;

namespace WowCyborg.Handlers
{
    public static class AddonLocator
    {
        private static Rectangle _inGameAddonLocation;

        public static void SetInGameAddonLocation(Rectangle rect)
            => _inGameAddonLocation = rect;
        
        public static Rectangle GetAddonLocation()
        {
            if (_inGameAddonLocation == Rectangle.Empty)
            {
                ReCalculateAddonPosition();
            }

            return _inGameAddonLocation;
        }

        public static void ReCalculateAddonPosition()
        {
            Bitmap clone;
            var bounds = Screen.GetBounds(Point.Empty);
            var bottomLeft = new Rectangle(0, Screen.PrimaryScreen.Bounds.Height - 100, 500, 100);
            
            using (var bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }
                clone = bitmap.Clone(bottomLeft, PixelFormat.Format24bppRgb);
            }
            Console.WriteLine("Image width: " + clone.Width);
            var frameSize = CalculateFrameWidth(clone);
            Console.WriteLine("Frame width: " + frameSize);
            var settings = SettingsLoader.LoadSettings<AppSettings>("settings.json");

            _inGameAddonLocation = new Rectangle(0,
                Screen.PrimaryScreen.Bounds.Height - frameSize * settings.AddonRowCount,
                frameSize * settings.AddonColumnCount,
                frameSize * settings.AddonRowCount);

            if (_inGameAddonLocation.Height == 0 || _inGameAddonLocation.Width == 0)
            {
                _inGameAddonLocation = new Rectangle(1, 1, 1, 1);
            }
        }

        private static int CalculateFrameWidth(Bitmap b)
        {
            var width = 0;
            Color firstPixel = Color.White;

            for (var x = 0; x < b.Width; x++)
            {
                if (firstPixel == Color.White)
                {
                    firstPixel = b.GetPixel(x, b.Height - 5);
                    continue;
                }

                if (firstPixel != b.GetPixel(x, b.Height - 5))
                {
                    width = x;
                    break;
                }
            }
            return width / 4;
        }
    }
}