using Arch.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueliteSurvivor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Systems
{
    public class EnemySpawnSystem : ArchSystem, IUpdateSystem
    {
        QueryDescription playerQuery = new QueryDescription()
                                            .WithAll<Player, Position>();
        Dictionary<string, Texture2D> textures;
        Random random;
        public EnemySpawnSystem(World world, Dictionary<string, Texture2D> textures)
            : base(world, new QueryDescription()
                                .WithAll<Enemy>())
        { 
            this.textures = textures;
            random = new Random();
        }

        public void Update(GameTime gameTime) 
        {
            int numEnemies = 0;
            world.Query(in query, (ref Enemy enemy) =>
            {
                numEnemies++;
            });

            if(numEnemies < 20)
            {
                for(int i = numEnemies; i < 20; i++)
                {
                    world.Create(
                        new Enemy(),
                        new Position() { XY = new Vector2(random.Next(32, 768), random.Next(32, 768)) },
                        new Velocity() { Direction = Vector2.Zero },
                        new Speed() { speed = 75f },
                        new Animation(0, 3, .1f, 2),
                        new SpriteSheet(textures["vampire_bat"], "vampire_bat", 4, 2),
                        new Collider() { Width = 16, Height = 16, Offset = new Vector2(8, 8) }
                    );
                }
            }
        }
    }
}
