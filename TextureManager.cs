using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace Sokoban
{

    public class TextureManager
    {
        public Texture2D wallTexture, boxTexture, targetTexture, floorTexture, playerTexture, boxDocked;

        public SpriteFont font;
        public void LoadTexture(ContentManager content)
        {
    
            wallTexture = content.Load<Texture2D>("wall");
            boxTexture = content.Load<Texture2D>("box");
            targetTexture = content.Load<Texture2D>("target");
            floorTexture = content.Load<Texture2D>("floor");
            playerTexture = content.Load<Texture2D>("player");
            boxDocked = content.Load<Texture2D>("box-docked");
            font = content.Load<SpriteFont>("DefaultFont");
        }

        public Texture2D GetTextureForTile(TileType tile)
        {
            return tile switch
            {
                TileType.Wall => wallTexture,
                TileType.Box => boxTexture,
                TileType.Target => targetTexture,
                TileType.Player => playerTexture,
                TileType.BoxDocked => boxDocked,
                _ => floorTexture,
            };
        }
        public SpriteFont GetFont()
        {
            return font;
        }
    }
}    

