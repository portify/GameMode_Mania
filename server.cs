function rm()
{
	exec("./server.cs");
}

exec("./lib/Array.cs");

exec("./src/support.cs");
exec("./src/sound.cs");
exec("./src/microgame.cs");
exec("./src/game.cs");
exec("./src/winning.cs");
exec("./src/events.cs");

exec("./src/microgames/move.cs");
exec("./src/microgames/rocket_jump.cs");
exec("./src/microgames/math.cs");
exec("./src/microgames/simon_says.cs");
exec("./src/microgames/stay_on_table.cs");
exec("./src/microgames/hit_an_enemy.cs");
exec("./src/microgames/stay_near_giant.cs");
exec("./src/microgames/avoid_kamikaze.cs");

exec("./src/microgames/boss_obby.cs");