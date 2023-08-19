namespace HorrorShorts_Game.Controls.Entities
{
    public interface IEntity
    {
        public void Update();
        public void PreDraw();
        public void Draw();
        public void Dispose();
    }
}
