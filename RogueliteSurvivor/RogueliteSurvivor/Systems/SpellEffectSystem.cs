using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using RogueliteSurvivor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Systems
{
    public class SpellEffectSystem : ArchSystem, IUpdateSystem
    {
        private QueryDescription burnQuery = new QueryDescription()
                                                    .WithAll<Burn, Health>();
        private QueryDescription slowQuery = new QueryDescription()
                                                    .WithAll<Slow>();
        private QueryDescription shockQuery = new QueryDescription()
                                                    .WithAll<Shock>();
        public SpellEffectSystem(World world)
            : base(world, new QueryDescription())
        {
        }

        public void Update(GameTime gameTime, float totalElapsedTime) 
        {
            world.Query(in burnQuery, (in Entity entity, ref Burn burn, ref Health health, ref Animation anim) =>
            {
                burn.TimeLeft -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                burn.NextTick -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

                if(burn.NextTick < 0)
                {
                    health.Current -= 1;
                    burn.NextTick += burn.TickRate;
                    anim.Overlay = Color.Red;

                    if(health.Current < 1) 
                    {
                        if(entity.TryGet(out Player player))
                        {
                            player.State = Constants.EntityState.Dead;
                            entity.Set(player);
                        }
                        else if(entity.TryGet(out Enemy enemy))
                        {
                            enemy.State = Constants.EntityState.ReadyToDie;
                            entity.Set(enemy);
                        }
                    }
                }

                if (burn.TimeLeft < 0)
                {
                    entity.Remove<Burn>();
                }
            });

            world.Query(in slowQuery, (in Entity entity, ref Slow slow) =>
            {
                slow.TimeLeft -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

                if (slow.TimeLeft < 0)
                {
                    entity.Remove<Slow>();
                }
            });

            world.Query(in shockQuery, (in Entity entity, ref Shock shock) =>
            {
                shock.TimeLeft -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

                if (shock.TimeLeft < 0)
                {
                    entity.Remove<Shock>();
                }
            });
        }
    }
}
