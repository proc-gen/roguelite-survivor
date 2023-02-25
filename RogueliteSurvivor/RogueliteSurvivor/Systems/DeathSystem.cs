using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Dynamics.Bodies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueliteSurvivor.ComponentFactories;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using RogueliteSurvivor.Containers;
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
        QueryDescription singleTargetQuery = new QueryDescription()
                                            .WithAll<SingleTarget>();

        Dictionary<string, Texture2D> textures;
        Box2D.NetStandard.Dynamics.World.World physicsWorld;
        Dictionary<Spells, SpellContainer> spellContainers;
        Random random;
        public DeathSystem(World world, Dictionary<string, Texture2D> textures, Box2D.NetStandard.Dynamics.World.World physicsWorld, Dictionary<Spells, SpellContainer> spellContainers)
            : base(world, new QueryDescription())
        { 
            this.textures = textures;
            this.physicsWorld = physicsWorld;
            this.spellContainers = spellContainers;
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
                    Spells spell = spriteSheet.TextureName.GetSpellFromString();
                    animation = SpellFactory.GetSpellHitAnimation(spellContainers[spell]);
                    spriteSheet = SpellFactory.GetSpellHitSpriteSheet(textures, spellContainers[spell], spriteSheet.Rotation); 
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

            world.Query(in singleTargetQuery, (ref SingleTarget single, ref Animation animation, ref Body body) =>
            {
                if (single.State == EntityState.Alive && single.DamageEndDelay < 0)
                {
                    single.State = EntityState.Dying;
                    physicsWorld.DestroyBody(body);
                }
                else if (single.State == EntityState.Dying)
                {
                    if (animation.CurrentFrame == animation.LastFrame)
                    {
                        single.State = EntityState.Dead;
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
    }
}
