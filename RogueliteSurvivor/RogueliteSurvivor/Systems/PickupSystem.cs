using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using RogueliteSurvivor.Extensions;
using RogueliteSurvivor.Helpers;

namespace RogueliteSurvivor.Systems
{
    public class PickupSystem : ArchSystem, IUpdateSystem
    {
        QueryDescription playerQuery = new QueryDescription()
                                            .WithAll<Player>();

        public PickupSystem(World world)
            : base(world, new QueryDescription()
                                .WithAll<PickupSprite, Position>())
        { }

        public void Update(GameTime gameTime, float totalElapsedTime, float scaleFactor)
        {
            Entity player = new Entity();
            Position? playerPos = null;
            float radiusMultiplier = 0;
            world.Query(in playerQuery, (in Entity entity, ref Position pos, ref AreaOfEffect areaOfEffect) =>
            {
                if (!playerPos.HasValue)
                {
                    player = entity;
                    playerPos = pos;
                    radiusMultiplier = areaOfEffect.Radius;
                }
            });

            if (playerPos.HasValue)
            {
                world.Query(in query, (in Entity entity, ref PickupSprite sprite, ref Position pos) =>
                {
                    if (Vector2.Distance(playerPos.Value.XY, pos.XY) < (16 * radiusMultiplier))
                    {
                        if (PickupHelper.ProcessPickup(ref player, sprite.Type))
                        {
                            world.TryDestroy(entity);
                        }
                    }
                    else
                    {
                        sprite.Count += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (sprite.Count > sprite.MaxCount)
                        {
                            sprite.Count = 0;
                            sprite.Current += sprite.Increment;

                            if (sprite.Current == sprite.Max || sprite.Current == sprite.Min)
                            {
                                sprite.Increment *= -1;
                            }
                        }
                    }
                });
            }
        }
    }
}
