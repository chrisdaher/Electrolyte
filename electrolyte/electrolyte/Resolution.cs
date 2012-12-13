using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace electrolyte
{
  public enum ScreenMode
  {
    // 640x480
    tv480i,
    // 854x480
    tv480p,
    // 800x600
    tv800,
    // 1280x720
    tv720p,
    // 1920x1080
    tv1080p
  }

  public class Resolution
  {
    public static int resWidth = 1280;//1960;
    public static int resHeight = 720;//1080;
    private int screenWidth;
    private int screenHeight;
    private Viewport viewport = new Viewport();
    private bool widescreen;
    private Matrix scale;
    private ScreenMode currentMode;
    private ScreenMode baseMode;

    private static readonly int[,] resolutions =
    new int[,] {
        {640, 480},
        {854, 480}, // widescreen tv480
        {800, 600}, // widescreen tv480
        {1280, 720},
        {1920, 1080}
        };

    private void UpdateScale()
    {
      scale =
              Matrix.CreateScale(
              (float)viewport.Width / (float)resolutions[(int)baseMode, 0],
              (float)viewport.Height / (float)resolutions[(int)baseMode, 1],
              1f);
    }

    public Viewport ScreenViewport
    {
      get { return viewport; }
      set
      {
        viewport = value;
        UpdateScale();
      }
    }

    public int ScreenWidth { get { return screenWidth; } }
    public int ScreenHeight { get { return screenHeight; } }
    public bool WideScreen { get { return widescreen; } }

    public int VirtualWidth { get { return resolutions[(int)baseMode, 0]; } }
    public int VirtualHeight { get { return resolutions[(int)baseMode, 1]; } }
    public float AspectRatio { get { return (float)screenWidth / screenHeight; } }

    public Matrix Scale { get { return this.scale; } }
    public Vector2 Scale2D { get { return new Vector2(scale.M11, scale.M22); } }

    public ScreenMode BaseMode
    {
      get { return this.baseMode; }
      set
      {
        this.baseMode = value;
        UpdateScale();
      }
    }

    public ScreenMode Mode
    {
      get { return currentMode; }
      set
            {
                currentMode = value;
                screenWidth = resolutions[(int)currentMode, 0];
                screenHeight = resolutions[(int)currentMode, 1];
                if (currentMode >= ScreenMode.tv480p)
                    this.widescreen = true;
                else
                    this.widescreen = false;
            }
    }

    public Resolution(GraphicsDeviceManager graphics, ScreenMode virtualMode, ScreenMode mode)
    {
      this.Mode = mode;
      this.baseMode = virtualMode;

      viewport.X = 0;
      viewport.Y = 0;
      viewport.Width = screenWidth;
      viewport.Height = screenHeight;

      SetResolution(graphics);
    }

    public void SetResolution(GraphicsDeviceManager graphics)
    {
      graphics.PreferredBackBufferWidth = resWidth;//screenWidth;
      graphics.PreferredBackBufferHeight = resHeight;//screenHeight;
      UpdateScale();

      graphics.ApplyChanges();
    }
  }
}