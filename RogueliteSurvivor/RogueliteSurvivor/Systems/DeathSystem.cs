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
            world.Query(in projectileQuery, (ref Projectile projectile, ref SpriteSheet spriteSheet, ref Animation animation, ref Body body) =>
            {
                if (projectile.State == EntityState.ReadyToDie)
                {
                    projectile.State = EntityState.Dying;
                    physicsWorld.DestroyBody(body);

                    animation = new Animation(0, getProjectileHitNumFrames(spriteSheet.TextureName) - 1, .03f, 1, false);
                    spriteSheet = new SpriteSheet(textures[spriteSheet.TextureName + "Hit"], spriteSheet.TextureName + "Hit", getProjectileHitNumFrames(spriteSheet.TextureName), 1, spriteSheet.Rotation, .5f);
                }
                else if (projectile.State == EntityState.Dying)
                {
                    if (animation.CurrentFrame == animation.LastFrame)
                    {
                        projectile.State = EntityState.Dead;
                    }
                }
            });

            world.Query(in enemyQuery, (ref Enemy enemy, ref SpriteSheet spriteSheet, ref Animation animation, ref Body body) =>
            {
                if(enemy.State == EntityState.ReadyToDie)
                {
                    enemy.State = EntityState.Dying;
                    physicsWorld.DestroyBody(body);
                    int bloodToUse = random.Next(1, 9);
                    spriteSheet = new SpriteSheet(textures["MiniBlood" + bloodToUse], "MiniBlood" + bloodToUse, getMiniBloodNumFrames(bloodToUse), 1, 0, spriteSheet.Width == 16 ? .5f : 1f);
                    animation = new Animation(0, getMiniBloodNumFrames(bloodToUse) - 1, 1 / 60f, 1, false);
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

        private int getMiniBloodNumFrames(int bloodToUse)
        {
            int retVal = 0;
            switch (bloodToUse)
            {
                case 1:
                case 2:
                case 6:
                case 7:
                    retVal = 30;
                    break;
                case 3:
                case 4:
                case 5:
                    retVal = 28;
                    break;
                case 8:
                    retVal = 29;
                    break;
                case 9:
                    retVal = 22;
                    break;
            }
            return retVal;
        }

        private int getProjectileHitNumFrames(string projectile)
        {
            int retVal = 0;

            switch (projectile)
            {
                case "IceShard":
                    retVal = 7;
                    break;
                case "LightningBlast":
                    retVal = 5;
                    break;
                case "Fireball":
                    retVal = 6;
                    break;
            }

            return retVal;
        }
    }
}
