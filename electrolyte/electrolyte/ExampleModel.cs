using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace electrolyte
{
    class ExampleModel : Model
    {
        public ExampleModel(Game game,
                            Pointf position,
                            Size size,
                            Velocity velocity) :
            base(game, position, size, velocity, 0)
        {
            //  how you would move
            mPosition.X = 40;
            Velocity.Speed = new Vector2(1,1);
            Velocity.Direction = new Vector2(2, 0);
        }


        protected override Texture2D LoadTexture()
        {
            SpriteBatch batch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            Texture2D rectangleTexture = new Texture2D(batch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            rectangleTexture.SetData(new[] { Color.Wheat });
            return rectangleTexture;
        }

        public override void Update(GameTime gameTime)
        {
            // here you can update your character...or handle keystates
            // to change a frame, you would set the mCurrentFrameIndex
            base.Update(gameTime);
        }

        protected override void GenerateFrames()
        {
            // here you would generate the frames for your model by adding frame object to the list.
            mFrames.Add(new Frame(new Rectangle(0, 0, 20, 20)));
        }
    }
}
