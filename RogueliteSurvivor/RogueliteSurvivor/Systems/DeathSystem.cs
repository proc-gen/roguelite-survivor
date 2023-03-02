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

namespace RogueliteSurvivor.Systems
{
    public class DeathSystem : ArchSystem, IUpdateSystem
    {
        QueryDescription singleTargetQuery = new QueryDescription()
                                            .WithAll<SingleTarget>();

        QueryDescription projectileQuery = new QueryDescription()
                                            .WithAll<Projectile>();

        QueryDescription enemyQuery = new QueryDescription()
                                            .WithAll<Enemy>();

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
            world.Query(in projectileQuery, (in Entity entity, ref EntityStatus entityStatus, ref SpriteSheet spriteSheet, ref Animation animation, ref Body body) =>
            {
                if (entityStatus.State == State.ReadyToDie)
                {
                    entityStatus.State = State.Dying;
                    physicsWorld.DestroyBody(body);

                    Spells spell = spriteSheet.TextureName.GetSpellFromString();
                    animation = SpellFactory.GetSpellHitAnimation(spellContainers[spell]);
                    spriteSheet = SpellFactory.GetSpellHitSpriteSheet(textures, spellContainers[spell], spriteSheet.Rotation);
                    
                }
                else if (entityStatus.State == State.Dying && animation.CurrentFrame == animation.LastFrame)
                {
                    entityStatus.State = State.Dead;
                }
            });

            world.Query(in enemyQuery, (in Entity entity, ref EntityStatus entityStatus, ref SpriteSheet spriteSheet, ref Animation animation, ref Body body) =>
            {
                if (entityStatus.State == State.ReadyToDie)
                {
                    entityStatus.State = State.Dying;
                    physicsWorld.DestroyBody(body);
                    
                    int bloodToUse = random.Next(1, 9);
                    spriteSheet = new SpriteSheet(textures["MiniBlood" + bloodToUse], "MiniBlood" + bloodToUse, getMiniBloodNumFrames(bloodToUse), 1, 0, spriteSheet.Width == 16 ? .5f : 1f);
                    animation = new Animation(0, getMiniBloodNumFrames(bloodToUse) - 1, 1 / 60f, 1, false);
                    
                }
                else if (entityStatus.State == State.Dying && animation.CurrentFrame == animation.LastFrame)
                {
                    entityStatus.State = State.Dead;
                }
            });

            world.Query(in singleTargetQuery, (ref EntityStatus entityStatus, ref SingleTarget single, ref Animation animation, ref Body body) =>
            {
                if (entityStatus.State == State.Alive && single.DamageEndDelay < 0)
                {
                    entityStatus.State = State.Dying;
                    physicsWorld.DestroyBody(body);
                }
                else if (entityStatus.State == State.Dying
                            && animation.CurrentFrame == animation.LastFrame)
                {
                    entityStatus.State = State.Dead;

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
