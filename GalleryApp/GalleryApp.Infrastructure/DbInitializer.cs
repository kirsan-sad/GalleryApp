using GalleryApp.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GalleryApp.Infrastructure
{
    public static class DbInitializer
    {
        public static void Initialize(GalleryContext context)
        {
            context.Database.EnsureCreated();

            if (context.Genres.Any())
            {
                return;
            }

            var genre1 = new GenreEntity {Name = "Giorgio Armani", Description = "Mens fall-winter 2020 collection"};
            var genre2 = new GenreEntity {Name = "Fendi", Description = "Mens fall-winter 2020 collection"};
            var genre3 = new GenreEntity { Name = "Chanel", Description = "Women's collection autumn-winter 2021"};

            context.Genres.AddRange(genre1, genre2, genre3);

            var photo1 = new PhotoEntity { Title = "Giorgio Armani 2020", Name = "c61fd91f-3a59-4a14-babe-d40b828fad83.jpg" };
            var photo2 = new PhotoEntity { Title = "Giorgio Armani 2020", Name = "732b58a5-2f35-400d-ae98-ff63e10a4fe5.jpg" };
            var photo3 = new PhotoEntity { Title = "Giorgio Armani 2020", Name = "e4cc6e4d-2d2e-4889-9039-2326a13198f3.jpg" };
            var photo4 = new PhotoEntity { Title = "Giorgio Armani 2020", Name = "f3b45fec-a368-4087-a047-6c10ef06693b.jpg" };
            var photo5 = new PhotoEntity { Title = "Giorgio Armani 2020", Name = "4519827d-9e6a-46f7-9312-85131a76e302.jpg" };
            var photo6 = new PhotoEntity { Title = "Giorgio Armani 2020", Name = "0f4152af-7554-4d5b-bb30-27461ab18d99.jpg" };
            var photo7 = new PhotoEntity { Title = "Giorgio Armani 2020", Name = "888c16ab-2ed6-4b9e-9cd0-b04d7c891d10.jpg" };
            var photo8 = new PhotoEntity { Title = "Giorgio Armani 2020", Name = "a3209f5c-fb54-41b1-abea-a44a922f8542.jpg" };
            var photo9 = new PhotoEntity { Title = "Giorgio Armani 2020", Name = "430ec696-90eb-4caf-8187-59cba3440c2b.jpg" };
            var photo10 = new PhotoEntity { Title = "Giorgio Armani 2020", Name = "248583fb-5ce0-4648-80dd-318249a5f129.jpg" };
            var photo11 = new PhotoEntity { Title = "Fendi 2020", Name = "02e4f070-0109-44b5-8fc5-f12f7da6d49e.jpg" };
            var photo12 = new PhotoEntity { Title = "Fendi 2020", Name = "8f64374f-6dd1-4200-8dce-aef703aab4db.jpg" };
            var photo13 = new PhotoEntity { Title = "Fendi 2020", Name = "92c6e113-e7a6-41e9-b164-1da8e54647d0.jpg" };
            var photo14 = new PhotoEntity { Title = "Fendi 2020", Name = "35197bc7-45a5-443e-b2d2-57367aeeef8c.jpg" };
            var photo15 = new PhotoEntity { Title = "Fendi 2020", Name = "f827c4d3-302d-4a1b-b4e8-61c81c7ddc3f.jpg" };
            var photo16 = new PhotoEntity { Title = "Fendi 2020", Name = "ee2cf9dc-d1c8-47a6-8c41-35329df488e4.jpg" };
            var photo17 = new PhotoEntity { Title = "Fendi 2020", Name = "6ae87ece-f8fd-406a-9f1c-01c93735170f.jpg" };
            var photo18 = new PhotoEntity { Title = "Fendi 2020", Name = "a7496a72-2c78-49a0-9426-85275626be57.jpg" };
            var photo19 = new PhotoEntity { Title = "Fendi 2020", Name = "4e051aaa-6e04-467c-b214-e5f6c0a864f5.jpg" };
            var photo20 = new PhotoEntity { Title = "Fendi 2020", Name = "575c5560-30cb-467d-ab45-e192c4abc2c2.jpg" };
            var photo21 = new PhotoEntity { Title = "Chanel 2021", Name = "78b54ac9-12f7-4cba-b8ae-532b228bfa95.jpg" };
            var photo22 = new PhotoEntity { Title = "Chanel 2021", Name = "d24d3ae7-4d31-4ef0-81e9-d8d3f9cce62f.jpg" };
            var photo23 = new PhotoEntity { Title = "Chanel 2021", Name = "f755f1d4-1a2e-40d0-af45-e620710c3b18.jpg" };
            var photo24 = new PhotoEntity { Title = "Chanel 2021", Name = "a38747b6-7e1c-4d94-bc8a-9698acbabbfa.jpg" };
            var photo25 = new PhotoEntity { Title = "Chanel 2021", Name = "e94d681e-c00d-4289-9ac6-ff9600b3e376.jpg" };
            var photo26 = new PhotoEntity { Title = "Chanel 2021", Name = "b0883f77-2662-4915-8ec6-e8fd800e46fc.jpg" };
            var photo27 = new PhotoEntity { Title = "Chanel 2021", Name = "f93b7bd2-f768-45b6-9993-bf92d92b8202.jpg" };
            var photo28 = new PhotoEntity { Title = "Chanel 2021", Name = "4138ff1f-7384-4e97-bc27-062f3a8ee8ac.jpg" };
            var photo29 = new PhotoEntity { Title = "Chanel 2021", Name = "cb41dac7-e8d2-4cc3-8f39-c695913462e2.jpg" };
            var photo30 = new PhotoEntity { Title = "Chanel 2021", Name = "337b9d6c-2295-49c6-8482-829e06e3b8cb.jpg" };

            var user = new UserEntity { Login="kirsan", Password= "kirsan" };

            context.Users.Add(user);

            context.Photos.AddRange(photo1, photo2, photo3, photo4, photo5, photo6, photo7, photo8, photo9, photo10,
                photo11, photo12, photo13, photo14, photo15, photo16, photo17, photo18, photo19, photo20,
                photo21, photo22, photo23, photo24, photo25, photo26, photo27, photo28, photo29, photo30);

            genre1.Photos.AddRange(new List<PhotoEntity>() { photo1, photo2, photo3, photo4, photo5, photo6, photo7, photo8, photo9, photo10 });
            genre2.Photos.AddRange(new List<PhotoEntity>() { photo11, photo12, photo13, photo14, photo15, photo16, photo17, photo18, photo19, photo20 });
            genre3.Photos.AddRange(new List<PhotoEntity>() { photo21, photo22, photo23, photo24, photo25, photo26, photo27, photo28, photo29, photo30 });

            context.SaveChanges();
        }
    }
}
