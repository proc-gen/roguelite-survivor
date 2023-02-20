using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Dynamics.Bodies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Systems
{
    public class DeathSystem : ArchSystem, IUpdateSystem
    {
        QueryDescription projectileQuery = new QueryDescription()
                                            .WithAll<Projectile>();
        QueryDescription enemyQuery = new QueryDescription()
                                            .WithAll<Enemy>();
        Dictionary<string, Texture2D> textures;
        Box2D.NetStandard.Dynamics.World.World physicsWorld;
        Random random;
        public DeathSystem(World world, Dictionary<string, Texture2D> textures, Box2D.NetStandard.Dynamics.World.World physicsWorld)
            : base(world, new QueryDescription())
        { 
            this.textures = textures;
            this.physicsWorld = physicsWorld;
            random = new Random();
        }

        public void Update(GameTime gameTime, float totalElapsedTime) 
        {
            world.Query(in projectileQuery, (ref Projectile projectile, ref SpriteSheet spriteSheet, ref Animation animation) =>
            {
                             
            });

            world.Query(in enemyQuery, (ref Enemy enemy, ref SpriteSheet spriteSheet, ref Animation animation, ref Body body) =>
            {
                if(enemy.State == EntityState.ReadyToDie)
                {
                    enemy.State = EntityState.Dying;
                    physicsWorld.DestroyBody(body);
                    spriteSheet = new SpriteSheet(textures["MiniBlood1"], "MiniBlood1", 30, 1, 0, .5f);
                    animation = new Animation(0, 30, 1 / 60f, 1, false);
                }
                else if(enemy.State == EntityState.Dying)
                {
                    if(animation.CurrentFrame == animation.LastFrame)
                    {
                        enemy.State = EntityState.Dead;
                    }
                }
            });
        }
    }
}
