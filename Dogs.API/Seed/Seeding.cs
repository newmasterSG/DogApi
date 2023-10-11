using Dogs.Domain.Entity;

namespace Dogs.API.Seed
{
    public static class Seeding
    {
        public static List<DogEntity> Seed()
        {
            var dogs = new List<DogEntity>()
            {
                new DogEntity
                {
                    Name = "Neo",
                    Color = "red & amber",
                    TailLength = 22,
                    Weight = 32,
                },
                new DogEntity
                {
                    Name = "Jessy",
                    Color = "black & white",
                    TailLength = 7,
                    Weight = 14,
                },
            };

            return dogs;
        }
    }
}
