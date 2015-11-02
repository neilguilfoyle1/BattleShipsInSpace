using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Assignment2C_sharp
{
    public class Animation
    {       
        int startFrame;
        int width;
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        int height;
        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        Vector2 origin;
        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public int StartFrame
        {
            get { return startFrame; }
            set { startFrame = value; }
        }
        int endFrame;

        public int EndFrame
        {
            get { return endFrame; }
            set { endFrame = value; }
        }

        int spacingX;

        public int SpacingX
        {
            get { return spacingX; }
            set { spacingX = value; }
        }

        public Animation(Vector2 origin, int width, int height, int spacingX, int startFrame, int endFrame)
        {
            this.origin = origin;
            this.width = width;
            this.height = height;
            this.spacingX = spacingX;
            this.startFrame = startFrame;
            this.endFrame = endFrame;
        }

        /// <summary>
        /// The constructor to make a new animation, takes in all data needed
        /// </summary>
        /// <param name="origin">Where on the sprite(picture) the top corner is for your frame(single piece)</param>
        /// <param name="width">Width of the frame(piece) you want from the big picture you use</param>
        /// <param name="height">Height of the frame(piecE) you want from the big picture you use</param>
        /// <param name="startFrame">starting number of those frames(pieces) in a sequence, 0 for just the one</param>
        /// <param name="endFrame">last number of those frames(pieces) in a sequence, 0 for just the one</param>
        public Animation(Vector2 origin, int width, int height, int startFrame, int endFrame):this(origin, width, height, 0, startFrame, endFrame)
        {
            
        }
    }
}
